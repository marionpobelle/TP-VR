using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class WebHandler : MonoBehaviour
{
    [SerializeField] SpringJoint joint;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Rigidbody target;

    [Header("WebSettings")]
    [SerializeField] float webStrength = 8.5f;
    [SerializeField] float webDamper = 7f;
    [SerializeField] float webMassScale = 4.5f;
    [SerializeField] float webZipStrength = 5f;
    [SerializeField] float maxDistance = 150f;
    [SerializeField] LayerMask webMask;
    [SerializeField] LineRenderer lr;
    [SerializeField] float pullTolerance = 10f;
    [SerializeField] float pullStrength = 1f;

    bool isWebOut = false;
    bool isHoldingWeb = false;
    Vector3 lastRecordedPos;
    Vector3 lastRecordedLocalPos;

    [SerializeField]TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        target.transform.parent = null;
        lr.transform.parent = null;
        lr.transform.position = Vector3.zero;
    }

    Vector3 lastAppliedForce;

    private void Update()
    {
        if (!isWebOut)
        {
            SetTargetAtRaycast();
            Debug.DrawLine(raycastOrigin.position, raycastOrigin.position + raycastOrigin.forward * maxDistance, Color.blue);
        }
        else
        {
            Debug.DrawLine(raycastOrigin.position, target.position, Color.red); 
            lr.SetPosition(0, raycastOrigin.position);
            lr.SetPosition(1, target.position);
        }

            string text = "";
        if (textMeshProUGUI != null)
        {

            text += "Is Web Out : " + (isWebOut ? "true" : false);
            text += "\nIs holding : " + (isHoldingWeb ? "true" : false);
            text += "\nIs pulling : " + (Vector3.Distance(transform.position, target.position) > Vector3.Distance(lastRecordedPos, target.position) ? "true" : false);
            text += "\nIs In Tolerance : " + (Vector3.Distance(playerRigidbody.transform.position, target.position) < pullTolerance ? "true" : false);

        }

        if (isWebOut && isHoldingWeb)
        {
            //if we are pulling && if we are close to the max distance of the joint
            if (Vector3.Distance(transform.position, target.position) > Vector3.Distance(lastRecordedPos, target.position)
                && (Vector3.Distance(playerRigidbody.transform.position, target.position) > joint.maxDistance - pullTolerance
                && Vector3.Distance(playerRigidbody.transform.position, target.position) < joint.maxDistance + pullTolerance))
            {
                Vector3 force = (target.position - playerRigidbody.transform.position).normalized *  (transform.position.y > target.position.y ? -1 : 1 )*Vector3.Dot(target.position - playerRigidbody.transform.position, lastRecordedLocalPos) * pullStrength;
                lastAppliedForce = force;
                playerRigidbody.AddForce(force, ForceMode.Force);
                lastRecordedPos = transform.position;
                lastRecordedLocalPos = transform.localPosition;
            }
        }

        if (textMeshProUGUI != null)
        {
            text += "\nLastAppliedForce : " + lastAppliedForce;
            textMeshProUGUI.text = text;
        }


        }

    private void SetTargetAtRaycast()
    {
        target.position = GetRaycastHit().point;
    }

    public RaycastHit GetRaycastHit()
    {
        RaycastHit hit;

        Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, maxDistance, webMask);

        return hit;
    }

    public void OnWebInput(bool isPressed)
    {
        if (isPressed)
        {
            AttemptWebShoot();
        }
        else
        {
            ResetWeb();
        }
    }

    public void OnHoldInput(bool isHolding)
    {
        isHoldingWeb = isWebOut && isHolding;

        if (isHoldingWeb)
        {
            lastRecordedPos = transform.position;
            lastRecordedLocalPos = transform.localPosition;
        }
    }

    private void AttemptWebShoot()
    {
        RaycastHit hit = GetRaycastHit();
        if (hit.collider != null)
        {
            target.position = hit.point;
            joint = playerRigidbody.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedBody = target;
            joint.spring = webStrength;
            joint.damper = webDamper;
            joint.massScale = webMassScale;
            joint.minDistance = 0;
            joint.maxDistance = Vector3.Distance(target.position, joint.transform.position);
            isWebOut = true;

            lr.enabled = true;
            lr.SetPosition(0, raycastOrigin.position);
            lr.SetPosition(1, target.position);
        }
        else
        {
            ResetWeb();
        }
    }

    private void ResetWeb()
    {
        lr.enabled = false;
        isWebOut = false;
        isHoldingWeb = false;
        Destroy(joint); 
    }
}

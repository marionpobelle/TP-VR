using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

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

    bool isWebOut = false;

    private void Awake()
    {
        target.transform.parent = null;
        lr.transform.parent = null;
        lr.transform.position = Vector3.zero;
    }

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
        Destroy(joint);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class WebHandler : MonoBehaviour
{
    [SerializeField] XRBaseController controller;
    [SerializeField] SpringJoint joint;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Rigidbody target;
    [SerializeField] Animator animator;

    [Header("WebSettings")]
    [SerializeField] float webStrength = 8.5f;
    [SerializeField] float webDamper = 7f;
    [SerializeField] float webMassScale = 4.5f;
    [SerializeField] float webZipStrength = 5f;
    [SerializeField] float maxDistance = 150f;
    [SerializeField] LayerMask webMask;
    [SerializeField] LineRenderer lr;
    [SerializeField] LineRenderer previewLR;
    [SerializeField] float pullTolerance = 10f;
    [SerializeField] float pullStrength = 1f;
    [SerializeField] float minPullDistance = 1f;
    [SerializeField] float vibrationStrength = .2f;

    public bool isWebOut { get; private set; }
    bool isHoldingWeb;
    Vector3 pullStartPos;
    Vector3 lastRecordedPos;
    Vector3 lastRecordedLocalPos;

    AudioManager audioManager;

    private void Awake()
    {
        isHoldingWeb = false;
        target.transform.parent = null;
        lr.transform.parent = null;
        lr.transform.position = Vector3.zero;
        previewLR.transform.parent = null;
        previewLR.transform.position = Vector3.zero;

        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (!isWebOut)
        {
            SetTargetAtRaycast();
        }
        else
        {
            Debug.DrawLine(raycastOrigin.position, target.position, Color.red);
            lr.SetPosition(0, raycastOrigin.position);
            lr.SetPosition(1, target.position);
        }

        if (isWebOut && (Vector3.Distance(playerRigidbody.transform.position, target.position) > joint.maxDistance - pullTolerance
                && Vector3.Distance(playerRigidbody.transform.position, target.position) < joint.maxDistance + pullTolerance)
                && playerRigidbody.velocity.sqrMagnitude > .1f)
        {
            controller.SendHapticImpulse(vibrationStrength, .05f);
        }

        if (isWebOut && isHoldingWeb)
        {
            //if we are pulling && if we are close to the max distance of the joint
            if (Vector3.Distance(transform.position, target.position) > Vector3.Distance(lastRecordedPos, target.position)
                && (Vector3.Distance(playerRigidbody.transform.position, target.position) > joint.maxDistance - pullTolerance
                && Vector3.Distance(playerRigidbody.transform.position, target.position) < joint.maxDistance + pullTolerance)
                && Vector3.Distance(pullStartPos, transform.localPosition) > minPullDistance)
            {
                Vector3 force = (target.position - playerRigidbody.transform.position).normalized * Vector3.Dot(target.position - playerRigidbody.transform.position, lastRecordedLocalPos - transform.localPosition) * pullStrength;
                playerRigidbody.AddForce(force, ForceMode.Force);
                lastRecordedPos = transform.position;
                lastRecordedLocalPos = transform.localPosition;
            }
        }
    }

    private void SetTargetAtRaycast()
    {
        RaycastHit hit = GetRaycastHit();
        if (hit.collider != null)
        {
            previewLR.startColor = Color.green;
            previewLR.endColor = Color.green;
            target.position = GetRaycastHit().point;
            previewLR.SetPosition(0, raycastOrigin.position);
            previewLR.SetPosition(1, target.position);
        }
        else
        {
            previewLR.startColor = Color.red;
            previewLR.endColor = Color.red;
            previewLR.SetPosition(0, raycastOrigin.position);
            previewLR.SetPosition(1, raycastOrigin.position + raycastOrigin.forward * maxDistance);
        }
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
            animator.SetBool("IsHolding", true);
            pullStartPos = transform.localPosition;
            lastRecordedPos = transform.position;
            lastRecordedLocalPos = transform.localPosition;
        }
        else
        {
            animator.SetBool("IsHolding", false);
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
            animator.SetBool("IsWebOut", true);
            previewLR.enabled = false;

            int randWebSound = UnityEngine.Random.Range(0, 5);
            audioManager.PlayOneShot("Web" + randWebSound);
        }
        else
        {
            ResetWeb();
        }
    }

    private void ResetWeb()
    {
        animator.SetBool("IsWebOut", false);
        animator.SetBool("IsHolding", false);
        lr.enabled = false;
        isWebOut = false;
        isHoldingWeb = false;
        previewLR.enabled = true;
        Destroy(joint);
    }
}

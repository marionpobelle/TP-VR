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
    public float webStrength = 8.5f;
    public float webDamper = 7f;
    public float webMassScale = 4.5f;
    public float webZipStrength = 5f;
    public float maxDistance = 150f;
    public LayerMask webMask;

    bool isWebOut = false;

    private void Awake()
    {
        target.transform.parent = null;
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
        }
        else
        {
            ResetWeb();
        }
    }

    private void ResetWeb()
    {
        isWebOut = false;
        Destroy(joint);
    }
}

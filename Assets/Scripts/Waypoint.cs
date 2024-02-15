using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public float waypointRadius = 1.0f;
    [SerializeField]
    public CapsuleCollider waypointCollider;
    public TimeAttackHandler handler;
    [SerializeField]
    GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            handler.OnWaypointCrossed(this);
    }

    public void SetParticleState(bool enable)
    {
        particles.SetActive(enable);
    }

#if UNITY_EDITOR

    [Button("Select handler")]
    private void SelectHandler()
    {
        Selection.activeGameObject = this.handler.gameObject;
    }
#endif
}

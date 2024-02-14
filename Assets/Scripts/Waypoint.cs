using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public float waypointRadius = 1.0f;
    [SerializeField]
    public CapsuleCollider waypointCollider;
    public TimeAttackHandler handler;

    public bool nextInRace = false;
    [SerializeField]
    GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        handler.OnWaypointCrossed(this);
    }

    private void Update()
    {
        if(nextInRace == true)
        {
            particles.SetActive(true);
        }
        else
        {
            particles.SetActive(false);
        }
    }

#if UNITY_EDITOR

    [Button("Select handler")]
    private void SelectHandler()
    {
        Selection.activeGameObject = this.handler.gameObject;
    }
#endif
}

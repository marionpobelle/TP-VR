using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TimeAttackHandler : MonoBehaviour
{
    [SerializeField]
    TimeAttackData data;

    [SerializeField]
    Waypoint prefabWaypoint;

    [SerializeField]
    float defaultWaypointRadius = 1.0f;

    [SerializeField]
    List<Waypoint> waypoints;
    AudioManager audioManager;

    bool isRaceStarted = false;
    int currentWaypointIndex;

    private void Awake()
    {
        LoadRaceCourse();

        audioManager = FindObjectOfType<AudioManager>();
    }

    [Button]
    public void StartRace()
    {
        isRaceStarted = true;
        EnableWaypoint(0);
        Debug.Log("Race is starting");
    }

    void OnRaceOver()
    {
        isRaceStarted = false;
        Debug.Log("Race is over");
    }

    public void OnWaypointCrossed(Waypoint crossedWaypoint)
    {
        int crossedWaypointIndex = waypoints.IndexOf(crossedWaypoint);
        Debug.Log($"Crossed waypoint {crossedWaypointIndex}");
        DisableWaypoint(crossedWaypointIndex);
        audioManager.PlayOneShot("RightWaypoint");

        if (crossedWaypointIndex >= waypoints.Count - 1)
        {
            OnRaceOver();
        }
        else
        {
            EnableWaypoint(crossedWaypointIndex + 1);
        }
    }

    private void EnableWaypoint(int waypointIndex)
    {
        waypoints[waypointIndex].SetParticleState(true);
        waypoints[waypointIndex].gameObject.SetActive(true);
    }

    private void DisableWaypoint(int waypointIndex)
    {
        waypoints[waypointIndex].SetParticleState(false);
        waypoints[waypointIndex].gameObject.SetActive(false);
    }

    void LoadRaceCourse()
    {
        //Load data
        foreach (var waypointData in data.waypointsData)
        {
            Waypoint newWaypoint = Instantiate(prefabWaypoint);
            newWaypoint.name = "Waypoint" + waypoints.Count;
            newWaypoint.waypointRadius = waypointData.Radius;
            newWaypoint.waypointCollider.radius = waypointData.Radius;
            newWaypoint.handler = this;
            newWaypoint.transform.position = waypointData.Position;
            waypoints.Add(newWaypoint);
            DisableWaypoint(waypoints.Count -1);
        }
    }

#if UNITY_EDITOR
    [Button("Add waypoint")]
    private void AddWaypointToRace()
    {
        Waypoint newWaypoint = Instantiate(prefabWaypoint);
        newWaypoint.name = "Waypoint" + waypoints.Count;
        newWaypoint.waypointRadius = defaultWaypointRadius;
        newWaypoint.waypointCollider.radius = newWaypoint.waypointRadius;
        newWaypoint.handler = this;

        waypoints.Add(newWaypoint);
        Selection.activeGameObject = newWaypoint.gameObject;
    }

    [Button("Remove last waypoint")]
    private void RemoveLastWaypointFromRace()
    {
        DestroyImmediate(waypoints[waypoints.Count - 1].gameObject);
        waypoints.RemoveAt(waypoints.Count - 1);
    }

    [Button("Save race course")]
    private void SaveRaceCourse()
    {
        //Clear data
        data.ClearTimeAttackData();
        //Save current list
        foreach (var waypoint in waypoints)
        {
            WaypointData currentWaypoint = new WaypointData();
            currentWaypoint.Position = waypoint.transform.position;
            currentWaypoint.Radius = waypoint.waypointRadius;
            data.waypointsData.Add(currentWaypoint);
            DestroyImmediate(waypoint.gameObject);
        }
        //Empty current list
        waypoints = new List<Waypoint>();
    }

    //GIZMOS
    void OnDrawGizmos()
    {
        foreach (var waypoint in waypoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(waypoint.transform.position, waypoint.waypointRadius);
        }
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.color = Color.blue;
            if (i == waypoints.Count - 1)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[0].transform.position);
            }
            else
            {
                Gizmos.DrawLine(waypoints[i].gameObject.transform.position, waypoints[i + 1].gameObject.transform.position);
            }
        }

    }


#endif
}

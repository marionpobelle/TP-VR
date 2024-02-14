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
    LineRenderer lineRenderer;
    GameObject player;

    [SerializeField]
    float defaultWaypointRadius = 1.0f;

    [SerializeField]
    List<Waypoint> waypoints;

    private void Awake()
    {
        //player = FindObjectOfType<InputHandler>().gameObject;
        LoadRaceCourse();
    }

    private void Update()
    {
        if(waypoints.Count > 0)
        {
            //lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
            //lineRenderer.SetPosition(1, new Vector3(waypoints[0].transform.position.x, waypoints[0].transform.position.y, waypoints[0].transform.position.z));
        }
    }

    public void OnWaypointCrossed(Waypoint crossedWaypoint)
    {
        //verifier si c'est le premier waypoint de la liste de l'handler
        if (waypoints[0] == crossedWaypoint)
        {
            Destroy(waypoints[0].gameObject);
            waypoints.RemoveAt(0);
            FindObjectOfType<AudioManager>().PlayOneShot("RightWaypoint");
            waypoints[0].nextInRace = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().PlayOneShot("WrongWaypoint");
        }
    }

    void LoadRaceCourse()
    {
        //Load data
        foreach(var waypointData in data.waypointsData)
        {
            Waypoint newWaypoint = Instantiate(prefabWaypoint);
            newWaypoint.name = "Waypoint" + waypoints.Count;
            newWaypoint.waypointRadius = waypointData.Radius;
            newWaypoint.waypointCollider.radius = waypointData.Radius;
            newWaypoint.handler = this;
            newWaypoint.transform.position = waypointData.Position;
            waypoints.Add(newWaypoint);
        }
        waypoints[0].nextInRace = true;
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
        foreach(var waypoint in waypoints)
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
        for(int i= 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.color = Color.blue;
            if(i == waypoints.Count -1)
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

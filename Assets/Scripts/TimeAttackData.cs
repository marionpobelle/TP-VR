using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimeAttackData : ScriptableObject
{
    public List<WaypointData> waypointsData;

    public void ClearTimeAttackData()
    {
        waypointsData = new List<WaypointData>();
    }
}

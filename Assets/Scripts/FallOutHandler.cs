using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FallOutHandler : MonoBehaviour
{
    public static event Action OnPlayerFall;

    Transform playerTransform;
    Rigidbody rb;
    [SerializeField]
    float fallHeightThreshold = -50.0f;

    [SerializeField]
    List<Transform> spawnPoints;

    [SerializeField] WebHandler leftWebHandler;
    [SerializeField] WebHandler rightWebHandler;

    private void Awake()
    {
        playerTransform = FindObjectOfType<InputHandler>().transform;
        rb = playerTransform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.y < fallHeightThreshold)
        {
            if (!leftWebHandler.isWebOut && !rightWebHandler.isWebOut)
            {
                Transform closestSpawnTransform = spawnPoints[0];
                float closestDistance = EuclidianDistance(playerTransform, spawnPoints[0]);
                float tempDistance;
                foreach( Transform t in spawnPoints )
                {
                    tempDistance = EuclidianDistance(playerTransform, t);
                    if(tempDistance < closestDistance )
                    {
                        closestSpawnTransform = t;
                    }
                }
                OnPlayerFall?.Invoke();
                playerTransform.position = closestSpawnTransform.position;
                rb.velocity = Vector3.zero;
            }
        }
    }

    float EuclidianDistance(Transform objectA, Transform objectB)
    {
        float x = (objectA.position.x - objectB.position.x) * (objectA.position.x - objectB.position.x);
        float y = (objectA.position.y - objectB.position.y) * (objectA.position.y - objectB.position.y);
        float z = (objectA.position.z - objectB.position.z) * (objectA.position.z - objectB.position.z);
        return Mathf.Sqrt(x + y + z);
    }
}

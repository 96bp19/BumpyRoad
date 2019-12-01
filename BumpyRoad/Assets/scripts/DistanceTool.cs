using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTool : MonoBehaviour
{
    public static DistanceTool Instance;
    float distanceToTravel;
    Transform startingPosition;
    Transform endPosition;
    Transform playerPosition;
    GameObject player;
    float traveledByPlayer;
    float finalValue;

    private void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (startingPosition)
        {
            playerPosition = player.transform;
            traveledByPlayer = Vector3.Distance(startingPosition.position, playerPosition.position);

            finalValue = traveledByPlayer / distanceToTravel;

            Debug.Log("Distance Travelled" + finalValue);
        }
    }

    public void InitializeDistance(Transform a, Transform b)
    {
        startingPosition = a;
        endPosition = b;

        distanceToTravel = Vector3.Distance(a.position, b.position);
    }

}

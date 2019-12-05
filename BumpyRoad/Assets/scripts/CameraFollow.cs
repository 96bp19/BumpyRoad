using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject ObjectToFollow;
    public Vector3 followOffset;
    
    void Start()
    {
        ObjectToFollow = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectToFollow == null)
        {
            ObjectToFollow = GameObject.FindGameObjectWithTag("Player");
        }
        transform.position = ObjectToFollow.transform.position + followOffset;


    }
}

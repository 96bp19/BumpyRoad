using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public Vector3 followOffset;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ObjectToFollow.transform.position + followOffset;
    }
}

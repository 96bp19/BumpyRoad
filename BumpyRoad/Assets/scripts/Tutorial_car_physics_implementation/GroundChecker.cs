using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool groundDetected;
    public Transform mainController;
    public Vector3 locationOffset;


    private void Update()
    {
        transform.position = mainController.position + locationOffset;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("balls"))
        {
            groundDetected = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("balls"))
        {
            groundDetected = false;

        }
    }
}

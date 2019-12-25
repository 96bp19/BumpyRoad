using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool groundDetected;

    

    private void OnTriggerStay(Collider other)
    {
        groundDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        groundDetected = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravitySetter : MonoBehaviour
{
    public float newGravityMultiplier = 5;
    Rigidbody rb;
    Vector3 newGravity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        newGravity = Physics.gravity * (newGravityMultiplier - 1);
        rb.AddForce(newGravity, ForceMode.Acceleration);
    }
}

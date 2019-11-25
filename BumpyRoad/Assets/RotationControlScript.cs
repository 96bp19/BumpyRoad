using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControlScript : MonoBehaviour
{
    public float rotationSpeed =10;
    public float rotationSmoothness=2;
    public float raycastDistance=100;

    Vector3 raycastPos;
    RaycastHit hit;

    bool allowedToRotate;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        raycastPos = transform.position;
        bool groundDetected = false;
        groundDetected = Physics.Raycast(raycastPos, Vector3.down, out hit, raycastDistance);
        if (!groundDetected)
        {
            groundDetected = Physics.Raycast(raycastPos, -transform.up, out hit, raycastDistance);
        }
        if (groundDetected)
        {
            Debug.DrawRay(hit.point, hit.normal * raycastDistance, Color.blue);
        }

        if (!groundDetected)
        {
            allowedToRotate = true; 

        }
        else
        {
            allowedToRotate = false;
        }

        RotateVehicle();
    }

    void RotateVehicle()
    {
//         if (!allowedToRotate)
//         {
//             return;
//         }

        float inputX = Input.GetAxis("Vertical")*rotationSpeed*Time.fixedDeltaTime;
        //transform.Rotate(inputX, 0, 0);

        rb.AddTorque(transform.right * -inputX *rb.mass* Time.fixedDeltaTime);
        Debug.Log("torque added");
    }
}

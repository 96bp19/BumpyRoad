using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControlScript : MonoBehaviour
{
    public float controlledRotatiobSpeed =10;
    public float rotationSmoothness=2;
    public float raycastDistance=100;

    Vector3 raycastPos;
    RaycastHit hit;

    bool allowedToRotate;

    Rigidbody rb;


    public float selfRotationSpeed = 50f;
    public bool usePhysicsSimulatedRotation;


    public float manualSelfRotation = 5f;
    public float manualControlledRotation = 10f;

    public LayerMask groundlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        raycastPos = transform.position;
        bool groundDetected = false;
        Debug.DrawRay(raycastPos, Vector3.down * raycastDistance, Color.red);
        groundDetected = Physics.Raycast(raycastPos, -transform.up, out hit, raycastDistance, groundlayer);
        //         groundDetected = Physics.Raycast(raycastPos, Vector3.down, out hit, raycastDistance,groundlayer);
        //         if (!groundDetected)
        //         {
        //             groundDetected = Physics.Raycast(raycastPos, -transform.up, out hit, raycastDistance,groundlayer);
        //         }
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

        if (usePhysicsSimulatedRotation)
        {
            RotateVehicle();

        }
        else
        {
            ApplyManualRotation();
        }

    }

    void RotateVehicle()
    {
        //         if (!allowedToRotate)
        //         {
        //             return;
        //         }



        if (InputHandler.IsScreenTapped())
        {
            rb.AddTorque(-transform.right  *rb.mass*controlledRotatiobSpeed* Time.fixedDeltaTime);

        }
        else
        {
            rb.AddTorque(transform.right * rb.mass *selfRotationSpeed* Time.fixedDeltaTime);
        }
       
    }

    private void FixedUpdate()
    {
        AddExtraGravity();
    }

    void ApplyManualRotation()
    {
        if (!allowedToRotate)
        {
            return;
        }
        //rb.AddTorque(-rb.angularVelocity * rb.mass * manualControlledRotation * Time.fixedDeltaTime);
        //         
        if (InputHandler.IsScreenTapped())
        {
            rb.angularVelocity = Vector3.zero;
            transform.Rotate(manualControlledRotation, 0, 0);
            Debug.Log("manual rotation applied");

        }
        else
        {

            transform.Rotate(-manualSelfRotation, 0, 0);
        }

        if (InputHandler.IsScreenTapped())
        {
            Debug.Log("torque added");

        }

    }

    Vector3 gravityForce;
    public float newGravityMultiplier;
    void AddExtraGravity()
    {
        gravityForce = Physics.gravity*(newGravityMultiplier - 1);
        rb.AddForce(gravityForce*rb.mass);
    }
}

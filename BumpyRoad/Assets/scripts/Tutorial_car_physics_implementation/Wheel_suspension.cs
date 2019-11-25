﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_suspension : MonoBehaviour
{
    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;


    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    private Vector3  suspensionForce;
    private Vector3 wheelVelocity_localSpace;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;

    private Rigidbody rb;

    [Header("Wheels")]
    public float wheelRadius;
    public float steerAngle;

    public float wheelAngle;
    public float wheelTurnSmoothness=10;


    public float force_x;
    public float force_y;

    public Transform wheelMesh;

    public float wheelRotateSpeed=10;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;

    }

    private void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, wheelTurnSmoothness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(transform.up*wheelAngle);

        Debug.DrawRay(transform.position, -transform.up * (maxLength + wheelRadius), Color.green);

        CalculateWheelPos();
        calculateWheelRotation();
    }


    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position ,-transform.up,out RaycastHit hit,maxLength +wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            wheelVelocity_localSpace = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            force_x = Input.GetAxis("Vertical") * springForce;
            force_y = wheelVelocity_localSpace.x * springForce;



            rb.AddForceAtPosition(suspensionForce +(force_x *transform.forward) +(force_y *-transform.right), hit.point);

            
        }
       
    }

    void CalculateWheelPos()
    {
        //wheelMesh.position = transform.position-  new Vector3(0, springLength, 0);
        Vector3 newWheelPos = transform.position;
        newWheelPos.y = transform.position.y - springLength;
        wheelMesh.position = newWheelPos;
    }

    void calculateWheelRotation()
    {
        Vector3 rotation = wheelMesh.rotation.eulerAngles;

        
        wheelMesh.Rotate(-Mathf.Sign(rb.velocity.z) * rb.velocity.magnitude*wheelRotateSpeed * Time.fixedDeltaTime, 0, 0);
    }

   
}

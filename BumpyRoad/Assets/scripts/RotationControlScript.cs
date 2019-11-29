using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControlScript : MonoBehaviour
{
   
    public float raycastDistance=100;
    public float manualSelfRotation = 5f;
    public float manualControlledRotation = 10f;
    public LayerMask groundlayer;
    public GameObject backWheelTrans , frontWheelTrans;
    public float maxSpeed = 100f;

    private Vector3 raycastPos;
    private RaycastHit hit;
    private bool allowedToRotate;
    private Rigidbody rb;
    
   


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
    }

    

    private void Update()
    {
        if (GameOverChecker .gameOver)
        {
            return;
        }
        raycastPos = transform.position;
        bool groundDetected = false;
        groundDetected = Physics.Raycast(raycastPos, -transform.up, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(backWheelTrans.transform.position, -transform.up, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(frontWheelTrans.transform.position, -transform.up, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(raycastPos, Vector3.down, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(backWheelTrans.transform.position, Vector3.down, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(frontWheelTrans.transform.position, Vector3.down, out hit, raycastDistance, groundlayer);

       
        if (groundDetected)
        {
            Debug.DrawRay(hit.point, hit.normal * raycastDistance, Color.blue);
        }

        if  (!groundDetected) allowedToRotate = true;
        else                  allowedToRotate = false;

        ApplyManualRotation();

    }

    private void FixedUpdate()
    {
        AddExtraGravity();
        LimitVehicleVelocity();
    }

    void ApplyManualRotation()
    {
        if (!allowedToRotate)
        {
            return;
        }
      
        rb.angularVelocity = Vector3.zero;
        if (InputHandler.IsScreenTapped())
        {
            transform.Rotate(manualControlledRotation, 0, 0);
        }
        else
        {
            transform.Rotate(-manualSelfRotation, 0, 0);
        }
    }

    Vector3 gravityForce;
    public float newGravityMultiplier;
    void AddExtraGravity()
    {
        gravityForce = Physics.gravity*(newGravityMultiplier - 1);
        rb.AddForce(gravityForce*rb.mass);
    }

    void LimitVehicleVelocity()
    {
        Vector3 velocity = rb.velocity;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        rb.velocity = velocity;
    }
    
}

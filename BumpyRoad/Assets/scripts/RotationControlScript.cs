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

    public GroundChecker groundChecker;
    public GameObject[] groundCheckers;
    public Vector3 newNormal;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
    }

    

    private void Update()
    {
        if (GameOverChecker .gameOver)
        {
            rb.constraints = RigidbodyConstraints.None;
            return;
        }
        raycastPos = transform.position;
        bool groundDetected = false;
        groundDetected = groundChecker.groundDetected;
       
        if (groundDetected)
        {
            Debug.DrawRay(hit.point, hit.normal * raycastDistance, Color.blue);
        }

        if  (!groundDetected) allowedToRotate = true;
        else                  allowedToRotate = false;

        ApplyManualRotation();

    }

    void calculateVehicleRotation()
    {
        newNormal = Vector3.zero;

        for (int i = 0; i < groundCheckers.Length; i++)
        {
            Debug.DrawRay(groundCheckers[i].transform.position, Vector3.down * raycastDistance, Color.black);
           if (Physics.Raycast(groundCheckers[i].transform.position, Vector3.down, out RaycastHit hitinfo, raycastDistance,groundlayer))
            {
                newNormal += hitinfo.normal;
                
            }
          
        }
      
        newNormal.Normalize();
        transform.up -= (transform.up - newNormal) * 0.2f;
        // transform.up = newNormal;
    }

    private void FixedUpdate()
    {
        AddExtraGravity();
        MoveVehicle();
        LimitVehicleVelocity();
       // calculateVehicleRotation();
      //  transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
       
    }

    void TryAlignWithGround()
    {
        if (!groundChecker.groundDetected)
        {
            return;
        }
        if (Physics.Raycast(transform.position, -transform.up,out RaycastHit hitinfo))
        {
            transform.up -= (transform.up - hitinfo.normal) * 0.1f;
        }
    }

    void ApplyManualRotation()
    {
        if (!allowedToRotate)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
            return;
        }
        rb.freezeRotation = true;
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
    public float currentSpeed;
    public float acceleration = 60;
    public float deacceleration = 30f;

    public Vector3 currentTransVal;
    void LimitVehicleVelocity()
    {
        if (!groundChecker.groundDetected)
        {
            return;
        }
        currentTransVal = transform.forward;
        float desiredSpeed = transform.forward.y*maxSpeed;
        if (InputHandler.IsScreenTapped())
        {
            desiredSpeed = -1 * maxSpeed;
            if (currentSpeed > 0)
            {
                currentSpeed = 0;
            }
                currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            if (Mathf.Abs(transform.forward.z) == 1)
            {
                // straight
                desiredSpeed = 0;
                currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, deacceleration * Time.deltaTime);
            }
            else
            {
                desiredSpeed = transform.forward.y * maxSpeed;
                currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, deacceleration * Time.deltaTime);
            }
        }

        Vector3 velocity = rb.velocity;
        velocity = Vector3.ClampMagnitude(velocity, Mathf.Abs(currentSpeed));
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }


    void MoveVehicle()
    {
        if (InputHandler.IsScreenTapped() && !GameOverChecker.gameOver && groundChecker.groundDetected)
        {
            Vector3 newVEl = -transform.forward * 100 * Time.fixedDeltaTime;
            rb.AddForce(newVEl, ForceMode.VelocityChange);

        }
        
    }



}

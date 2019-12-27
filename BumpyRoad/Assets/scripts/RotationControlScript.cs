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
            return;
        }
        raycastPos = transform.position;
        bool groundDetected = false;
        //         groundDetected = Physics.Raycast(raycastPos, -transform.up, out hit, raycastDistance, groundlayer) ||
        //                          Physics.Raycast(backWheelTrans.transform.position, -transform.up, out hit, raycastDistance, groundlayer) ||
        //                          Physics.Raycast(frontWheelTrans.transform.position, -transform.up, out hit, raycastDistance, groundlayer) ||
        //                          Physics.Raycast(raycastPos, Vector3.down, out hit, raycastDistance, groundlayer) ||
        //                          Physics.Raycast(backWheelTrans.transform.position, Vector3.down, out hit, raycastDistance, groundlayer) ||
        //                          Physics.Raycast(frontWheelTrans.transform.position, Vector3.down, out hit, raycastDistance, groundlayer);
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
            return;
        }

   //     Vector3 previousAngularvel = rb.angularVelocity;
        float desiredJumpVel = transform.InverseTransformDirection(rb.angularVelocity).x;
        

                rb.angularVelocity = Vector3.zero;
        if (InputHandler.IsScreenTapped())
        {
             transform.Rotate(manualControlledRotation, 0, 0);
           // desiredJumpVel = manualControlledRotation - desiredJumpVel;
         //   Vector3 newAngularvel = Vector3.right * manualControlledRotation *Time.fixedDeltaTime;
          //  rb.angularVelocity = newAngularvel;
        //    rb.AddTorque(newAngularvel, ForceMode.VelocityChange);
            
        }
        else
        {
             transform.Rotate(-manualSelfRotation, 0, 0);
//             desiredJumpVel = manualSelfRotation - desiredJumpVel ;
//             Vector3 newAngularvel = Vector3.right * manualSelfRotation * Time.fixedDeltaTime;
//             rb.angularVelocity = newAngularvel;
           // rb.AddTorque(newAngularvel, ForceMode.VelocityChange);
        }

       // rb.maxAngularVelocity = manualControlledRotation * Time.fixedDeltaTime;
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
        }
        currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, acceleration * Time.deltaTime);
        Vector3 velocity = rb.velocity;
        velocity = Vector3.ClampMagnitude(velocity, Mathf.Abs(currentSpeed));
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPhysicsCarController : MonoBehaviour
{
    public float movingForce =2;
    public float raycastDistance;


    public GroundChecker groundChecker;
    public GameObject[] groundCheckers;
    public LayerMask groundLayer;


    public Vector3 up_projection, forwardProjection;
    public bool groundDetected;
    private Rigidbody rb;

    public GameObject vehicleMesh;
    public Vector3 vehicleLocationOffset;

    public float rotationSpeed = 0.8f;
    public float groundRotationSmoothness =10;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void checkGroundedStatus()
    {
        groundDetected = groundChecker.groundDetected;
    }

    private void Update()
    {
        CalculateRaycasts();
      
        MoveVehicle();
        adjustVehicleMesh();
        BalanceVehicle();
    }

    private void FixedUpdate()
    {
        checkGroundedStatus();
        ApplyManualVehicleRotation();
    }

    void adjustVehicleMesh()
    {

        vehicleMesh.transform.localPosition = transform.localPosition + vehicleMesh.transform.up *vehicleLocationOffset.y;
        
    }

   

    void CalculateRaycasts()
    {

          up_projection = forwardProjection = Vector3.zero;
        bool detected = false;
        for (int i = 0; i < groundCheckers.Length; i++)
        {
            Debug.DrawRay(groundCheckers[i].transform.position, Vector3.down * raycastDistance, Color.red);
            if (Physics.Raycast(groundCheckers[i].transform.position, Vector3.down, out RaycastHit hitInfo, raycastDistance, groundLayer))
            {
                
                forwardProjection += Vector3.ProjectOnPlane(transform.forward, hitInfo.normal);
                  up_projection += MyMath.SmoothedNormal(hitInfo);
                up_projection += hitInfo.normal;
                detected = true;

            }
        }
        if (detected == false)
        {
            forwardProjection = Vector3.forward;
            up_projection = Vector3.up;
        }
        forwardProjection.Normalize();
        up_projection.Normalize();
    }

     


    private float angle;
    void ApplyManualVehicleRotation()
    {
        if (!groundDetected)
        {
            return;
        }
        // vehicleMesh.transform.up -= (vehicleMesh.transform.up - up_projection) * 0.2f;
       // vehicleMesh.transform.up = Vector3.Lerp(vehicleMesh.transform.up, up_projection, Time.deltaTime * groundRotationSmoothness);
      
        
         angle= Vector3.SignedAngle( Vector3.up, up_projection, Vector3.right);
        // float lerpedAngle = Mathf.LerpAngle(vehicleMesh.transform.eulerAngles.x, angle, Time.deltaTime*groundRotationSmoothness);

        float lerpedAngle = Mathf.MoveTowardsAngle(vehicleMesh.transform.eulerAngles.x, angle, Time.deltaTime * groundRotationSmoothness);
       
        vehicleMesh.transform.eulerAngles = new Vector3(lerpedAngle, 0, 0);
    }

    void BalanceVehicle()
    {
        if (groundDetected)
        {
            return;
        }

       
        if (InputHandler.IsScreenTapped())
        {

            vehicleMesh.transform.Rotate(rotationSpeed, 0, 0);
        }
        else
        {
            vehicleMesh.transform.Rotate(-rotationSpeed, 0, 0);
        }
    }

    public float maxVehicleSpeed = 60;
    public float acceleration = 2f;
    float currentSpeed;
    
    void MoveVehicle()
    {
        if (groundDetected)
        {
           
            float currentAcceleration = vehicleMesh.transform.up.z;
            if (InputHandler.IsScreenTapped())
            {
                currentAcceleration = -1;
                rb.AddForce(-vehicleMesh.transform.forward * movingForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            float desiredvelocity = maxVehicleSpeed * currentAcceleration;
            currentSpeed = Mathf.MoveTowards(currentSpeed, desiredvelocity, acceleration * Time.fixedDeltaTime);
            Vector3 vel = Vector3.ClampMagnitude(rb.velocity, Mathf.Abs(currentSpeed));
            
            rb.velocity = vel;

        }
    }
    

}

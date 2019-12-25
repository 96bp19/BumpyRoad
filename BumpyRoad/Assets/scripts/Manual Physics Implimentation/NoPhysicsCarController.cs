using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPhysicsCarController : MonoBehaviour
{
    public float moveSpeed =2;
    public float raycastDistance;

     GameObject[] groundCheckers;
    public LayerMask groundLayer;


    public Vector3 up_projection, forwardProjection;
    private bool groundDetected;
    private Rigidbody rb;

    public GameObject vehicleMesh;
    public Vector3 vehicleLocationOffset;

    public float rotationSpeed = 0.8f;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CalculateRaycasts();
      
        MoveVehicle();
        adjustVehicleMesh();
        BalanceVehicle();
    }

    void adjustVehicleMesh()
    {

        vehicleMesh.transform.localPosition = transform.localPosition + vehicleLocationOffset;
    }

    void CalculateRaycasts()
    {
      
//       //  up_projection = forwardProjection = Vector3.zero;
//         for (int i = 0; i < groundCheckers.Length; i++)
//         {
//             Debug.DrawRay(groundCheckers[i].transform.position, Vector3.down * raycastDistance, Color.red);
//             if (Physics.Raycast(groundCheckers[i].transform.position, Vector3.down, out RaycastHit hitInfo,raycastDistance,groundLayer))
//             {
//                 groundDetected = true;
//                 forwardProjection += Vector3.ProjectOnPlane(transform.forward, hitInfo.normal);
//               //  up_projection += hitInfo.normal; 
// 
//             }
//         }
    //    forwardProjection.Normalize();
     //   up_projection.Normalize();
    }

     


    public float angle;
    void ApplyManualVehicleRotation()
    {
        if (!groundDetected)
        {
            return;
        }
        // vehicleMesh.transform.up -= (vehicleMesh.transform.up - up_projection) * 0.2f;
         angle= Vector3.Angle(up_projection, Vector3.up);
        Debug.Log("angle : " + angle);
        //  Quaternion fromToRot = Quaternion.FromToRotation(vehicleMesh.transform.up, up_projection);
        //  vehicleMesh.transform.rotation = Quaternion.Lerp(vehicleMesh.transform.rotation,fromToRot, rotationSpeed);
        // vehicleMesh.transform.localEulerAngles = new Vector3(vehicleMesh.transform.localEulerAngles.x,vehicleMesh.transform.localEulerAngles.y, 0);

        // vehicleMesh.transform.eulerAngles = new Vector3(Mathf.LerpAngle(vehicleMesh.transform.eulerAngles.x,angle,rotationSpeed),0,0);
        // vehicleMesh.transform.eulerAngles = new Vector3(Mathf.SmoothDampAngle(vehicleMesh.transform.localEulerAngles.x, angle, ref vel , rotationSpeed),0,0);
        vehicleMesh.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }

    void BalanceVehicle()
    {
        if (groundDetected)
        {
            return;
        }

        if (InputHandler.IsScreenTapped())
        {
            vehicleMesh.transform.Rotate(1.5f, 0, 0);
        }
        else
        {
            vehicleMesh.transform.Rotate(-1.5f, 0, 0);
        }
    }

    void MoveVehicle()
    {
        if (groundDetected)
        {
            if (InputHandler.IsScreenTapped())
            {
                //  transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
                //  rb.velocity = -forwardProjection * moveSpeed * Time.deltaTime;
                rb.velocity = -transform.forward * moveSpeed * Time.deltaTime;
            }


        }
    }



    private void OnCollisionStay(Collision collision)
    {
        groundDetected = true;
        up_projection = forwardProjection = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
        {
           
                up_projection += collision.GetContact(i).normal;
                forwardProjection += Vector3.ProjectOnPlane(transform.forward, collision.GetContact(i).normal);

            
        }

        up_projection.Normalize();
        forwardProjection.Normalize();
      
    }

    private void OnCollisionExit(Collision collision)
    {
        groundDetected = false;
    }

    private void LateUpdate()
    {
         transform.localEulerAngles = new Vector3(transform.localEulerAngles.z, 0,0);
       // ApplyManualVehicleRotation();
    }




}

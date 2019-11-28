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

    public float selfRotationStartDelay = 1;
    public float manualRotationStartDelay = 1;
    bool manualRotationAllowed = false;
    bool selfRotationAllowed = false;

    IEnumerator selfRotationRoutine = null;
    IEnumerator manualRotationRoutine = null;
    delegate IEnumerator MyCoroutine();
    MyCoroutine manualRotationCoroutine, selfRotationCoroutine;

    public GameObject backWheelTrans;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        manualRotationCoroutine = ManualRotationStartDelay;
        selfRotationCoroutine = SelfRotationDelay;
    }

    private void Update()
    {
        raycastPos = transform.position;
        bool groundDetected = false;
        Debug.DrawRay(raycastPos, Vector3.down * raycastDistance, Color.red);
        groundDetected = Physics.Raycast(raycastPos, -transform.up, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(backWheelTrans.transform.position, transform.up, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(raycastPos, Vector3.down, out hit, raycastDistance, groundlayer) ||
                         Physics.Raycast(backWheelTrans.transform.position, Vector3.down, out hit, raycastDistance, groundlayer);

       
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
            StartRoutine(manualRotationRoutine, manualRotationCoroutine);
            StopRoutine(selfRotationRoutine);
            selfRotationAllowed = false;
            if (manualRotationAllowed)
            {
                rb.angularVelocity = Vector3.zero;
                transform.Rotate(manualControlledRotation, 0, 0);
                Debug.Log("manual rotation applied");

            }

        }
        else
        {
            Debug.Log("self rotation applied");
            StartRoutine(selfRotationRoutine, selfRotationCoroutine);
            StopRoutine(manualRotationRoutine);
            manualRotationAllowed = false;
            
            if (selfRotationAllowed)
            {
               
                    transform.Rotate(-manualSelfRotation, 0, 0);

              

            }
        }

       

    }

    Vector3 gravityForce;
    public float newGravityMultiplier;
    void AddExtraGravity()
    {
        gravityForce = Physics.gravity*(newGravityMultiplier - 1);
        rb.AddForce(gravityForce*rb.mass);
    }

    void StartRoutine(IEnumerator routine,MyCoroutine routine_function)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;

        }
        if (routine == null)
        {
            routine = routine_function();
            StartCoroutine(routine);
        }
    }
    void StopRoutine(IEnumerator routine)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }

    IEnumerator SelfRotationDelay( )
    {
        yield return new WaitForSeconds(selfRotationStartDelay);
        selfRotationAllowed = true;
      
    }
    IEnumerator ManualRotationStartDelay( )
    {
        yield return new WaitForSeconds(manualRotationStartDelay);
        manualRotationAllowed = true;
    }
}

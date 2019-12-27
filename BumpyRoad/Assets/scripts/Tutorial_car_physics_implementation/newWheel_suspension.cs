using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class newWheel_suspension : MonoBehaviour
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
    public float wheelTurnSmoothness = 10;


    public float force_x;
    public float force_y;

    public Transform wheelMesh;

    public float wheelRotateSpeed=10;
    public float movingForce =15000;

    public float accelerationTime=2;
    public float deaccelerationTime = 1f;
    private float currentAppliedForce = 0;
    private float acceleraterate = 0;
    private float deacelerateRate = 0;
  


    public Color[] dirtColors;


    [Header("Particles")]
    public GameObject WheelSmokeParticle;

    private int sceneIndex =0;

 

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
        springLength = maxLength;

        acceleraterate = (movingForce / accelerationTime) * Time.fixedDeltaTime;
        deacelerateRate = (movingForce / deaccelerationTime) * Time.fixedDeltaTime;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        WheelSmokeParticle.GetComponent<ParticleSystem>().startColor = dirtColors[sceneIndex];

       




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
        
        CalculateSuspensionAndMovingForce();
       // MoveVehicle();

    }



    private void CalculateSuspensionAndMovingForce()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius) && !GameOverChecker.gameOver)
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;
            suspensionForce = (springForce + damperForce) * transform.up;

            wheelVelocity_localSpace = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            force_y = wheelVelocity_localSpace.x;
            rb.AddForceAtPosition(suspensionForce  + (force_y * -transform.right), hit.point);
            if (Input.GetMouseButtonDown(0))
            {
                OnVehicleMoved(hit.point);
            }
        }
    }

   

    void CalculateWheelPos()
    {
        Vector3 newWheelPos = transform.position;  
        wheelMesh.position = -springLength * transform.up+ transform.position;
    }

    void calculateWheelRotation()
    {
        Vector3 rotation = wheelMesh.rotation.eulerAngles;
        wheelMesh.Rotate(-Mathf.Sign(rb.velocity.z) * rb.velocity.magnitude*wheelRotateSpeed * Time.fixedDeltaTime, 0, 0);
    }


    void OnVehicleMoved(Vector3 groundPos)
    {
        WheelSmokeParticle.transform.position = groundPos;
        WheelSmokeParticle.SetActive(false);
        WheelSmokeParticle.SetActive(true);
    }

  

   
}

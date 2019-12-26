using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class noPhysicsSuspension : MonoBehaviour
{
    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;


    public float restLength;
    public float springTravel;

    public Rigidbody rb;

    private Vector3 suspensionForce;
    private Vector3 wheelVelocity_localSpace;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;

   

    [Header("Wheels")]
    public float wheelRadius;
  
    public float wheelTurnSmoothness = 10;

    public Transform wheelMesh;

    public float wheelRotateSpeed = 10;
 
    public Color[] dirtColors;


    [Header("Particles")]
    public GameObject WheelSmokeParticle;

    private int sceneIndex = 0;



    void Start()
    {
       
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
        springLength = maxLength;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

       // WheelSmokeParticle.GetComponent<ParticleSystem>().startColor = dirtColors[sceneIndex];

    }

    private void Update()
    {
        CalculateWheelPos();
        calculateWheelRotation();
    }


    void FixedUpdate()
    {
        CalculateSuspensionAndMovingForce();
    }



    private void CalculateSuspensionAndMovingForce()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius) && !GameOverChecker.gameOver)
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
         
            if (Input.GetMouseButtonDown(0))
            {
               // OnVehicleMoved(hit.point);
            }


        }
    }



    void CalculateWheelPos()
    {
        //wheelMesh.position = transform.position-  new Vector3(0, springLength, 0);
        Vector3 newWheelPos = transform.position;
        // newWheelPos.y = (transform.position.y - springLength);
        wheelMesh.position = -springLength * transform.up + transform.position;
    }

    void calculateWheelRotation()
    {
        Vector3 rotation = wheelMesh.rotation.eulerAngles;


        wheelMesh.Rotate(-Mathf.Sign(rb.velocity.z) * rb.velocity.magnitude * wheelRotateSpeed * Time.fixedDeltaTime, 0, 0);
    }

 
    void OnVehicleMoved(Vector3 groundPos)
    {
        WheelSmokeParticle.transform.position = groundPos;
        WheelSmokeParticle.SetActive(false);
        WheelSmokeParticle.SetActive(true);
    }




}

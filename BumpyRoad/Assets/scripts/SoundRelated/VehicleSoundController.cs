using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSoundController : MonoBehaviour
{
    private AudioSource vehicleAudioSource;
    private AudioSource defaultAudioSource;
    private  Rigidbody vehicle_rb;
    private bool gameStarted;

    public float maxSpeed = 60f;
    public float pitchRatio = 0.75f;

   

    private void Start()
    {
        defaultAudioSource = GetComponent<AudioSource>();
        InputHandler.inputReceivedListeners += OnInputReceived;
    }

  
    void OnInputReceived()
    {
      
        gameStarted = true;
        InputHandler.inputReceivedListeners -= OnInputReceived;
    }

   

    private void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        ChangeSoundPitchAccordingToVehicleSpeed();


    }

    private void ChangeSoundPitchAccordingToVehicleSpeed()
    {
        GetPlayerReference();

        float currentSpeed = Mathf.Abs(vehicle_rb.velocity.z);
        float currentPitchModifier = (currentSpeed / maxSpeed) * pitchRatio;
        float newPitch = 1 + currentPitchModifier;
        vehicleAudioSource.pitch = newPitch;
    }

    void GetPlayerReference()
    {
        if ((vehicle_rb == null || vehicleAudioSource == null) && !GameOverChecker.gameOver)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            vehicle_rb = player.GetComponent<Rigidbody>();
            vehicleAudioSource = player.GetComponent<AudioSource>();

        }
        if (GameOverChecker.gameOver)
        {
            vehicle_rb = null;
            vehicleAudioSource = null;
        }
    }

}

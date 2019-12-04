using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public Rigidbody vehicle_rb;
    public float maxSpeed = 60f;
    public float pitchRatio = 0.75f;


    private void Update()
    {
        float currentSpeed = Mathf.Abs(vehicle_rb.velocity.z);

        Debug.Log("current speed : " + currentSpeed);
        float currentPitchModifier = (currentSpeed / maxSpeed)*pitchRatio;
        float newPitch = 1 + currentPitchModifier;

        Debug.Log("pitch : " + newPitch);
        audioSource.pitch = newPitch;


    }

}

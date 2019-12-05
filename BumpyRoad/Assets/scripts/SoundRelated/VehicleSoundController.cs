using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSoundController : MonoBehaviour
{
    private AudioSource vehicleAudioSource;
    private AudioSource defaultAudioSource;
    private  Rigidbody vehicle_rb;
    private bool gameStarted;

    string AudioSave = "AudioMuted";
    string VibrationSave = "VibrationEnabled";

    public float maxSpeed = 60f;
    public float pitchRatio = 0.75f;


    public static bool vibration_enabled;
    public static bool sound_enabled;

    public static  VehicleSoundController Instance;

    private void Start()
    {
        Instance = this;
        defaultAudioSource = GetComponent<AudioSource>();
        InputHandler.inputReceivedListeners += OnInputReceived;
    }

  
    void OnInputReceived()
    {
      
        gameStarted = true;
        InputHandler.inputReceivedListeners -= OnInputReceived;
    }


    public void PlaySound(AudioClip clip, AudioSource source = null)
    {
        if (source == null)
        {
            source = defaultAudioSource;
        }


        source.PlayOneShot(clip);
    }

    public void MuteSound()
    {

        if (PlayerPrefs.GetInt(AudioSave, 1) == 1)
        {
            PlayerPrefs.SetInt(AudioSave, 0);


        }
        else
        {
            PlayerPrefs.SetInt(AudioSave, 1);

        }
        SetAudioMuteState();



    }
    public void ToggleVibration()
    {
        if (PlayerPrefs.GetInt(VibrationSave, 1) == 1)
        {
            PlayerPrefs.SetInt(VibrationSave, 0);

        }
        else
        {
            PlayerPrefs.SetInt(VibrationSave, 1);

        }
        SetVibrationState();
       
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
        if (!vehicleAudioSource)
        {
            Debug.Log("vehicle not found");
            return;
        }

        vehicleAudioSource.mute = sound_enabled;
        

        float currentSpeed = Mathf.Abs(vehicle_rb.velocity.z);
        float currentPitchModifier = (currentSpeed / maxSpeed) * pitchRatio;
        float newPitch = 0.5f + currentPitchModifier;
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

    void SetVibrationState()
    {

        if (PlayerPrefs.GetInt(VibrationSave, 1) == 1)
        {
            vibration_enabled = true;
            

        }
        else
        {

            vibration_enabled = false;
           

        }
    }

    void SetAudioMuteState()
    {
        if (PlayerPrefs.GetInt(AudioSave, 0) == 1)
        {
            sound_enabled = false;
        }
        else
        {

            sound_enabled = true;
           
        }
        defaultAudioSource.mute = sound_enabled;
    }

}

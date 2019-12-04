using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundPlayer : MonoBehaviour
{
    public static AudioSource audio_source;
    public static AudioMixer audioMixer;
    

    public static SoundPlayer Instance;

    public AudioClip angryStartClip, lightCrossedClip, lightChangeClip, successfulClip, crashClip, trainHornClip, trainBackgroundClip,fevermodeStartClip,feverModeEndClip;
    public AudioClip[] rageHorn;

    string AudioSave = "AudioMuted";
    string VibrationSave = "VibrationEnabled";

    public Image SoundImage;
    public Sprite soundOn, soundOff;

    public Image VibrationImage;
    public Sprite vibrationOn, vibrationOff;

    [HideInInspector]
    public bool VibrationEnabled;


    public AudioClip musicClip;
    
    

    private void Awake()
    {
        Instance = this;
       
    }
    void Start()
    {
       
        audio_source = GetComponent<AudioSource>();
        SetAudioMuteState();
        SetVibrationState();

        PlaySound(musicClip);
    }

   
    public void PlaySound(AudioClip clip, AudioSource source = null)
    {
        if (source == null)
        {
            source = audio_source;
        }

       
        source.PlayOneShot(clip);
    }

    public void MuteSound()
    {
       
        if (PlayerPrefs.GetInt(AudioSave, 1) ==1)
        {
            PlayerPrefs.SetInt(AudioSave, 0);
            

        }
        else
        {
            PlayerPrefs.SetInt(AudioSave, 1);
          
        }
        SetAudioMuteState();

        
        
    }

    void SetAudioMuteState()
    {
        if (PlayerPrefs.GetInt(AudioSave, 0) == 1)
        {
            audio_source.mute = true;
            SoundImage.sprite = soundOff;
        }
        else
        {
           
            audio_source.mute = false;
            SoundImage.sprite = soundOn;
        }
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

     void SetVibrationState()
    {
       
        if (PlayerPrefs.GetInt(VibrationSave, 1) == 1)
        {
            VibrationEnabled = true;
            VibrationImage.sprite = vibrationOn;
            
        }
        else
        {
                      
            VibrationEnabled = false;
            VibrationImage.sprite = vibrationOff;

        }
    }

    
}

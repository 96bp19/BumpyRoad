using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tut_carController : MonoBehaviour
{
    public Wheel_suspension[] wheels;

    [Header("Car specs")]
    public float wheelbase;
    public float rearTrack;
    public float turnRadius;

    [Header("Inputs")]
    public float steerInput;



    private float accermanAngleLeft;
    private float accermanAngleRight;
  
   
    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");

        if (steerInput > 0)
        {
            accermanAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius + (rearTrack / 2))) * steerInput;
            accermanAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius - (rearTrack / 2))) * steerInput;
            // turn right
        }
        else if(steerInput < 0)
        {
            accermanAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius - (rearTrack / 2))) * steerInput;
            accermanAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius + (rearTrack / 2))) * steerInput;
            // turn left
        }
        else
        {
            accermanAngleLeft = 0;
            accermanAngleRight = 0;
            // no steering
        }

        foreach (var item in wheels)
        {
            if (item.wheelFrontLeft)
            {
                item.steerAngle = accermanAngleLeft;

            }
            if (item.wheelFrontRight)
            {
                item.steerAngle = accermanAngleRight;
            }
        }
    }
}

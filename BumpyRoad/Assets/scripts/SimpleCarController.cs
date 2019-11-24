using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public Transform wheel_front_left, wheel_front_right;
    public Transform wheel_rear_left, wheel_rear_right;

    public WheelCollider wheel_front_left_collider, wheel_front_right_collider;
    public WheelCollider wheel_rear_left_collider, wheel_rear_right_collider;

    public float enginePower =5000f;


    float inputX;

    private void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        
    }

    private void FixedUpdate()
    {
        AccelerateVehicle();
        UpdateWheelPositions();
    }

    void AccelerateVehicle()
    {
        wheel_front_left_collider.motorTorque = enginePower * inputX * Time.fixedDeltaTime;
        wheel_front_right_collider.motorTorque = enginePower * inputX * Time.fixedDeltaTime;
    }

    public void UpdateWheelPosition(WheelCollider collider, Transform wheelMesh)
    {
        Vector3 pos;
        Quaternion rotation;

        collider.GetWorldPose(out pos, out rotation);
        wheelMesh.position = pos;
        wheelMesh.rotation = rotation;
    }
    public void UpdateWheelPositions()
    {
        UpdateWheelPosition(wheel_front_left_collider, wheel_front_left);
        UpdateWheelPosition(wheel_front_right_collider, wheel_front_right);
        UpdateWheelPosition(wheel_rear_left_collider, wheel_rear_left);
        UpdateWheelPosition(wheel_rear_right_collider, wheel_rear_right);
    }
}

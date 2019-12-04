using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("Normal force")]
    public float ForceFieldPower = 100;
    public ForceMode forceMode;
    public Vector3 appliedForce;

    public float delay = 2f;
    Rigidbody steppedBody = null;
    public GameObject LandMineParticle;

    public AudioClip landMineSound;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            steppedBody = other.GetComponent<Rigidbody>();
            LandMineParticle.SetActive(true);
            System.Action a = () => AddForce();
            MyMath.RunFunctionAfter(a, this, delay);  
   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (landMineSound)
            {
                VehicleSoundController.Instance.PlaySound(landMineSound);
            }
            steppedBody = null;
        }
    }

    void AddForce()
    {
        
        if (steppedBody == null)
        {
            return;
        }
        
        steppedBody.AddForce(appliedForce * steppedBody.mass * ForceFieldPower, forceMode);
    }
    


    
}

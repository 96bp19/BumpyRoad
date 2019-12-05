using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("Normal force")]
    public float ForceFieldPower = 100;
    public ForceMode forceMode;
    private Vector3 appliedForce;

    public float delay = 2f;
    Rigidbody steppedBody = null;
    public GameObject LandMineParticle;

    public AudioClip landMineSound;

    private void Start()
    {
        appliedForce = new Vector3(0, 1, 0.5f);
        appliedForce.z = Random.Range(-0.7f, 0.7f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (landMineSound)
            {
                VehicleSoundController.Instance.PlaySound(landMineSound);
            }
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
           
            steppedBody = null;
        }

    }

    void AddForce()
    {
        
        if (steppedBody == null)
        {
            Destroy(gameObject);
            return;
        }
        
        steppedBody.AddForce(appliedForce * steppedBody.mass * ForceFieldPower, forceMode);
         Destroy(gameObject);
    }
    


    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("Normal force")]
    public float ForceFieldPower = 100;
    public ForceMode forceMode;
    public ForceType forcetype;
    public Vector3 appliedForce;

    public float delay = 2f;
    Rigidbody steppedBody = null;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            steppedBody = other.GetComponent<Rigidbody>();
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
            return;
        }
        steppedBody.AddForce(appliedForce * steppedBody.mass * ForceFieldPower, forceMode);
    }
    


    public enum ForceType
    {
        E_ExplosionForce, E_ConstantForce
    }
}

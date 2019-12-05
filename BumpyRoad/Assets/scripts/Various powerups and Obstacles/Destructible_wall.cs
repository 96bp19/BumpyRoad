using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible_wall : MonoBehaviour
{
	public GameObject pieces_mesh;
    public AudioClip brokenClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (brokenClip)
            {
                VehicleSoundController.Instance.PlaySound(brokenClip);
            }
            Instantiate(pieces_mesh,transform.position,Quaternion.Euler(0,90,0)); 
            Destroy(gameObject);
        }
    }

    

}

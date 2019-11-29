using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverChecker : MonoBehaviour
{

    public static bool gameOver;
    public float distanceBelowGroundForDeathTrigger = 20f;

    private void FixedUpdate()
    {
        if (gameOver)
        {
            return;
        }
        if (transform.position.y < -distanceBelowGroundForDeathTrigger)
        {
            gameOver = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("Player died");
            gameOver = true;
        }
    }
}

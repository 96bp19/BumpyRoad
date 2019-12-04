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

            Debug.Log("Game over");
            this.enabled = false;
            return;
        }
        if (transform.position.y < -distanceBelowGroundForDeathTrigger)
        {
            gameOver = true;
        }
    }

  
}

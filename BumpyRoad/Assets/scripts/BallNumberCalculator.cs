using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallNumberCalculator : MonoBehaviour
{
    int totalBalls;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balls"))
        {
            totalBalls++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("balls"))
        {
            totalBalls--;
        }

        if (totalBalls <=0)
        {
            GameOverChecker.gameOver = true;
        }
    }
}

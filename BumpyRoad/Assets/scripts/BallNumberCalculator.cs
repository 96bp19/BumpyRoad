using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallNumberCalculator : MonoBehaviour
{
    int totalBalls;

    public TMP_Text BallRemainingText;

    int currentBallRemaining;

    private void Start()
    {
        BallRemainingText = GameObject.FindGameObjectWithTag("BallRemaining").GetComponent<TMP_Text>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balls"))
        {
           currentBallRemaining++;
            CancelInvoke("CountBalls");
            Invoke("CountBalls",1f);

            BallRemainingText.text = "Balls : " + currentBallRemaining.ToString();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("balls"))
        {
            currentBallRemaining--;
            CancelInvoke("CountBalls");
            Invoke("CountBalls", 1f);
            BallRemainingText.text = "Balls : " + currentBallRemaining.ToString();
        }
    }

   void CountBalls()
   {
        totalBalls = currentBallRemaining;
        

        if (totalBalls <= 0)
        {
            GameOverChecker.gameOver = true;
        }
    }


}

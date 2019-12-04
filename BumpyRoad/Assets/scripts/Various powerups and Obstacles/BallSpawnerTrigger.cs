using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerTrigger : MonoBehaviour
{
    public float objectSpawnChance = 100;
    public int numberofBallsToSpawn =10;
    public float ballSpawnDelay =1f;
    public float ballSpawnRate = 0.2f;
    public Transform ballSpawnPos;
    public GameObject ballPrefab;
    
    bool alreadyActivated;
    int currentSpawnedBall =0;


    public AudioClip ballSpawnSound;
    private void Start()
    {
       bool canActivate =  MyMath.getSuccessRate(objectSpawnChance);
        if (!canActivate)
        {
            this.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alreadyActivated = true;
            InvokeRepeating("SpawningBalls", ballSpawnDelay,ballSpawnRate);
           
        }
    }

    void SpawningBalls()
    {
        if (ballSpawnSound)
        {
            VehicleSoundController.Instance.PlaySound(ballSpawnSound);
        }
        currentSpawnedBall++;
       GameObject obj = Instantiate(ballPrefab, ballSpawnPos);
        obj.transform.position = ballSpawnPos.position;
        if (currentSpawnedBall >= numberofBallsToSpawn)
        {
            CancelInvoke("SpawningBalls");
        }
    }

    
}

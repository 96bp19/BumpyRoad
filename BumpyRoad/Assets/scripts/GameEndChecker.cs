using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndChecker : MonoBehaviour
{
    int totalCount;
    public GameObject celebrationparticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balls"))
        {
            
            totalCount++;
            StartCoroutine(waiter(0.1f, other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (totalCount >0)
            {
                Debug.Log("total balls present : " + totalCount);
                OnlevelComplete();
            }
        }
    }
    IEnumerator waiter(float time, GameObject itemToDisable)
    {
        yield return new WaitForSeconds(time);
        itemToDisable.SetActive(false);
    }

    void OnlevelComplete()
    {
        SpawnCelebrationParticle();
    }

    void SpawnCelebrationParticle()
    {
        celebrationparticle.SetActive(false);
        celebrationparticle.SetActive(true);
    }
}

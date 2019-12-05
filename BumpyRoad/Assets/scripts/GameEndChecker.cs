using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameEndChecker : MonoBehaviour
{
    int totalCount;
    public GameObject celebrationparticle;

    Slider MeterRemainingSlider;
    Transform player;
    float remainingmeterinPercentage;
    float maxMeter;

    public AudioClip celebrationSound;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MeterRemainingSlider = GameObject.FindGameObjectWithTag("MeterRemainingSlider").GetComponent<Slider>();
        maxMeter = player.transform.position.z - transform.position.z;
        

    }

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
        VehicleSoundController.Instance.PlaySound(celebrationSound);
        SpawnCelebrationParticle();
        UIManager.Instance.LevelCompletedUI();

        Destroy(this.GetComponent<BoxCollider>());
    }

    void SpawnCelebrationParticle()
    {
        celebrationparticle.SetActive(false);
        celebrationparticle.SetActive(true);
    }

    private void Update()
    {
        calculateDistance();
    }

    void calculateDistance()
    {
        if (player == null)
        {
            return;
        }
        float currentDistance = player.transform.position.z - transform.position.z;
        remainingmeterinPercentage = 1 -currentDistance / maxMeter;
        remainingmeterinPercentage = Mathf.Clamp(remainingmeterinPercentage, 0f, 1f);
        MeterRemainingSlider.value =  remainingmeterinPercentage;
    }
}

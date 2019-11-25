using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startUI, gameOverUI, settingUI, doubleUI, playingUI, noThanksButton;

    public TextMeshProUGUI levelText, diamondText;

    public Transform stageParent;
    private List <GameObject> stageItem = new List<GameObject>();
    bool isPlaying = false;
    bool isDead = false;
    bool isStarted = false;
    bool levelClear = false;
    public Sprite stageComplete, stagePlaying, stageNext;


    GameObject levelGenerator;
    LevelGenerator levelGeneratorCS;

    private void Awake()
    {
        
        DontDestroyOnLoad(this.gameObject);

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        startUI.SetActive(true);
        playingUI.SetActive(true);
        gameOverUI.SetActive(false);
        settingUI.SetActive(false);
        doubleUI.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        if (isPlaying)
        {
            startUI.SetActive(false);
            playingUI.SetActive(true);
            gameOverUI.SetActive(false);
            isPlaying = false;
        }
        else if (isDead)
        {
            gameOverUI.SetActive(true);
            playingUI.SetActive(false);
            StartCoroutine(ActiveText());
            isDead = false;
        }
        else if(isStarted)
        {
            startUI.SetActive(true);
            doubleUI.SetActive(false);
            playingUI.SetActive(true);
            gameOverUI.SetActive(false);
            isStarted = false;
        }
        else if(isStarted)
        {
            startUI.SetActive(true);
            doubleUI.SetActive(false);
            playingUI.SetActive(true);
            gameOverUI.SetActive(false);
            isStarted = false;
        }
        else if(levelClear)
        {
            startUI.SetActive(false);
            doubleUI.SetActive(false);
            gameOverUI.SetActive(false);
            levelClear = false;
        }
    }

    public void IsPlaying()
    {
        isPlaying = true;
    }

    public void GameOver()
    {
        isDead = true;
        Debug.Log("Fuck Over");
    }

    public void NoThanksButton()
    {
        isStarted = true;
    }

    IEnumerator ActiveText()
    {
        noThanksButton.SetActive(false);
        yield return new WaitForSeconds(5f);
        noThanksButton.SetActive(true);
        yield return true;
    }
    
    public IEnumerator DeactiveStage(int s)
    {
            foreach (Transform g in stageParent.transform.GetComponentsInChildren<Transform>())
            {
                if (stageParent != g)
                {
                    stageItem.Add(g.gameObject);
                    g.gameObject.SetActive(false);
                    Debug.Log("Current Size: " + stageItem.Count);
                }
            }

            Debug.Log("Total Size: " + stageItem.Count);

            for (int i = 0; i < s; i++)
            {
                stageItem[i].gameObject.SetActive(true);
                stageItem[i + 1].gameObject.GetComponent<Image>().sprite = stageNext;
            }
        yield return true;
    }

    public void ChangeStageUI(int s)
    {
        stageItem[s-2].gameObject.GetComponent<Image>().sprite = stageComplete;
        stageItem[s-1].gameObject.GetComponent<Image>().sprite = stagePlaying;
        Debug.Log("FUcking Stage: " + s);
    }

    public void LevelCompletedUI()
    {
        doubleUI.SetActive(true);
    }

    public void UpdateLevelText(int i)
    {
        levelText.text = "LEVEL " + i;
    }

    public void LevelFinished()
    {
        levelGenerator = GameObject.Find("LevelGenerator");
        levelGeneratorCS = levelGenerator.GetComponent<LevelGenerator>();

        levelGeneratorCS.CheckStageUpdate();

        levelClear = true;

    }
}

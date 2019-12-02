using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startUI, gameOverUI, settingUI,  playingUI, levelClearUI, noThanksButton;

    public TextMeshProUGUI levelText;

    bool isPlaying = false;
    bool isDead = false;
    bool isStarted = false;
    bool levelClear = false;


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
        levelClearUI.SetActive(false);
    }

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
        else if (isStarted)
        {
            startUI.SetActive(true);
            levelClearUI.SetActive(false);
            playingUI.SetActive(true);
            gameOverUI.SetActive(false);
            isStarted = false;
        }
        else if (levelClear)
        {
            startUI.SetActive(false);
            levelClearUI.SetActive(false);
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

    public void LevelCompletedUI()
    {
        levelClearUI.SetActive(true);
    }
    
    public void LevelFinished()
    {
        levelGenerator = GameObject.Find("LevelGenerator");
        levelGeneratorCS = levelGenerator.GetComponent<LevelGenerator>();

        levelGeneratorCS.CheckStageUpdate();

        levelClear = true;
    }

    public void SetStageText(int stage)
    {
        levelText.text = "LEVEL " + stage;
    }

    public void NextLevel()
    {

    }
}

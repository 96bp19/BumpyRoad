using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startUI, gameOverUI, settingUI,  playingUI, levelClearUI, noThanksButton;

    public TextMeshProUGUI levelText;

    public bool isPlaying = false; 
    public bool isDead = false;    
    public bool isStarted = false;
    public bool levelClear = false;


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
        Debug.Log("UI manager start called");
        
        isStarted = true;
        // registering for delegate event
        InputHandler.inputReceivedListeners += OnInputReceived;

        startUI.SetActive(true);
        playingUI.SetActive(true);
        gameOverUI.SetActive(false);
        settingUI.SetActive(false);
        levelClearUI.SetActive(false);
    }

    void OnInputReceived()
    {
        Debug.Log("Input received");
        isPlaying = true;
        // unregistering delegate routine so that this function will only called once
        InputHandler.inputReceivedListeners -= OnInputReceived;
    }

    private void Update()
    {


        if (!isDead)
        {
            isDead = GameOverChecker.gameOver;

        }
        
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
           // StartCoroutine(ActiveText());
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

    public void Restart()
    {
        gameOverUI.SetActive(false);
        startUI.SetActive(true);
        levelClearUI.SetActive(false);
        playingUI.SetActive(true);
        isStarted = true;
        isDead = false;
        GameOverChecker.gameOver = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        InputHandler.inputReceivedListeners += OnInputReceived;
    }
}

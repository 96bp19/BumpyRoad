﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject counter;
    public GameObject counterEnableTrigger;
    public GameObject player;


    private Vector3 nextSpawnPoint;
    public int platformSize = 3;
    public Transform spawnPosition;

    private int currentLevel;

    private static  LevelGenerator _Instance;
    public static LevelGenerator Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<LevelGenerator>();
            }
            return _Instance;
        }
    }
    private void Awake()
    {

        if (PlayerPrefs.HasKey("CL")) //CL= Current Level;
        {
            currentLevel = PlayerPrefs.GetInt("CL");
        }
        else
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("CL", 1);
        }

    }

    public void SaveLevel(int level, int sceneIndex)
    {
        PlayerPrefs.SetInt("CL", level);
        PlayerPrefs.SetInt("CS", sceneIndex);
    }

    public int Getlevel()
    {
        return currentLevel;
    }

    public void Start()
    {
        CreateLevel();
        GameAnalyticsManager.GetIdfaID();
       
    }

    public void CheckStageUpdate()
    {
            NextLevel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckStageUpdate();
        }
    }
    public void CreateLevel()
    {
        Debug.Log("Level is:" + currentLevel);

        UIManager.Instance.SetStageText(Getlevel());

        Transform newSpawnPoint = spawnPosition.transform;

        Generate(newSpawnPoint, LevelSize()); //reset size every next level 

    }

    //Generate Platforms in a specific position and size.
    public void Generate(Transform firstPosition, int size)
    {
        Transform newParent = firstPosition;
        int i = 0;

        GameObject tempObject = Instantiate(platforms[0], firstPosition.transform.position, Quaternion.identity) as GameObject;

        //parrent it to its is spawn point
        tempObject.transform.parent = newParent;

        nextSpawnPoint = tempObject.gameObject.transform.GetChild(1).position;
        Vector3 tempPosition = new Vector3(nextSpawnPoint.x, nextSpawnPoint.y, nextSpawnPoint.z);

        //Player needs to get spawn at the first platform;
        GameObject newPlayer = Instantiate(player, tempObject.gameObject.transform.GetChild(3).position, Quaternion.identity) as GameObject;
        //parrent it to its is spawn point
        //newPlayer.transform.parent = newParent;

        //Retarget Camera to player;
        //CameraBehaviour._camera.SetNewTarget(newPlayer);

        for (int j = 0; j < size - 1; j++)
        {
            i = genInt(i, GetPlatformID());
            GameObject clone = Instantiate(platforms[i], tempPosition, Quaternion.identity) as GameObject;
            //parrent it to its is spawn point
            clone.transform.parent = newParent;

            nextSpawnPoint = clone.gameObject.transform.GetChild(1).position;
            tempPosition = new Vector3(nextSpawnPoint.x, nextSpawnPoint.y, nextSpawnPoint.z);
        }

        // Instantiate The Counter at the end of the level
        GameObject counterClone = Instantiate(counter, tempPosition, Quaternion.identity) as GameObject;

        //parrent it to its is spawn point
        counterClone.transform.parent = newParent;

        //ScoreManager.Instance.counterCS = counterClone.GetComponent<ObjCounter>();

        // One Trigger is required at the end of the platform to enable counting the balls in a score manager
        Vector3 counterTriggerPosition = tempPosition;
        counterTriggerPosition.x = counterTriggerPosition.x - 0.8f;

        counterTriggerPosition.z = counterTriggerPosition.z + 1.5f;
        GameObject counterTrigger = Instantiate(counterEnableTrigger, counterTriggerPosition, Quaternion.identity) as GameObject;

        //parrent it to its is spawn point
        counterTrigger.transform.parent = newParent;

        // Measuring Distance for UI Update
        //DistanceTool.Instance.InitializeDistance(tempObject.transform, counterClone.transform);

    }

    public static int genInt(int p, int Length)
    {
        int i = Random.Range(0, Length);
        if (i == p)
        {
            i=genInt(p, Length);
        }
        return i;
    }

    public void NextLevel()
    {
        int s;
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {

            s = SceneManager.GetActiveScene().buildIndex + 1;
        }
        else
        {
            s = 0;
        }
        currentLevel++;
        SaveLevel(currentLevel, s);

        SceneManager.LoadSceneAsync(s);
        UIManager.Instance.GameStarted();
        Debug.Log("Current Level: " + currentLevel);
    }

    private int LevelSize()
    {
        int i = 6;
        if (currentLevel >= 1 && currentLevel < 5)
        {
            i = Random.Range(6, 10);
        }
        else if (currentLevel >= 5 && currentLevel < 10)
        {
            i = Random.Range(7, 12);
        }
        else if (currentLevel >= 10 && currentLevel < 20)
        {
            i = Random.Range(9, 15);
        }
        else if (currentLevel >= 20 && currentLevel < 40)
        {
            i = Random.Range(10, 18);
        }
        else if (currentLevel >= 40)
        {
            i = Random.Range(12, 25);
        }

        return i;
    }

    private int GetPlatformID()
    {
        int i = 6;
        if (currentLevel >= 1 && currentLevel < 5)
        {
            i = 6;
        }
        else if (currentLevel >= 5 && currentLevel < 10)
        {
            i = 7;
        }
        else if (currentLevel >= 10 && currentLevel < 20)
        {
            i = 9;
        }
        else if (currentLevel >= 20 && currentLevel < 30)
        {
            i = 10;
        }
        else if (currentLevel >= 30 && currentLevel < 40)
        {
            i = 12;
        }
        else if (currentLevel >= 40)
        {
            i = platforms.Length;
        }


        return i;
    }
}

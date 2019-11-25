using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
   // public ObjCounter counterCS;
    private int count = 0;
    public int stages = 1;
    public int currentStage = 1;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void CheckCount()
    {
        //count = counterCS.GetCount();

        //Debug.Log("Fucking Count: " + count);
       // Debug.Log("CS: " + currentStage + " Total Stages: " + stages);

        if (currentStage == stages)
        {
            if (count < RequiredBalls())
            {
                UIManager.Instance.GameOver();
            }
            else
            {
                UIManager.Instance.LevelCompletedUI();
            }
        }
        else
        {
            if (count < RequiredBalls())
            {
                UIManager.Instance.GameOver();
            }
            else
            {
                UIManager.Instance.LevelFinished();
            }
        }        

    }

    private int ItemCount()
    {
        return GameObject.FindGameObjectsWithTag("ball").Length;
    }

    private int RequiredBalls()
    {
        int level = CheckLevel();
        int totalItem = ItemCount();
        int i = 0;
        if (level >= 1 && level < 10)
        {
            i = (totalItem * 80) / 100;
        }
        else if (level >= 10 && level < 20)
        {
            i = (totalItem * 85) / 100;
        }
        else if (level >= 20 && level < 30)
        {
            i = (totalItem * 88) / 100;
        }
        else if (level >= 30 && level < 40)
        {
            i = (totalItem * 90) / 100;
        }
        else if (level >= 40 && level < 50)
        {
            i = (totalItem * 92) / 100;
        }
        else if (level >= 50 && level < 60)
        {
            i = (totalItem * 94) / 100;
        }
        else if (level >= 60 && level < 70)
        {
            i = (totalItem * 95) / 100;
        }
        else if (level >= 70 && level < 80)
        {
            i = (totalItem * 96) / 100;
        }
        else if (level >= 80 && level < 90)
        {
            i = (totalItem * 96) / 100;
        }
        else if (level >= 90 && level < 100)
        {
            i = (totalItem * 98) / 100;
        }
        else
        {
            i = (totalItem * 99) / 100;
        }
        return i;
    }

    int CheckLevel()
    {
        int cr;
        if (PlayerPrefs.HasKey("CL")) //CL= Current Level;
        {
            cr = PlayerPrefs.GetInt("CL");
        }
        else
        {
            cr = 1;
            PlayerPrefs.SetInt("CL", 1);
        }

        return cr;

    }

    public void SetTotalStages(int s)
    {
        stages = s;
    }

    public void SetCurrentStage(int s)
    {
        currentStage = s;
    }
}

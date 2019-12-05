using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameAnalyticsSDK.GameAnalytics.Initialize();
    }

    private static GameAnalyticsManager _Instance;
    public static GameAnalyticsManager apple;
    public static  GameAnalyticsManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<GameAnalyticsManager>();
            }
            return _Instance;
        }
    }
  
    public void OnLevelStarted()
    {
        Debug.Log("Level Started ");
    }

    public void OnLevelCompleted()
    {
        Debug.Log("Level Complete");
    }

    public  void OnLevelFailed()
    {
        Debug.Log("Level Failed");
    }
}

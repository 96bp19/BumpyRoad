using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using com.adjust.sdk;

public class GameAnalyticsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GameAnalytics.Initialize();
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
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Application.version, LevelGenerator.Instance.Getlevel().ToString("00000"));

        AdjustEvent adjustEvent = new AdjustEvent("8yu94w5l8oow");
        Adjust.trackEvent(adjustEvent);
        Debug.Log("Level Started ");
    }

    public void OnLevelCompleted()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Application.version, LevelGenerator.Instance.Getlevel().ToString("00000"));
        Debug.Log("Level Complete");
    }

    public  void OnLevelFailed()
    {
        Debug.Log("Level Failed");
    }

    public static void GetIdfaID()
    {
        Debug.Log("Idfa id called ");
        string idfa = Adjust.getIdfa();
    }
}

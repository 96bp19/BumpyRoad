using com.adjust.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustEventtracking : MonoBehaviour
{
    
    public void RunAdjustEvent()
    {
        Debug.Log("event should be called");
        AdjustEvent adjustEvent = new AdjustEvent("8yu94w5l8oow");
        Adjust.trackEvent(adjustEvent);
    }
}

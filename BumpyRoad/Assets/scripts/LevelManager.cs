using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int currentSceneIndex;
    private void Start()
    {
        if (PlayerPrefs.HasKey("CS")) //CL= Current Level;
        {
            currentSceneIndex = PlayerPrefs.GetInt("CS");
        }
        else
        {
            currentSceneIndex = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex != currentSceneIndex)
        {
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}

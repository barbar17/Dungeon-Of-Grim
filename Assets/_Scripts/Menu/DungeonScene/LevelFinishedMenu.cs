using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelFinishedMenu : MonoBehaviour
{
    public static bool isLevelFinished = false;

    void OnEnable()
    {
        Time.timeScale = 0;
        isLevelFinished = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isLevelFinished = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        isLevelFinished = false;
        SceneController.instance.LoadMainMenuScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
}

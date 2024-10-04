using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedGameMenu : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("savedLevel", 0);
    }

    public void ClickSFX()
    {
        AudioManager.instance.ClickSFX();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadMainMenuScene();
    }

    public void QuitGame()
    {
        SceneController.instance.QuitGame();
    }
}

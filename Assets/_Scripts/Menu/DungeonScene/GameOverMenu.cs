using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public void ClickSFX()
    {
        AudioManager.instance.ClickSFX();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadMainMenuScene();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadDungeonScene();
    }

    public void QuitGame()
    {
        SceneController.instance.QuitGame();
    }
}

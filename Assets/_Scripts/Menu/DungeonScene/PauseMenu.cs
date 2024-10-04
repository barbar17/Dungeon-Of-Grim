using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;

    void Update()
    {
        if (LevelFinishedMenu.isLevelFinished == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ClickSFX()
    {
        AudioManager.instance.ClickSFX();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadMainMenuScene();
    }

    public void FinishedGameScene()
    {
        SceneController.instance.LoadFinishedGameScene();
    }

    public void GameOverScene()
    {
        SceneController.instance.LoadGameOverScene();
    }

    public void QuitGame()
    {
        SceneController.instance.QuitGame();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneController.instance.LoadDungeonScene();
    }

    public void Benchmark()
    {
        SceneController.instance.LoadBenchmarkScene();
    }

    public void QuitGame()
    {
        SceneController.instance.QuitGame();
    }

    public void ClickSFX()
    {
        AudioManager.instance.ClickSFX();
    }
}

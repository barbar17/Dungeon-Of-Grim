using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField]
    private Animator sceneTransition;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadDungeonScene()
    {
        StartCoroutine(LoadScene("DungeonScene", AudioManager.instance.dungeonMusic));
    }

    public void LoadMainMenuScene()
    {
        StartCoroutine(LoadScene("MainMenuScene", AudioManager.instance.mainMenuMusic));
    }

    public void LoadFinishedGameScene()
    {
        StartCoroutine(LoadScene("FinishedGameScene", AudioManager.instance.finishedGameMusic));
    }

    public void LoadGameOverScene()
    {
        StartCoroutine(LoadScene("GameOverScene", AudioManager.instance.gameOverMusic));
    }

    public void LoadBenchmarkScene()
    {
        SceneManager.LoadScene("BenchmarkScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadScene(string sceneName, AudioClip audioClip)
    {
        sceneTransition.SetTrigger("EndScene");

        yield return new WaitForSeconds(1);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => asyncOperation.isDone);
        AudioManager.instance.PlayMusic(audioClip);

        sceneTransition.SetTrigger("StartScene");
    }
}

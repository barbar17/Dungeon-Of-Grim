using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DungeonSceneTimer : MonoBehaviour
{
    public TextMeshProUGUI timerDisplay;

    private bool timerStart;

    private float timer;

    void Awake()
    {
        ResetTimer();
        timerStart = false;
    }

    void Update()
    {
        if (!timerStart)
        {
            return;
        }

        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliSeconds = Mathf.FloorToInt(timer * 1000) % 1000;

        timerDisplay.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

    void ResetTimer()
    {
        timer = 0f;
    }

    public void StartTimer()
    {
        ResetTimer();
        timerStart = true;
    }

    public void StopTimer()
    {
        timerStart = false;
    }
}

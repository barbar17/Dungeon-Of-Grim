using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DungeonActivation : MonoBehaviour
{
    public TextMeshProUGUI totalActivation, roomActivation, itemActivation, agentActivation, dungeonType, regenerateCount;

    public BenchmarkDungeonGenerator dungeonGenerator;

    public UnityEvent OnFinishedBenchmark;

    private bool isWatchTotalTime;

    private float timer, timerInMs;

    void Awake()
    {
        ResetTimer();
    }

    void Update()
    {
        if (isWatchTotalTime)
        {
            timer += Time.deltaTime;
            timerInMs = timer * 1000;
            totalActivation.text = "Total Activation Time - " + timerInMs + " ms";
        }
    }

    void ResetTimer()
    {
        timer = 0f;
    }

    public void StopTotalTime()
    {
        isWatchTotalTime = false;
    }

    public void StartTotalTime()
    {
        ResetTimer();
        isWatchTotalTime = true;
    }

    // public void InitiateLevel()
    // {
    //     currentLevel = DungeonGenerator.dungeonLevel;
    //     // levelDisplay = currentLevel + 1;
    //     // level.text = "Level - " + levelDisplay;
    // }

    public void InitiateRegenerateCount()
    {
        regenerateCount.text = "Regenerate Count - " + dungeonGenerator.regenerate;
    }

    public void InitiateDungeonType()
    {
        if (dungeonGenerator.dungeonParameterIndex == 0)
        {
            dungeonType.text = "Dungeon Type - Small";
        }
        else if (dungeonGenerator.dungeonParameterIndex == 1)
        {
            dungeonType.text = "Dungeon Type - Medium";
        }
        else if (dungeonGenerator.dungeonParameterIndex == 2)
        {
            dungeonType.text = "Dungeon Type - Large";
        }
    }
}
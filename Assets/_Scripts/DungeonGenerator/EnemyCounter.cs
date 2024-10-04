using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI enemiesCountDisplay;
    public UnityEvent OnAllEnemiesDied, OnFinishedGame;
    public int enemyCount;
    public int currentMinionCount;
    void Awake()
    {
        enemyCount = 0;
        currentMinionCount = 0;
    }

    public void CountEnemy()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesCountDisplay.text = "Enemies - " + enemyCount;
    }

    public void SubstractEnemyCount()
    {
        enemyCount -= 1;
        enemiesCountDisplay.text = "Enemies - " + enemyCount;

        if (enemyCount <= 0 && DungeonGenerator.dungeonLevel != AgentPlacer.bossLevel)
        {
            OnAllEnemiesDied?.Invoke();
        }
        else if (enemyCount <= 0 && DungeonGenerator.dungeonLevel == AgentPlacer.bossLevel)
        {
            OnFinishedGame?.Invoke();
        }
    }

    public void SubstractMinionCount()
    {
        currentMinionCount -= 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerformSummonMinion : MonoBehaviour
{
    public GameObject bossMinionPrefab;
    public Transform summonPosition;
    [SerializeField]
    private int summonCount = 4;
    [SerializeField]
    private float summonDelay = 1f, newBatchSummonDelay = 1.2f;

    private bool canSummon;

    private EnemyCounter enemyCounter;

    void Awake()
    {
        enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
        enemyCounter.currentMinionCount = 0;
        canSummon = true;
    }

    public void StopSummoning()
    {
        canSummon = false;
    }

    public void SummonMinionsBatch()
    {
        if (enemyCounter.currentMinionCount > 0)
        {
            return;
        }
        if (canSummon)
        {
            enemyCounter.currentMinionCount = summonCount;
            StartCoroutine(SummonMinion(summonCount, summonDelay));
        }
    }

    private IEnumerator SummonMinion(int summonCount, float summonDelay)
    {
        yield return new WaitForSeconds(newBatchSummonDelay);
        for (int i = 0; i < summonCount; i++)
        {
            Instantiate(bossMinionPrefab, summonPosition.position, Quaternion.identity);
            enemyCounter.enemyCount++;
            yield return new WaitForSeconds(summonDelay);
        }
    }
}

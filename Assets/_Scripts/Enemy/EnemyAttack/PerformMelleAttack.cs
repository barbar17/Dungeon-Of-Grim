using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformMelleAttack : MonoBehaviour
{
    private EnemyWeaponParent enemyWeaponParent;
    private EnemyAudioManager audioManager;
    private bool canAttack = true;

    public void StopAttacking()
    {
        canAttack = false;
    }
    private void Awake()
    {
        enemyWeaponParent = GetComponentInChildren<EnemyWeaponParent>();
        audioManager = GetComponent<EnemyAudioManager>();
    }

    public void PerformAttack()
    {
        if (!canAttack)
        {
            return;
        }
        audioManager.AttackSFX();
        enemyWeaponParent.AttackMethod();
    }
}

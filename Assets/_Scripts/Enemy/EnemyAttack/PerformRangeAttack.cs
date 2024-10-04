using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformRangeAttack : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectilePos;
    private EnemyAudioManager audioManager;
    private bool canAttack = true;

    void Awake()
    {
        audioManager = GetComponent<EnemyAudioManager>();
    }

    public void StopAttacking()
    {
        canAttack = false;
    }

    public void ShootProjectile()
    {
        if (!canAttack)
        {
            return;
        }
        audioManager.AttackSFX();
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAttackPerformed, OnPowerAttackPerformed;
    public Transform effectPos;
    public GameObject slashEffect, powerSlashEffect;
    private WeapontParent weapontParent;
    private EnemyWeaponParent enemyWeaponParent;

    void Awake()
    {
        weapontParent = GetComponentInParent<WeapontParent>();
        if (weapontParent == null)
        {
            enemyWeaponParent = GetComponentInParent<EnemyWeaponParent>();
        }
    }

    public void TriggerEvent()
    {
        if (weapontParent == null)
        {
            enemyWeaponParent.ResetIsAttacking();
            return;
        }
        weapontParent.ResetIsAttacking();
    }

    public void TriggerAttack()
    {
        if (weapontParent == null)
        {
            enemyWeaponParent.DetectColliders();
            return;
        }
        weapontParent.DetectColliders();
        OnAttackPerformed?.Invoke();
    }

    public void TriggerPowerAttack()
    {
        weapontParent.DetectColliders();
        OnPowerAttackPerformed?.Invoke();
    }

    public void SlashEffect()
    {
        Vector3 direction = effectPos.transform.position - transform.position;
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        Instantiate(slashEffect, effectPos.position, Quaternion.Euler(0, 0, rotation + 180));
    }

    public void PowerSlashEffect()
    {
        Vector3 direction = effectPos.transform.position - transform.position;
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        Instantiate(powerSlashEffect, effectPos.position, Quaternion.Euler(0, 0, rotation + 180));
    }
}

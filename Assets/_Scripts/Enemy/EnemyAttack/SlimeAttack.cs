using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    [SerializeField]
    private int playerLayer = 6;
    [SerializeField]
    private int hitDamage = 1;
    [SerializeField]
    private float attackDelay = 0.5f;
    private bool attackBlocked;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (attackBlocked)
        {
            return;
        }

        PlayerHealth health;
        if (collision.gameObject.layer == playerLayer)
        {
            if (health = collision.gameObject.GetComponent<PlayerHealth>())
            {
                health.GetHit(hitDamage, transform.gameObject);
            }
        }
        StartCoroutine(DelayAttack(attackDelay));

        attackBlocked = true;
    }

    private IEnumerator DelayAttack(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
    }
}

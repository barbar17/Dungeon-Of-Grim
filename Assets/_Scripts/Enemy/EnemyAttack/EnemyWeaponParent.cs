using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemyWeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip weaponSwing;
    public float weaponDelay = 0.2f;
    private bool attackBlocked;
    public bool isAttacking { get; private set; }
    public Transform circleOrigin;
    public float radius = 0.7f;
    public Vector2 PointerInput { get; set; }
    [SerializeField]
    private int weaponDamage = 1;

    public void ResetIsAttacking()
    {
        isAttacking = false;
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }

        Vector2 direction = (PointerInput - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
    }

    public void AttackMethod()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        StartCoroutine(DelayAttack(weaponDelay));

        isAttacking = true;
        attackBlocked = true;
    }

    private IEnumerator DelayAttack(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            PlayerHealth health;
            if (health = collider.GetComponent<PlayerHealth>())
            {
                health.GetHit(weaponDamage, transform.parent.gameObject);
            }
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}

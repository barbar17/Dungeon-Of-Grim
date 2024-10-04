using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyProjectileHealth : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;
    private Rigidbody2D rb2d;
    public UnityEvent OnDeath;

    [SerializeField]
    private bool isDead = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            isDead = true;
            rb2d.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}

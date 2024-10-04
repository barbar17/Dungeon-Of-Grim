using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;
    public UnityEvent<GameObject> OnHitWithReference, OnHitWithCheckSenderReference;
    public UnityEvent OnDeath;
    private Rigidbody2D rb2d;
    private EnemyCounter enemyCounter;
    private EnemyAudioManager audioManager;

    [SerializeField]
    private bool isDead = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<EnemyAudioManager>();
        enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
        {
            return;
        }
        if (sender.layer == gameObject.layer)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth > 0)
        {

            OnHitWithReference?.Invoke(sender);
            if (sender.tag != "Projectile")
            {
                OnHitWithCheckSenderReference?.Invoke(sender);
            }
        }
        else
        {
            audioManager.StopWalkSFX();
            OnDeath?.Invoke();
            isDead = true;
            rb2d.velocity = Vector2.zero;
            Destroy(gameObject, 1f);
        }
    }

    public void OnDeathEnemyCounter()
    {
        enemyCounter.SubstractEnemyCount();
    }

    public void OnDeathMinionCounter()
    {
        enemyCounter.SubstractMinionCount();
    }
}

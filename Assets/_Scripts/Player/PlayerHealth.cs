using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;
    public UnityEvent<GameObject> OnHitWithReference;
    public UnityEvent OnDeath;
    private Rigidbody2D rb2d;
    private HealthBar healthBar;
    private PauseMenu gameOver;

    [SerializeField]
    private bool isDead = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        gameOver = GameObject.Find("LevelUI").GetComponent<PauseMenu>();
        healthBar.SetMaxHealth(maxHealth);
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
        healthBar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            AudioManager.instance.GetHitSFX();
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            AudioManager.instance.DeathSFX();
            OnDeath?.Invoke();
            isDead = true;
            rb2d.velocity = Vector2.zero;
            Destroy(gameObject, 1f);
        }
    }

    public void LoadGameOverScene()
    {
        gameOver.GameOverScene();
    }
}

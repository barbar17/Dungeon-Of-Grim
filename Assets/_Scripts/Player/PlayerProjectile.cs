using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private WeapontParent weapontParent;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;
    public float force;
    private float timer;
    public int enemyLayer = 7, obstacleLayer = 8;
    [SerializeField]
    private int damage = 10;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        weapontParent = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeapontParent>();

        Vector2 direction = (weapontParent.PointerPosition - (Vector2)transform.position).normalized;
        rb2d.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == obstacleLayer)
        {
            capsuleCollider.isTrigger = false;
            rb2d.velocity = new Vector2(0, 0);
            Destroy(gameObject, 0.6f);
            return;
        }
        if (collider2D.gameObject.layer == enemyLayer)
        {
            Destroy(gameObject);
            Health health;
            if (health = collider2D.GetComponent<Health>())
            {
                health.GetHit(damage, transform.gameObject);
            }
            return;
        }
    }
}

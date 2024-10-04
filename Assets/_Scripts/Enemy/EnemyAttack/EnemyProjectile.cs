using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb2D;
    public float force;
    private float timer;
    public int playerLayer = 6, obstacleLayer = 8;

    [SerializeField]
    private int projectileDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb2D.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 180);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 4)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == obstacleLayer)
        {
            Destroy(gameObject);
            return;
        }
        if (collider2D.gameObject.layer == playerLayer)
        {
            Destroy(gameObject);
            PlayerHealth health;
            if (health = collider2D.GetComponent<PlayerHealth>())
            {
                health.GetHit(projectileDamage, transform.gameObject);
            }
            return;
        }
    }

}

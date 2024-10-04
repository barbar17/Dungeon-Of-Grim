using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapontParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip weaponPerformAttack;

    [SerializeField]
    private float attackDelay, powerAttackDelay;
    private bool attackBlocked;
    public bool isAttacking { get; private set; }
    private Transform circleOrigin, projectilePosition;
    public float radius;
    public GameObject arrowPrefab, powerArrowPrefab, weaponPrefab;

    [SerializeField]
    private int weaponDamage, damageMultiplier = 2;
    public string weaponName;
    public bool isWeaponMelee;
    private int currentWeaponDamage;
    private WeaponSO weaponData;
    private AudioClip[] attackSFX;
    private AudioClip[] powerAttackSFX;

    void Start()
    {
        //inisiasi data untuk weapon yang dipakai
        //data diambil dari WeaponManager yang berisi SO dari weapon
        weaponData = WeaponManager.instance.usedWeapon;
        weaponName = weaponData.weaponName;
        isWeaponMelee = weaponData.isWeaponMelee;
        weaponDamage = weaponData.weaponDamage;
        radius = weaponData.attackRadius;
        attackDelay = weaponData.attackDelay;
        powerAttackDelay = weaponData.powerAttackDelay;
        weaponPrefab = weaponData.weaponPrefab;
        attackSFX = weaponData.attackSFX;
        powerAttackSFX = weaponData.PowerAttackSFX;

        //instatiate weapon yang telah diinisiasi
        Instantiate(weaponPrefab, gameObject.transform);

        animator = GetComponentInChildren<Animator>();

        //menentukan jenis weapon yang dipakai untuk membuat GameObject yang membantu saat player melakukan serangan
        if (isWeaponMelee)
        {
            circleOrigin = GameObject.FindGameObjectWithTag("Weapon").transform.Find("CircleOrigin").GetComponent<Transform>();
        }
        else
        {
            projectilePosition = GameObject.FindGameObjectWithTag("Weapon").transform.Find("ProjectilePosition").GetComponent<Transform>();
        }
    }

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

        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
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
        {
            return;
        }

        currentWeaponDamage = weaponDamage;
        int audioIndex = Random.Range(0, attackSFX.Length - 1);
        AudioManager.instance.PlayNormalizeSFX(attackSFX[audioIndex]);
        animator.SetTrigger("Attack");

        //jika weapon bertipe ranged maka akan melakukan instantiate projectile dari senjata setiap kali menyerang
        if (!isWeaponMelee)
        {
            Instantiate(arrowPrefab, projectilePosition.position, Quaternion.identity);

        }
        StartCoroutine(DelayAttack(attackDelay));
        isAttacking = true;
        attackBlocked = true;
    }

    public void PowerAttackMethod()
    {
        if (attackBlocked)
        {
            return;
        }

        currentWeaponDamage = weaponDamage * damageMultiplier;
        int audioIndex = Random.Range(0, powerAttackSFX.Length - 1);
        AudioManager.instance.PlayNormalizeSFX(powerAttackSFX[audioIndex]);
        animator.SetTrigger("PowerAttack");

        //jika weapon bertipe ranged maka akan melakukan instantiate projectile dari senjata setiap kali menyerang
        if (!isWeaponMelee)
        {
            Instantiate(powerArrowPrefab, projectilePosition.position, Quaternion.identity);
        }
        StartCoroutine(DelayAttack(powerAttackDelay));

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
            Health health;
            EnemyProjectileHealth enemyProjectileHealth;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(currentWeaponDamage, transform.parent.gameObject);
            }
            else if (enemyProjectileHealth = collider.GetComponent<EnemyProjectileHealth>())
            {
                enemyProjectileHealth.GetHit(currentWeaponDamage, transform.parent.gameObject);
            }
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehavior> steeringBehaviors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 0.8f;

    [SerializeField]
    private float attackDistance = 0.5f;

    //Inputs sent from the enemy ai to the enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    // public AudioSource audioSource;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;
    bool following = false;

    private void Start()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        Detect(aiData);
        DetectObstacle(aiData);

        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringBehavior behavior in steeringBehaviors)
        {
            (danger, interest) = behavior.GetSteering(danger, interest, aiData);
        }
    }

    private void Update()
    {
        //pergerakaan ai berdasarkan ketersediaan target
        if (aiData.currentTarget != null)
        {
            //melihat target
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //logika akuisisi target
            aiData.currentTarget = aiData.targets[0];
        }
        //menggerakkan agent
        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //stopping logic
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                //logika menyerang
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //logika mengejar
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviors, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }

    //algoritma deteksi player
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField]
    private bool showGizmo = false;

    private List<Transform> colliders;

    public void Detect(AIData aiData)
    {
        //deteksi apakah pemain berjarak dekat dengan agent
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null)
        {
            //deteksi apakah agent melihat pemain
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstaclesLayerMask);

            //pastikan Collider2D yang dilihat berada pada Player layer
            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                colliders = new List<Transform>() { playerCollider.transform };
            }
            else
            {
                colliders = null;
            }
        }
        else
        {
            //agent tidak melihat player
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmo == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }

    //algoritma deteksi obstacle (prop dan wall)
    [SerializeField]
    private float detectionRadius = 2;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool showGizmosObstacles = true;
    Collider2D[] collidersObstacle;

    public void DetectObstacle(AIData aiData)
    {
        collidersObstacle = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.obstacles = collidersObstacle;
    }

    private void OnDrawGizmos()
    {
        if (showGizmosObstacles == false)
            return;
        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in collidersObstacle)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}

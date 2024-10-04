using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    private AgentMover agentMover;
    private EnemyAudioManager audioManager;
    private Vector2 pointerInput, movementInput;
    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyWeaponParent enemyWeaponParent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        agentMover = GetComponent<AgentMover>();
        enemyWeaponParent = GetComponentInChildren<EnemyWeaponParent>();
        audioManager = GetComponent<EnemyAudioManager>();
    }

    private void Update()
    {
        agentMover.movementInput = movementInput;
        if (enemyWeaponParent != null)
        {
            enemyWeaponParent.PointerInput = pointerInput;
        }
        AnimateCharacter();
    }

    private void AnimateCharacter()
    {
        if (movementInput.x != 0 || movementInput.y != 0)
        {

            animator.SetBool("IsMove", true);
            if (!audioManager.enemyWalkSource.isPlaying)
            {
                audioManager.PlayWalkSFX();
            }
        }
        else
        {
            animator.SetBool("IsMove", false);
            audioManager.StopWalkSFX();
        }

        //baca enemy direction
        Vector2 direction = (pointerInput - (Vector2)transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }
}

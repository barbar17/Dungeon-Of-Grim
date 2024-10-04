using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float dashSpeed = 10f, dashDuration = 1f, dashDelay = 1f;
    private bool isDashing, canDash = true;
    public ParticleSystem dust;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Vector2 movementInput, smoothedMovementInput, movementInputSmoothVelocity, pointerInput;
    private WeapontParent weapontParent;

    [SerializeField]
    private InputActionReference pointerPosition, attack, powerAttack, dash;

    private void OnEnable()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        attack.action.performed += PerformAttack;
        powerAttack.action.performed += PerformPowerAttack;
        dash.action.performed += PerformDash;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
        powerAttack.action.performed -= PerformPowerAttack;
        dash.action.performed -= PerformDash;
    }

    private void PerformDash(InputAction.CallbackContext context)
    {
        if (!canDash)
        {
            return;
        }
        StartCoroutine(Dash());
    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weapontParent.AttackMethod();
    }

    private void PerformPowerAttack(InputAction.CallbackContext context)
    {
        weapontParent.PowerAttackMethod();
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weapontParent = GetComponentInChildren<WeapontParent>();

        canDash = true;
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, ref movementInputSmoothVelocity, 0.1f);
        rigidBody.velocity = smoothedMovementInput * speed;
    }

    private void OnMove(InputValue inputValue)
    {
        if (isDashing)
        {
            return;
        }
        if (Time.timeScale == 0)
        {
            return;
        }

        movementInput = inputValue.Get<Vector2>();
        if (movementInput.x != 0 || movementInput.y != 0)
        {

            animator.SetBool("IsMove", true);
            CreateDustParticle();
            if (!AudioManager.instance.playerWalkSource.isPlaying)
            {
                AudioManager.instance.PlayWalkSFX();
            }
        }
        else
        {
            animator.SetBool("IsMove", false);
            StopDustParticle();
            AudioManager.instance.StopWalkSFX();
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (isDashing)
        {
            return;
        }

        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;

        pointerInput = Camera.main.ScreenToWorldPoint(mousePos);

        weapontParent.PointerPosition = pointerInput;

        //baca mouse direction
        Vector2 direction = (pointerInput - (Vector2)transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        rigidBody.velocity = new Vector2(movementInput.x * dashSpeed, movementInput.y * dashSpeed);
        AudioManager.instance.DashSFX();
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashDelay);
        canDash = true;
    }

    private void CreateDustParticle()
    {
        dust.Play();
    }

    private void StopDustParticle()
    {
        dust.Stop();
    }
}

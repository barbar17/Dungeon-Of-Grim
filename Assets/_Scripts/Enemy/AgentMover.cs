using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private float maxSpeed = 2;

    public Vector2 movementInput { get; set; }
    private Vector2 smoothedMovementinput, movementInputSmoothVelocity, pointerInput;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        smoothedMovementinput = Vector2.SmoothDamp(smoothedMovementinput, movementInput, ref movementInputSmoothVelocity, 0.1f);
        rb2d.velocity = smoothedMovementinput * maxSpeed;
    }
}

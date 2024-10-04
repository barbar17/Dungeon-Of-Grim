using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField]
    private float speed, range;
    private Vector2 smoothMovement, smoothMovementVelocity;
    private RectTransform rectTransform;
    private Vector2 direction = new Vector2(0, 1);
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rectTransform.anchoredPosition.y > 70)
        {
            direction = new Vector2(0, -1);
        }
        else if (rectTransform.anchoredPosition.y < 10)
        {
            direction = new Vector2(0, 1);
        }

        smoothMovement = Vector2.SmoothDamp(smoothMovement, direction, ref smoothMovementVelocity, speed);
        rectTransform.anchoredPosition = smoothMovement * range;
    }
}

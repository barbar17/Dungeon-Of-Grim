using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private Transform player;

    public void InitiatePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if (!player)
        {
            return;
        }

        Vector3 newPosition = new Vector3(player.position.x, player.position.y, -1);
        transform.position = newPosition;
    }
}

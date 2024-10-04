using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchmarkCamera : MonoBehaviour
{
    public static int xPosCamera, yPosCamera;
    public float moveSpeed = 5f; // Speed of camera movement
    public float zoomSpeed = 10f; // Speed of camera zooming
    public float minOrthoSize = 10f; // Minimum orthographic size
    public float maxOrthoSize = 50f; // Maximum orthographic size

    void Update()
    {
        // Camera movement
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += moveDirection * moveSpeed;

        // Camera zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newSize = Camera.main.orthographicSize - scrollInput * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
    }

    public void MoveCamera()
    {
        xPosCamera = BenchmarkDungeonGenerator.dungeonCenter.x;
        yPosCamera = BenchmarkDungeonGenerator.dungeonCenter.y;
        gameObject.transform.position = new Vector3(xPosCamera, yPosCamera, -1);
    }
}

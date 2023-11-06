using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float edgeScrollSpeed = 5.0f; // Speed of camera movement
    public float minX = -10.0f; // Minimum X-axis camera position
    public float maxX = 10.0f; // Maximum X-axis camera position
    public float minY = -10.0f; // Minimum Y-axis camera position
    public float maxY = 10.0f; // Maximum Y-axis camera position

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Check if the mouse is at the edge of the screen
        if (mousePosition.x <= 0)
        {
            // Move the camera left
            MoveCamera(Vector3.left);
        }
        else if (mousePosition.x >= Screen.width - 1)
        {
            // Move the camera right
            MoveCamera(Vector3.right);
        }

        if (mousePosition.y <= 0)
        {
            // Move the camera down
            MoveCamera(Vector3.down);
        }
        else if (mousePosition.y >= Screen.height - 1)
        {
            // Move the camera up
            MoveCamera(Vector3.up);
        }
    }

    private void MoveCamera(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * edgeScrollSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }
}

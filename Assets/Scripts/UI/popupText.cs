using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupText : MonoBehaviour
{
    public float initialSpeed = 1.0f; // Initial upward speed
    public float acceleration = 0.1f; // Rate of speed decrease over time
    public float maxSpeed = 5.0f; // Maximum speed

    private float currentSpeed;

    void Start()
    {
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        // Move the object upward
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        // Decrease the speed over time
        currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.deltaTime, 0, maxSpeed);
    }
}

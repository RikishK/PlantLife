using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour
{
    public PlantData.Resource resourceType;
    public float rotationSpeed = 30f; // Rotation speed in degrees per second
    public float floatAmplitude = 0.5f; // Amplitude of the floaty motion
    public float floatFrequency = 1.0f; // Frequency of the floaty motion
    public float horizontalSpeed = 0.1f; // Speed of horizontal movement

    private Vector3 initialPosition;
    private float randomOffset;

    void Start()
    {
        // Store the initial position of the object
        Setup();

        // Generate a random offset for the floaty motion to make it look less uniform
        randomOffset = Random.Range(0f, 360f);
        StartCoroutine(DeathTimer());
    }

    public void Setup(){
        initialPosition = transform.position;
    }

    private IEnumerator DeathTimer(){
        yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }

    void Update()
    {
        // Rotate the object on the Z-axis slowly
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Move the object in a floaty fashion
        float floatOffset = Mathf.Sin((Time.time + randomOffset) * floatFrequency) * floatAmplitude;
        Vector3 newPosition = initialPosition + Vector3.up * floatOffset;

        // Add random horizontal movement
        float horizontalOffset = Mathf.PerlinNoise(Time.time * horizontalSpeed, randomOffset) * 2f - 1f;
        newPosition += Vector3.right * horizontalOffset;

        transform.position = newPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N2Spawner : MonoBehaviour
{
    public GameObject objectToInstantiate; // The object to be instantiated
    public float minInterval = 1.0f; // Minimum time interval between instantiations
    public float maxInterval = 3.0f; // Maximum time interval between instantiations
    public Vector2 spawnRange = new Vector2(10f, 10f); // Range of random positions

    private void Start()
    {
        StartCoroutine(InstantiateObjectsPeriodically());
    }

    private IEnumerator InstantiateObjectsPeriodically()
    {
        while (true)
        {
            // Wait for a random interval between minInterval and maxInterval
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            // Randomly determine the position within the spawnRange
            Vector3 randomPosition = new Vector3(
                transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                transform.position.z
            );

            // Instantiate the object at the random position
            Instantiate(objectToInstantiate, randomPosition, Quaternion.identity);
        }
    }
}

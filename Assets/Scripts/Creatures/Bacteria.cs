using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : MonoBehaviour
{
    public string targetTag = "Target"; // The tag of the object to search for
    public PlantData.Resource targetResource;
    public float searchRange = 10f; // The range to search for the target
    public float moveSpeed = 5f; // The speed at which the GameObject moves
    public float grabDistance = 1f; // The distance at which it grabs the target
    public float timerDuration = 3.0f; // The duration of the timer
    public GameObject prefabToInstantiate; // The prefab to instantiate

    private Transform target;
    private State currentState = State.Searching;

    private enum State
    {
        Searching,
        MovingToTarget,
        Grabbing,
        Timer,
        Instantiating,
    }

    private void Start()
    {
        StartCoroutine(StateMachineRoutine());
    }

    private IEnumerator StateMachineRoutine()
    {
        while (true)
        {
            
            switch (currentState)
            {
                case State.Searching:
                    FindTarget();
                    break;

                case State.MovingToTarget:
                    MoveTowardsTarget();
                    break;

                case State.Grabbing:
                    GrabTarget();
                    break;

                case State.Timer:
                    yield return new WaitForSeconds(timerDuration);
                    currentState = State.Instantiating;
                    break;

                case State.Instantiating:
                    InstantiateObject();
                    currentState = State.Searching;
                    break;
            }

            yield return null; // Wait for the next frame
        }
    }

    private void FindTarget()
    {
        target = FindClosestTarget();
        if (target != null)
        {
            currentState = State.MovingToTarget;
        }
    }

    private Transform FindClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRange);
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            
            if (collider.CompareTag(targetTag))
            {
                
                if(collider.GetComponent<Molecule>().resourceType == targetResource){
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = collider.transform;
                    }
                }
            }
        }

        return closestTarget;
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            // Calculate the direction vector from the current position to the target's position in 2D space.
            Vector2 direction = target.position - transform.position;

            // Calculate the angle in radians from the direction vector.
            float angle = Mathf.Atan2(direction.y, direction.x);

            // Convert the angle to degrees and set the rotation in 2D space.
            float rotationInDegrees = angle * Mathf.Rad2Deg;

            // Update the GameObject's rotation to directly face the target.
            transform.rotation = Quaternion.Euler(0, 0, rotationInDegrees);

            // Move the GameObject in the forward direction (2D space).
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < grabDistance)
            {
                currentState = State.Grabbing;
            }
        }
        else
        {
            currentState = State.Searching;
        }
    }

    private void GrabTarget()
    {
        if (target != null)
        {
            Destroy(target.gameObject);
        }

        currentState = State.Timer;
    }

    private void InstantiateObject()
    {
        if (prefabToInstantiate != null)
        {
            Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
        }
    }
}

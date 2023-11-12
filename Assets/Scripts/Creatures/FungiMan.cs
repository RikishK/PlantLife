using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiMan : Creature
{
    private enum State
    {
        Searching,
        MovingTowardsObject,
        Interrupted,
    }

    private State currentState = State.Searching;
    private GameObject targetObject;
    private Vector2 interruptPosition;
    private int nitrateEaten = 0;

    public float searchRange = 10f;
    public float moveSpeed = 5f;

    void Start()
    {
        StartCoroutine(StateMachineRoutine());
    }

    void Update()
    {
        if (currentState == State.Interrupted)
        {
            // Move towards the interrupt position
            transform.position = Vector2.MoveTowards(transform.position, interruptPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, interruptPosition) < 0.1f)
            {
                FindObjectOfType<GameManager>().GainResource(PlantData.Resource.Nitrate, nitrateEaten * 5);
                nitrateEaten = 0;
                currentState = State.Searching;
            }
        }
    }

    IEnumerator StateMachineRoutine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Searching:
                    FindTarget();
                    break;

                case State.MovingTowardsObject:
                    MoveTowardsObject();
                    break;

                case State.Interrupted:
                    // Handle interruption, e.g., move towards an interrupt position
                    break;
            }

            yield return null; // Wait for the next frame
        }
    }

    void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Molecule"))
            {
                Molecule moleculeScript = collider.GetComponent<Molecule>();

                if (moleculeScript != null && moleculeScript.resourceType == PlantData.Resource.Nitrate)
                {
                    targetObject = collider.gameObject;
                    currentState = State.MovingTowardsObject;
                    return;
                }
            }
        }
    }

    void MoveTowardsObject()
    {
        if (targetObject != null)
        {
            Vector2 direction = (Vector2)targetObject.transform.position - (Vector2)transform.position;
            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetObject.transform.position) < 0.1f)
            {
                // Consume the molecule (e.g., increase nitrate_eaten counter)
                nitrateEaten++;
                Destroy(targetObject);
                currentState = State.Searching;
            }
        }
    }

    // Function to interrupt the regular behavior
    public void Interrupt(Vector2 position)
    {
        Debug.Log("interrupted");
        if(nitrateEaten > 0){
            interruptPosition = position;
            currentState = State.Interrupted;
        }
    }
}

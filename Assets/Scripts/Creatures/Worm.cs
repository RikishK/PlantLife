using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public float satedTimer = 40.0f;
    public float hungryTimerOrganicMatter = 60.0f;
    public float hungryTimerBacteria = 20.0f;
    public float moveSpeed = 0.5f;
    public float minPauseDuration = 3.0f;
    public float maxPauseDuration = 7.0f;
    public GameObject nitrateObjectPrefab;

    private float currentTimer;
    private float pauseDuration;
    public State currentState;
    public GameObject target;
    private bool shootingNitrate;
    private Vector3 randomDestination;
    private float moveTimer, starveTimer;

    [SerializeField] private Transform renderTransform;
    
    public enum State
    {
        Sated,
        HungryOrganicMatter,
        HungryBacteria,
    }
    
    private void Start()
    {
        currentState = State.Sated;
        currentTimer = satedTimer;
        pauseDuration = UnityEngine.Random.Range(minPauseDuration, maxPauseDuration);
        randomDestination = GetRandomDestination();
        moveTimer = 0.0f;
        starveTimer = 0f;
    }
    
    private void Update()
    {
        // Debug.Log(currentState);
        switch (currentState)
        {
            case State.Sated:
                if (moveTimer >= pauseDuration)
                {
                    randomDestination = GetRandomDestination();
                    moveTimer = 0.0f;
                }
                else
                {
                    float distance = Vector3.Distance(transform.position, randomDestination);
                    if (distance > 0.5f) MoveTowardsTarget(randomDestination);
                }

                moveTimer += Time.deltaTime;

                currentTimer -= Time.deltaTime;
                if (currentTimer <= 0)
                {
                    currentState = State.HungryOrganicMatter;
                    currentTimer = hungryTimerOrganicMatter;
                }
                break;
    
            case State.HungryOrganicMatter:
                if (target == null)
                {
                    starveTimer += Time.deltaTime;
                    if (starveTimer >= 20){
                        Destroy(gameObject);
                    }
                    target = FindClosestObjectWithTag("OrganicMatter");
                    if (target == null){
                        currentState = State.HungryBacteria;
                    }
                }
                else
                {
                    //Debug.Log("organic matter at: " + target.transform.position);
                    MoveTowardsTarget(target.transform.position);
    
                    if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
                    {
                        //EatOrganicMatter(target);
                        StartCoroutine(SpawnNitrate(target.GetComponent<Organic_Matter>().Harvest()));
                        target.GetComponent<Organic_Matter>().Consume();
    
                        currentState = State.Sated;
                        target = null;
                        currentTimer = hungryTimerOrganicMatter;
                        starveTimer = 0f;
                    }
                }
                break;
    
            case State.HungryBacteria:
                if (target == null)
                {
                    starveTimer += Time.deltaTime;
                    if (starveTimer >= 20){
                        Destroy(gameObject);
                    }
                    target = FindClosestObjectWithTag("Bacteria");
                    if (target == null){
                        currentState = State.HungryOrganicMatter;
                    }
                }
                else
                {
                    MoveTowardsTarget(target.transform.position);
    
                    if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
                    {
                        Destroy(target);
                        currentState = State.Sated;
                        target = null;
                        currentTimer = hungryTimerBacteria;
                        starveTimer = 0f;
                    }
                }
                break;
        }
    }
    
     private Vector3 GetRandomDestination()
    {
        float randomX = UnityEngine.Random.Range(transform.position.x - 5, transform.position.x + 5);
        float randomY = UnityEngine.Random.Range(transform.position.y - 5, Mathf.Min(transform.position.y + 5, -1.5f));
        return new Vector3(randomX, randomY, 0);
    }
    
    private GameObject FindClosestObjectWithTag(string tag)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20.0f);

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = collider.gameObject;
                }
            }
        }

        return closestObject;
    }
    
    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        // Calculate the direction vector from the current position to the target's position in 2D space.
        Vector2 direction = (Vector2)targetPosition - (Vector2)transform.position;
        
        // Move the GameObject in the forward direction (2D space).
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

        // Calculate the angle in radians from the direction vector.
        float angle = Mathf.Atan2(direction.y, direction.x);

        // Convert the angle to degrees and set the rotation in 2D space.
        float rotationInDegrees = angle * Mathf.Rad2Deg;

        // Update the GameObject's rotation to directly face the target.
        renderTransform.rotation = Quaternion.Euler(0, 0, rotationInDegrees - 90f);


    }

    private IEnumerator SpawnNitrate(int count){
        for(int i=0; i < count; i++){
            ShootNitrateObject(transform.position);
            DisableMoleculeScript(target);
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    private void ShootNitrateObject(Vector3 targetPosition)
    {
        GameObject nitrateObject = Instantiate(nitrateObjectPrefab, transform.position, Quaternion.identity);
        
        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Adjust the speed as needed (for example, 2.0f)
        float speed = 2.0f;

        // Set the initial position of the nitrateObject
        nitrateObject.transform.position = transform.position;

        // Start moving the nitrateObject towards the target position
        StartCoroutine(MoveNitrateObject(nitrateObject, direction, speed));
    }

    private IEnumerator MoveNitrateObject(GameObject nitrateObject, Vector3 direction, float speed)
    {
        float distance = 0f;
        float stoppingDistance = 5.0f; // Adjust the stopping distance as needed

        while (nitrateObject != null && distance < stoppingDistance)
        {
            // Move the nitrateObject towards the target
            nitrateObject.transform.position += direction * speed * Time.deltaTime;

            // Update the distance
            distance = Vector3.Distance(nitrateObject.transform.position, transform.position);

            yield return null;
        }

        if (nitrateObject != null){
            // Enable the Molecule script when it reaches the stopping distance
            Molecule moleculeScript = nitrateObject.GetComponent<Molecule>();
            if (moleculeScript != null)
            {
                moleculeScript.enabled = true;
            }
        }

    }
    
    private void DisableMoleculeScript(GameObject target)
    {
        if (target == null) return;
        Molecule moleculeScript = target.GetComponent<Molecule>();
        if (moleculeScript != null)
        {
            moleculeScript.enabled = false;
        }
    }
}

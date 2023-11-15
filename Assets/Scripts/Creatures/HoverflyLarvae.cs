using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverflyLarvae : Creature
{
    public float moveSpeed = 2f;
    public float distance = 2f;
    public LayerMask plantBlockLayer;
    private bool isFacingRight = true;
    private HoverflyLarvaeState hoverflyLarvaeState = HoverflyLarvaeState.Idle;
    private enum HoverflyLarvaeState {
        Idle, FindingTarget, Eating, Evolving
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HoverflyLarvaeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator HoverflyLarvaeRoutine(){
        while(true){
            switch (hoverflyLarvaeState){
                case HoverflyLarvaeState.Idle:
                    MoveRandom();
                    yield return new WaitForSeconds(10f);
                    break;
                case HoverflyLarvaeState.FindingTarget:
                    break;
                case HoverflyLarvaeState.Eating:
                    break;
                case HoverflyLarvaeState.Evolving:
                    break;
            }
            yield return null;
        }
        
    }

    private void MoveRandom(float timeOverride = 0f)
    {
        // Generate a random direction (left or right)
        int randomDirection = Random.Range(0, 2) * 2 - 1; // -1 for left, 1 for right
        float moveDistance = distance * Random.Range(0f, 2f);
        // Calculate the movement vector based on the random direction and distance
        Vector2 movementVector = new Vector2(randomDirection * moveDistance, 0f);

        // Transform the movement vector based on the object's current rotation
        movementVector = transform.rotation * movementVector;

        // Calculate the target position based on the rotated movement vector
        Vector2 targetPosition = new Vector2(transform.position.x + movementVector.x, transform.position.y + movementVector.y);

        // Flip the sprite renderer if moving right
        if (randomDirection == 1)
        {
            isFacingRight = true;
            OrientSpriteHorizontal();
        }
        else{
            isFacingRight = false;
            OrientSpriteHorizontal();
        }

        // Move the object to the target position
        StartCoroutine(MoveObject(targetPosition, timeOverride));
    }

    private IEnumerator MoveObject(Vector2 targetPosition, float timeOverride = 0f)
    {
        float elapsedTime = timeOverride;
        Vector2 startingPosition = transform.position;

        while (elapsedTime < moveSpeed)
        {
            // Calculate the new position
            Vector2 newPosition = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / moveSpeed);

            // Clamp the Y coordinate to prevent going below a certain limit
            newPosition.y = Mathf.Max(newPosition.y, -0.95f);

            // Update the object's position
            transform.position = newPosition;

            // Check for wall and handle rotation
            if (CheckForWall()) break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (elapsedTime < moveSpeed) MoveRandom(elapsedTime);

        // If the object goes below the limit, reset its rotation to 0
        if (transform.position.y <= -0.95f)
        {
            transform.rotation = Quaternion.identity;
        }

    }

    private bool CheckForWall()
    {
        // Check if there is a wall (Collider2D with "PlantBlock" tag) in front of the object
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left * (isFacingRight ? -1 : 1)), 0.1f, plantBlockLayer);
        if (hit.collider != null)
        {
            Debug.Log("Hit Wall");
            
            float angle = Mathf.Atan2(-hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;
            Debug.Log(angle);

            // Rotate the object to match the wall's orientation    
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            return true;
        }
        else
        {
            //Debug.Log("No Wall");
            return false;
        }
    }

    private void OrientSpriteHorizontal()
    {
        transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1);
    }

}

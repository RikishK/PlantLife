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

    private void MoveRandom()
    {
        // Generate a random direction (left or right)
        int randomDirection = Random.Range(0, 2) * 2 - 1; // -1 for left, 1 for right
        float moveDistance = distance * Random.Range(0f, 2f);
        // Calculate the target position based on the random direction and distance
        Vector2 targetPosition = new Vector2(transform.position.x + randomDirection * moveDistance, transform.position.y);

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
        StartCoroutine(MoveObject(targetPosition));
    }

    private IEnumerator MoveObject(Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = transform.position;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            if (CheckForWall()) break;
            yield return null;
        }

    }

    private bool CheckForWall()
    {
        // Check if there is a wall (Collider2D with "PlantBlock" tag) in front of the object
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * (isFacingRight ? -1 : 1), 0.1f, plantBlockLayer);
        if (hit.collider != null)
        {
            Debug.Log("Hit Wall");
            // Calculate the angle between the object's current up direction and the wall's normal
            //float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
            //float angle = Mathf.Atan2(hit.point.y - transform.position.y, hit.point.x - transform.position.x) * Mathf.Rad2Deg;
            float angle = Mathf.Atan2(-hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;
            Debug.Log(angle);

            // Rotate the object to match the wall's orientation    
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Calculate the target position on the wall's surface based on the angle
            // Vector2 targetPosition = new Vector2(hit.point.x + Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
            //                                     hit.point.y + Mathf.Sin(angle * Mathf.Deg2Rad) * distance);

            // Move to the target position on the wall
            //StartCoroutine(MoveObject(targetPosition));
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

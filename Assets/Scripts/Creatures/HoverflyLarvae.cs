using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverflyLarvae : Creature
{
    // Manual override for random move
    public bool manualOverride = false;
    public int manualOverrideDirection = 0;

    public float moveSpeed = 2f;
    public float distance = 2f;
    public LayerMask plantBlockLayer, floorLayer;
    private bool isFacingRight = true;
    private HoverflyLarvaeState hoverflyLarvaeState = HoverflyLarvaeState.Idle;
    private Collider2D previousFloorHitCollider;
    [SerializeField] private Transform detectionPoint, stickPoint, wallCornerPoint;
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
        int randomDirection = Random.Range(-1, 2); // -1 for left, 1 for right
        if (manualOverride) randomDirection = manualOverrideDirection;
        float moveDistance = distance * Random.Range(0f, 2.0f);

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
        StartCoroutine(MoveObject(targetPosition));
    }

    private IEnumerator MoveObject(Vector2 targetPosition)
    {
        Debug.Log("Target: " + targetPosition);
        float elapsedTime = 0f;
        Vector2 startingPosition = new Vector2(transform.position.x, transform.position.y);

        while (elapsedTime < moveSpeed)
        {
            // Calculate the new position
            Vector2 newPosition = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / moveSpeed);

            // Update the object's position
            transform.position = newPosition;

            // Check if the object is making contact with the floor (Collider2D with "PlantBlock" or "Floor" tag) below it
            RaycastHit2D floorHit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 0.3f, plantBlockLayer | floorLayer);
            Collider2D floorHitCollider = null;
            if(floorHit.collider){
                Plant_Block potential_leaf = floorHit.collider.GetComponent<Plant_Block>();
                floorHitCollider = floorHit.collider;
                if(potential_leaf != null && potential_leaf.BlockType() == PlantData.BlockType.Leaf){
                    floorHitCollider = null;
                }
            }
            if (floorHitCollider == null) {
                if (previousFloorHitCollider != null) {
                    Debug.Log("Snapping around corner");

                    // Get the closest point on the collider's edge to the current object position
                    Vector2 closestPoint = previousFloorHitCollider.ClosestPoint(wallCornerPoint.position);

                    // Calculate the rotation angle based on the direction from stickPoint to the hit collider
                    Vector2 directionToWallCornerPoint = previousFloorHitCollider.transform.InverseTransformDirection(closestPoint - (Vector2)previousFloorHitCollider.bounds.center);
                    // float rotationAngle = Mathf.Atan2(directionToWallCornerPoint.y, directionToWallCornerPoint.x) * Mathf.Rad2Deg;
                    // float snappedAngle = Mathf.Round(rotationAngle / 90) * 90;
                    // // Adjust the rotation angle based on the collider's orientation
                    // float colliderRotation = previousFloorHitCollider.transform.eulerAngles.z;

                    // // Set the object's rotation to be parallel to the side of the collider
                    // transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle - colliderRotation - 90f);
                    Debug.Log("Corner Snap Direction: " + directionToWallCornerPoint);
                    if (directionToWallCornerPoint.y > 0.2){
                        transform.rotation = Quaternion.Euler(0f, 0f, previousFloorHitCollider.transform.eulerAngles.z);
                    }
                    else{
                        if(directionToWallCornerPoint.x > 0){
                            transform.rotation = Quaternion.Euler(0f, 0f, previousFloorHitCollider.transform.eulerAngles.z - 90f);
                        }
                        else {
                            transform.rotation = Quaternion.Euler(0f, 0f, previousFloorHitCollider.transform.eulerAngles.z + 90f);
                        }
                    }
                    
                    // Snap the object to the closest point and align its rotation with the collider's normal
                    transform.position = closestPoint;
                }
                break;
            }
            else {
                previousFloorHitCollider = floorHit.collider;
            }

            // Check for wall and handle rotation
            if (CheckForWall()) break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // if (elapsedTime < moveSpeed){
        //     int chance = Random.Range(1, 100);
        //     if (chance > 80) MoveRandom();
        // }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw the ray in the Scene view
        Vector2 raycastDirection = transform.TransformDirection(Vector2.down);
        Gizmos.DrawRay(transform.position, raycastDirection * 0.3f);
        Gizmos.DrawRay(detectionPoint.position,transform.TransformDirection(Vector2.left * (isFacingRight ? -1 : 1)) * 0.1f);
    }

    private bool CheckForWall()
    {
        // Check if there is a wall (Collider2D with "PlantBlock" tag) in front of the object
        RaycastHit2D hit = Physics2D.Raycast(detectionPoint.position, transform.TransformDirection(Vector2.left * (isFacingRight ? -1 : 1)), 0.1f, plantBlockLayer | floorLayer);
        Collider2D wallCollider = null;
        if(hit.collider){
            wallCollider = hit.collider;
            Plant_Block potential_leaf = hit.collider.GetComponent<Plant_Block>();
            if(potential_leaf != null && potential_leaf.BlockType() == PlantData.BlockType.Leaf){
                wallCollider = null;
            }
        }

        if (wallCollider != null)
        {
            // Get the closest point on the collider's edge to the current object position
            Vector2 closestPoint = hit.collider.ClosestPoint(stickPoint.position);

            // Calculate the rotation angle based on the direction from stickPoint to the hit collider
            //Vector2 directionToStickPoint = hit.collider.transform.InverseTransformDirection(stickPoint.position - hit.collider.bounds.center);
            Vector2 directionToStickPoint = hit.collider.transform.InverseTransformDirection(closestPoint - (Vector2)hit.collider.bounds.center);
            
            // float rotationAngle = Mathf.Atan2(directionToStickPoint.y, directionToStickPoint.x) * Mathf.Rad2Deg;
            // float snappedAngle = Mathf.Round(rotationAngle / 90) * 90;
            // // Adjust the rotation angle based on the collider's orientation
            // float colliderRotation = hit.collider.transform.eulerAngles.z;

            // // Set the object's rotation to be parallel to the side of the collider
            // transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle - colliderRotation - 90f);
            // Debug.Log("DirectionToStickPoint: " + directionToStickPoint + "Rotation angle: " + rotationAngle + " snappedAngle: " + snappedAngle + " colliderRotation: " + colliderRotation);

            if (directionToStickPoint.x > 0){
                transform.rotation = Quaternion.Euler(0f, 0f, hit.collider.transform.eulerAngles.z - 90f);
            }
            else{
                transform.rotation = Quaternion.Euler(0f, 0f, hit.collider.transform.eulerAngles.z + 90f);
            }

            if(hit.collider.tag == "Floor"){
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            Debug.Log("Wall: " + hit.collider);
            Debug.Log("ClosestPoint: " + closestPoint);
            // Snap the object to the closest point and align its rotation with the collider's normal
            transform.position = closestPoint;

            return true;
        }
        else
        {
            // Debug.Log("No Wall");
            return false;
        }
    }

    private void OrientSpriteHorizontal()
    {
        transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1);
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class HoverflyLarvae : Creature
{
    // Manual override for random move
    public bool manualOverride = false;
    public int manualOverrideDirection = 0;

    public float moveSpeed = 2f, distance = 2f, attackRange = 2f, attackSpeed = 1f, targetDetectionRange = 5f;
    public int attackDamage = 10;
    private int aphidsEaten = 0;
    private float startTime, bonusTime = 0f;
    public LayerMask plantBlockLayer, floorLayer;
    private bool isFacingRight = true, isTargetingEnemy = false;
    private HoverflyLarvaeState hoverflyLarvaeState = HoverflyLarvaeState.Idle;
    private Collider2D previousFloorHitCollider;
    private GameObject targetObj;
    [SerializeField] private Transform detectionPoint, stickPoint, wallCornerPoint;
    [SerializeField] private GameObject HoverflyPrefab;
    private enum HoverflyLarvaeState {
        Idle, Attacking, Evolving
    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        StartCoroutine(HoverflyLarvaeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        float current_time = Time.time;
        if(180 - (current_time - (startTime + bonusTime)) <= 0){
            Die();
        }
    }

    private void Die(){
        //TODO: Summon dead hoverfly larvae 
        Destroy(gameObject);
    }

    private IEnumerator HoverflyLarvaeRoutine(){
        while(true){
            switch (hoverflyLarvaeState){
                case HoverflyLarvaeState.Idle:
                    if(targetObj == null){
                        MoveRandom();
                        targetObj = SearchForTarget();
                        yield return new WaitForSeconds(5f);
                    }
                    else{
                        if(!isTargetingEnemy){
                            float current_time = Time.time;
                            if(180 - (current_time - (startTime + bonusTime)) > 90){
                                targetObj = null;
                                break;
                            }
                        }
                        float target_distance = Vector3.Distance(transform.position, targetObj.transform.position);
                        if (target_distance <= attackRange){
                            hoverflyLarvaeState = HoverflyLarvaeState.Attacking;
                        }
                        else{
                            int direction = transform.position.x - targetObj.transform.position.x > 0 ? -1 : 1;
                            MoveSpecific(direction);
                            yield return new WaitForSeconds(3f);
                            targetObj = SearchForTarget();
                        }
                    }
                    break;
                case HoverflyLarvaeState.Attacking:
                    if(targetObj){
                        if(isTargetingEnemy){
                            Enemy enemyScript = targetObj.GetComponent<Enemy>();
                            enemyScript.TakeDamage(attackDamage);
                            float seconds_to_wait = 1f / attackSpeed;
                            yield return new WaitForSeconds(seconds_to_wait);
                            if (targetObj == null){
                                bonusTime += 3f;
                                targetObj = SearchForTarget();
                                if(targetObj == null) hoverflyLarvaeState = HoverflyLarvaeState.Idle; 
                            }
                        }
                        else{
                            Aphid aphidScript = targetObj.GetComponent<Aphid>();
                            aphidScript.Die();
                            Debug.Log("Larvae ate ahpid creature");
                            aphidsEaten++;
                            bonusTime += 30f;
                            if (aphidsEaten == 3) hoverflyLarvaeState = HoverflyLarvaeState.Evolving;
                            else hoverflyLarvaeState = HoverflyLarvaeState.Idle;
                        }
                    }
                    else{
                        hoverflyLarvaeState = HoverflyLarvaeState.Idle;
                    }
                    break;
                case HoverflyLarvaeState.Evolving:
                    Evolve();
                    break;
            }
            yield return null;
        }
        
    }

    private void MoveRandom()
    {
        // Generate a random direction (left or right)
        int randomDirection = Random.Range(-1, 2); // -1 for left, 1 for right
        if(transform.position.x > 7) randomDirection = -1;
        if(transform.position.x < -7) randomDirection = 1;
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

    private void MoveSpecific(int direction)
    {
        if (manualOverride) direction = manualOverrideDirection;
        float moveDistance = distance * Random.Range(0f, 2.0f);

        // Calculate the movement vector based on the random direction and distance
        Vector2 movementVector = new Vector2(direction * moveDistance, 0f);

        // Transform the movement vector based on the object's current rotation
        movementVector = transform.rotation * movementVector;

        // Calculate the target position based on the rotated movement vector
        Vector2 targetPosition = new Vector2(transform.position.x + movementVector.x, transform.position.y + movementVector.y);

        // Flip the sprite renderer if moving right
        if (direction == 1)
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

                    // Get the closest point on the collider's edge to the current object position
                    Vector2 closestPoint = previousFloorHitCollider.ClosestPoint(wallCornerPoint.position);

                    // Calculate the rotation angle based on the direction from stickPoint to the hit collider
                    Vector2 directionToWallCornerPoint = previousFloorHitCollider.transform.InverseTransformDirection(closestPoint - (Vector2)previousFloorHitCollider.bounds.center);
                    
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
            

            if (directionToStickPoint.x > 0){
                transform.rotation = Quaternion.Euler(0f, 0f, hit.collider.transform.eulerAngles.z - 90f);
            }
            else{
                transform.rotation = Quaternion.Euler(0f, 0f, hit.collider.transform.eulerAngles.z + 90f);
            }

            if(hit.collider.tag == "Floor"){
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            // Snap the object to the closest point and align its rotation with the collider's normal
            transform.position = closestPoint;

            return true;
        }
        else
        {
            return false;
        }
    }

    private void OrientSpriteHorizontal()
    {
        transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1);
    }

    private GameObject SearchForTarget(){
        GameObject[] creatureObjects = GameObject.FindGameObjectsWithTag("Creature");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        float enemyDistance = float.MaxValue;
        GameObject closestEnemy = null;

        foreach(GameObject enemyObject in enemyObjects){
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            if (enemyScript.EnemyType() == EnemyData.EnemyType.AphidEnemy){
                float distance = Vector3.Distance(transform.position, enemyObject.transform.position);
                if (distance < enemyDistance && distance < targetDetectionRange){
                    enemyDistance = distance;
                    closestEnemy = enemyObject;
                }
            }
        }
        if(closestEnemy){
            isTargetingEnemy = true;
            return closestEnemy;
        }

        float creatureDistance = float.MaxValue;
        GameObject closestCreature = null;

        foreach(GameObject creatureObject in creatureObjects){
            Creature creatureScript = creatureObject.GetComponent<Creature>();
            if (creatureScript.creatureType == CreatureSpawnData.CreatureType.Aphid){
                float distance = Vector3.Distance(transform.position, creatureObject.transform.position);
                if(distance < creatureDistance && distance < targetDetectionRange){
                    creatureDistance = distance;
                    closestCreature = creatureObject;
                }
            }
        }

        if(closestCreature){
            isTargetingEnemy = false;
            return closestCreature;
        }

        return null;
    }

    private void Evolve(){
        GameObject hoverfly = Instantiate(HoverflyPrefab);
        hoverfly.transform.position = transform.position;
        Destroy(gameObject);
    }

}

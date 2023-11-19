using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : Creature
{
    private AphidState aphidState = AphidState.FindingTarget;
    private Transform target;
    private float moveSpeed = 2.0f;

    private int nitrate_value, leaves_eaten;

    private float startTime, bonusTime = 0f; 
    [SerializeField] private GameObject deadAphid;
    [SerializeField] private GameObject AphidObj;

    // Start is called before the first frame update
    void Start()
    {   
        startTime = Time.time;
        nitrate_value = 0;
        leaves_eaten = 0;
        StartCoroutine(AphidRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        float current_time = Time.time;
        if(60 - (current_time - (startTime + bonusTime)) <= 0){
            Die();
        }
    }

    public void Die(){
        GameObject deadAphidObj = Instantiate(deadAphid);
        deadAphidObj.transform.position = transform.position; 
        deadAphidObj.GetComponent<Organic_Matter>().Setup(nitrate_value/2);
        Destroy(gameObject);
    }

    private enum AphidState{
        Idle,
        FindingTarget,
        MovingToTarget,
        Eating
    }

    private IEnumerator AphidRoutine(){
        while(true){
            switch(aphidState){
                case AphidState.Idle:
                    float current_time = Time.time;
                    if(60 - (current_time - (startTime + bonusTime)) <= 30){
                        aphidState = AphidState.FindingTarget;
                    }
                    break;
                case AphidState.FindingTarget:
                    target = null;
                    target = FindClosestLeaf();
                    if (target != null) aphidState = AphidState.MovingToTarget;
                    break;
                case AphidState.MovingToTarget:
                    MoveTowardsTarget();
                    break;
                case AphidState.Eating:
                    EatLeaf();
                    break;
            }
            yield return null;
        }
    }



    private Transform FindClosestLeaf(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 4f);
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PlantBlock"))
            {
                if(collider.GetComponent<Plant_Block>().BlockType() == PlantData.BlockType.Leaf){
                    Plant_Leaf plant_Leaf = collider.GetComponent<Plant_Leaf>();
                    if(plant_Leaf.LeafState() == PlantData.LeafState.Medium || plant_Leaf.LeafState() == PlantData.LeafState.Large){
                        float distance = Vector3.Distance(transform.position, collider.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestTarget = collider.transform;
                        }
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

            if (Vector2.Distance(transform.position, target.position) < 0.3f)
            {
                aphidState = AphidState.Eating;
            }
        }
        else
        {
            aphidState = AphidState.FindingTarget;
        }
    }

    private void EatLeaf(){
        Plant_Leaf plant_Leaf = target.GetComponent<Plant_Leaf>();
        if(plant_Leaf.LeafState() == PlantData.LeafState.Medium || plant_Leaf.LeafState() == PlantData.LeafState.Large){
            if (plant_Leaf.LeafState() == PlantData.LeafState.Medium){
                bonusTime += 10f;
                leaves_eaten += 1;   
            }
            if (plant_Leaf.LeafState() == PlantData.LeafState.Large){
                bonusTime += 25f;
                leaves_eaten += 2;
            };
            int nitrate_gained = plant_Leaf.EatLeaf(50);
            nitrate_value += nitrate_gained;
            if (leaves_eaten >= 3){
                leaves_eaten -= 3;
                GameObject offspring = Instantiate(AphidObj);
                offspring.transform.position = new Vector3(transform.position.x + 1f, transform.position.y - 0.5f, 0);
            }
            moveSpeed = 2.0f - 0.5f * leaves_eaten;
            aphidState = AphidState.Idle;
        }
        else{
            aphidState = AphidState.FindingTarget;
        }
    }
}

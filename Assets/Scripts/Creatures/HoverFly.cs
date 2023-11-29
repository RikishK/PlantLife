using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverFly : Creature
{
    [SerializeField] private HoverFlyState hoverFlyState = HoverFlyState.Idle;
    private GameObject targetPollon;
    [SerializeField] private LayerMask pollonLayer;
    [SerializeField] private float pollonDetectionRange, hungerTimerRange, lifeTime;
    [SerializeField] private Transform center, pollonGrabPoint;
    private float start_time, bonus_time, moveSpeed = 2.0f;
    private enum HoverFlyState{
        Idle, ChasingPollon, DeliveringPollon, EatingPollon
    }
    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;
        bonus_time = 0;
        StartCoroutine(HoverflyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        float current_time = Time.time;
        if(lifeTime - (current_time - (start_time + bonus_time)) <= 0){
            Die();
        }
    }

    private void Die(){

    }

    private IEnumerator HoverflyRoutine(){
        while(true){
            switch(hoverFlyState){
                case HoverFlyState.Idle:
                    targetPollon = FindPollon();
                    if(targetPollon) hoverFlyState = HoverFlyState.ChasingPollon;
                    MoveRandom();
                    break;
                case HoverFlyState.ChasingPollon:
                    if(targetPollon == null){
                        hoverFlyState = HoverFlyState.Idle;
                        break;
                    }
                    float distance = Vector2.Distance(targetPollon.transform.position, transform.position);
                    if(distance < 1f){
                        // Switch to delivering if experience pollon
                        Pollon pollonScript = targetPollon.GetComponent<Pollon>();
                        switch(pollonScript.pollonType){
                            case Pollon.PollonType.Regular:
                                // Grab and eat
                                targetPollon.transform.position = pollonGrabPoint.position;
                                targetPollon.transform.parent = pollonGrabPoint;
                                hoverFlyState = HoverFlyState.EatingPollon;
                                break;
                            case Pollon.PollonType.Experience:
                                // Grab and deliver
                                targetPollon.transform.position = pollonGrabPoint.position;
                                targetPollon.transform.parent = pollonGrabPoint;
                                hoverFlyState = HoverFlyState.DeliveringPollon;
                                break;
                        }  
                    }
                    else{
                        MoveTowardsTarget();
                    }
                    break;
                case HoverFlyState.DeliveringPollon:
                    if(targetPollon == null){
                        hoverFlyState = HoverFlyState.Idle;
                        break;
                    }
                    Vector3 targetPos = targetPollon.GetComponent<ExperiencePollon>().DestinationFlower().transform.position;
                    float deliver_distance = Vector2.Distance(targetPos, transform.position);
                    if(deliver_distance < 0.3f){
                        // Give exp to flower
                        int exp_gain = targetPollon.GetComponent<ExperiencePollon>().ExperienceAmount();
                        targetPollon.GetComponent<ExperiencePollon>().DestinationFlower().GainExperience(exp_gain);
                        Destroy(targetPollon);
                    }
                    else{
                        DeliverPollon();
                    }
                    break;
                case HoverFlyState.EatingPollon:
                    MoveRandom();
                    break;
            }
            yield return null;
        }
    }

    private GameObject FindPollon(){
        
        float min_distance = float.MaxValue;
        GameObject pollon = null;
        Collider2D[] pollon_objs = Physics2D.OverlapCircleAll(center.position, pollonDetectionRange, pollonLayer);
        
        foreach(Collider2D pollon_obj in pollon_objs){
            Pollon pollonScript = pollon_obj.GetComponent<Pollon>();
            if (pollonScript.pollonType == Pollon.PollonType.Regular){
                // check timer is in hunger value
                float current_time = Time.time;
                if(lifeTime - (current_time - (start_time + bonus_time)) <= hungerTimerRange){
                    // distance
                    float distance = Vector3.Distance(pollon_obj.transform.position, center.position);
                    if(distance < min_distance){
                        min_distance = distance;
                        pollon = pollon_obj.gameObject;
                    }
                }
            }
            else if(pollonScript.pollonType == Pollon.PollonType.Experience){
                // distance
                float distance = Vector3.Distance(pollon_obj.transform.position, center.position);
                if(distance < min_distance){
                    min_distance = distance;
                    pollon = pollon_obj.gameObject;
                }
            }
        }

        if (pollon) return pollon;

        return null;
    }

    private void MoveTowardsTarget()
    {
        
        // Get the current position of the object
        Vector2 currentPosition = transform.position;

        // Get the target position
        Vector2 targetPosition = targetPollon.transform.position;

        // Calculate the new position towards the target
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Update the object's position
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        
    }

    private void MoveRandom()
    {
        // Calculate random movement in X and Y directions
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f); // Ensure Y is above 0

        if(transform.position.x > 7) randomX = -0.5f;
        if(transform.position.x < -7) randomX = 0.5f;
        if(transform.position.y > 7) randomY = -0.5f;
        if(transform.position.y < 0) randomY = 0.5f;


        // Create a movement vector
        Vector2 movement = new Vector2(randomX, randomY);

        // Normalize the vector to ensure constant speed in any direction
        movement.Normalize();

        // Update the object's position based on the random movement
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void DeliverPollon()
    {
        
        // Get the current position of the object
        Vector2 currentPosition = transform.position;

        // Get the target position
        Vector2 targetPosition = targetPollon.GetComponent<ExperiencePollon>().DestinationFlower().transform.position;

        // Calculate the new position towards the target
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Update the object's position
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

}

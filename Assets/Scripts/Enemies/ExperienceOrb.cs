using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    private int experience_amount;
    private Flower target_blue_flower;
    private ExperienceOrbState experienceOrbState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private enum ExperienceOrbState {
        Floating, MovingToTarget
    }

    public void Init(int experience_amount){
        this.experience_amount = experience_amount;
        experienceOrbState = ExperienceOrbState.Floating;
    }

    private IEnumerator ExperienceOrbRoutine(){
        while(true){
            switch (experienceOrbState){
                case ExperienceOrbState.Floating:
                    Vector3 random_movement = new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f),
                        Mathf.Clamp(transform.position.y + Random.Range(-0.1f, 0.1f), -0.8f, float.MaxValue), 0f);
                    transform.position = random_movement;
                    break;
                case ExperienceOrbState.MovingToTarget:
                    if (target_blue_flower != null)
                    {
                        // Calculate the direction vector from the current position to the target's position in 2D space.
                        Vector2 direction = target_blue_flower.transform.position - transform.position;

                        // Calculate the angle in radians from the direction vector.
                        float angle = Mathf.Atan2(direction.y, direction.x);

                        // Convert the angle to degrees and set the rotation in 2D space.
                        float rotationInDegrees = angle * Mathf.Rad2Deg;

                        // Update the GameObject's rotation to directly face the target.
                        transform.rotation = Quaternion.Euler(0, 0, rotationInDegrees);

                        // Move the GameObject in the forward direction (2D space).
                        transform.Translate(Vector3.right * 1f * Time.deltaTime);

                        if (Vector2.Distance(transform.position, target_blue_flower.transform.position) < 0.3f)
                        {
                            // Give blue flower experience
                            // Die
                            Die();
                        }
                    }
                    break;
            }
            yield return null;
        }
    }

    private void Die(){
        Destroy(gameObject);
    }

}

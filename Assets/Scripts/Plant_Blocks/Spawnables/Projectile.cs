using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(GameObject target){
        this.target = target;
        StartCoroutine(ProjectileRoutine());
    }

    private IEnumerator ProjectileRoutine(){
        while(true){
            if(target == null) Destroy(gameObject);

            if (target != null)
            {
                // Calculate the direction vector from the current position to the target's position in 2D space.
                Vector2 direction = target.transform.position - transform.position;

                // Move the GameObject in the forward direction (2D space).
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, target.transform.position) < 0.3f)
                {
                    Enemy enemyScript = target.GetComponent<Enemy>();
                    enemyScript.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }

            yield return null;
        }
    }

}

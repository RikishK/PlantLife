using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private float moveSpeed, explosiveRange;
    private int damage, pierceCount;
    private bool isExplosive;
    [SerializeField] private LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(GameObject target, int damage, float moveSpeed, bool isExplosive, float explosiveRange, int pierceCount){
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.explosiveRange = explosiveRange;
        this.pierceCount = pierceCount;
        this.isExplosive = isExplosive;
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
                    HandleDamage();
                    HandleDeath();
                }
            }

            yield return null;
        }
    }

    private void HandleDeath(){
        if(pierceCount > 0){
            Debug.Log("Piercing: " + pierceCount);
            target = FindEnemyTarget();
            pierceCount--;
            Debug.Log(target);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void HandleDamage(){
        if(isExplosive){
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosiveRange, enemyLayer);
            foreach(Collider2D enemy in enemies){
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.TakeDamage(damage);
            }
        }
        
        if(target){
            Enemy enemyScript = target.GetComponent<Enemy>();
            enemyScript.TakeDamage(damage);
        }
    }

    private GameObject FindEnemyTarget(){
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 3f, enemyLayer);

        float target_distance = float.MaxValue;
        GameObject target = null;
        
        
        foreach(Collider2D enemy in enemies){
            Debug.Log("Might pierce to: " + enemy);
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < target_distance && distance <= 3f){
                target_distance = distance;
                target = enemy.gameObject;
            }
        }
        return target;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyData.EnemyType enemyType;
    [SerializeField] protected float moveSpeed, attackRange, attackSpeed, attackDamageWait;
    [SerializeField] protected int attackDamage, health;
    [SerializeField] private GameObject DamageIndicatorPrefab;
    private EnemyState enemyState = EnemyState.SelectingTarget;
    private GameObject targetObj;
    // Start is called before the first frame update
    private enum EnemyState {
        SelectingTarget, MovingTowardsTarget, Attacking
    }
    void Start()
    {
        StartCoroutine(EnemyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EnemyData.EnemyType EnemyType(){
        return enemyType;
    }

    protected virtual IEnumerator EnemyRoutine(){
        while(true){
            switch  (enemyState){
                case EnemyState.SelectingTarget:
                    targetObj = SelectTarget();
                    if (targetObj != null) enemyState = EnemyState.MovingTowardsTarget;
                    break;
                case EnemyState.MovingTowardsTarget:
                    MoveTowardsTarget();
                    break;
                case EnemyState.Attacking:
                    StartCoroutine(AttackTarget());
                    float seconds_to_wait = 1f / attackSpeed;
                    yield return new WaitForSeconds(seconds_to_wait);
                    break;
            }
            yield return null;
        }
    }

    protected virtual GameObject SelectTarget(){
        GameObject[] plant_blocks = GameObject.FindGameObjectsWithTag("PlantBlock");
        foreach(GameObject plant_block in plant_blocks){
            Plant_Block block_block_script = plant_block.GetComponent<Plant_Block>();
            if(block_block_script.BlockType() == PlantData.BlockType.Core){
                return plant_block;
            }
        }
        return null;
    }

    protected virtual void MoveTowardsTarget(){
        if (targetObj != null)
        {
            // Calculate the direction vector from the current position to the target's position in 2D space.
            Vector2 direction = targetObj.transform.position - transform.position;

            // Calculate the angle in radians from the direction vector.
            float angle = Mathf.Atan2(direction.y, direction.x);

            // Convert the angle to degrees and set the rotation in 2D space.
            float rotationInDegrees = angle * Mathf.Rad2Deg;

            // Update the GameObject's rotation to directly face the target.
            transform.rotation = Quaternion.Euler(0, 0, rotationInDegrees);

            // Move the GameObject in the forward direction (2D space).
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetObj.transform.position) <= attackRange)
            {
                enemyState = EnemyState.Attacking;
            }
        }
        else
        {
            enemyState = EnemyState.SelectingTarget;
        }
    }

    protected virtual IEnumerator AttackTarget(){
        Plant_Block target_block = targetObj.GetComponent<Plant_Block>();
        DamageIndicator();
        yield return new WaitForSeconds(attackDamageWait);
        target_block.TakeDamage(attackDamage);
    }

    protected virtual void DamageIndicator(){
        GameObject damageIndicator = Instantiate(DamageIndicatorPrefab);
        Vector3 damageIndicatorPosition = new Vector3(targetObj.transform.position.x + Random.Range(-0.2f, 0.2f), targetObj.transform.position.y + Random.Range(-0.2f, 0.2f), 0);
        damageIndicator.transform.position = damageIndicatorPosition;
        float rotation_z = Random.Range(-30, 30);
        damageIndicator.transform.rotation = Quaternion.Euler(0, 0, rotation_z);
    }

    public void TakeDamage(int damage){
        health -= damage;
        if (health <= 0) Die();
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }
}

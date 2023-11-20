using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Plant_Block
{
    [SerializeField] private PlantData.FlowerType flowerType;
    [SerializeField] private float attackRange, attackSpeed;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private FlowerTargetingOption flowerTargetingOption;

    [SerializeField] private int Max_Glucose_Count;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Animator flowerAnimator;
    private int Glucose_Count;
    private GameObject enemyTargetObj;
    private FlowerState flowerState = FlowerState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        base.Init();

        Glucose_Count = 20;
        StartCoroutine(FlowerRoutine());
    }

    private enum FlowerState {
        Idle, Attacking
    }

    [System.Serializable]
    private enum FlowerTargetingOption{
        Closest_To_Flower, Furthest_From_Flower, Closest_To_Core, Furthest_From_Core
    }    

    private IEnumerator FlowerRoutine(){
        while(true){
            switch (flowerState){
                case FlowerState.Idle:
                    if(Glucose_Count > 0){
                        enemyTargetObj = FindEnemyTarget();
                        if(enemyTargetObj != null) flowerState = FlowerState.Attacking; 
                    }
                    break;
                case FlowerState.Attacking:
                    if (enemyTargetObj == null) {
                        flowerState = FlowerState.Idle;
                        break;
                    }
                    Attack();
                    float seconds_to_wait = 1.0f / attackSpeed;
                    yield return new WaitForSeconds(seconds_to_wait);
                    if (enemyTargetObj == null) {
                        flowerState = FlowerState.Idle;
                        break;
                    }
                    float distance = Vector3.Distance(transform.position, enemyTargetObj.transform.position);
                    if (distance > attackRange) flowerState = FlowerState.Idle;

                    if (enemyTargetObj == null) flowerState = FlowerState.Idle;
                    break;
            }
            
            yield return null;
        }
    }

    private GameObject FindEnemyTarget(){
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        float target_distance; 
        GameObject target = null;

        switch (flowerTargetingOption){
            case FlowerTargetingOption.Closest_To_Flower:
                target_distance = float.MaxValue;
                foreach(Collider2D enemy in enemies){
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < target_distance){
                        target_distance = distance;
                        target = enemy.gameObject;
                    }
                }
                return target;
            case FlowerTargetingOption.Furthest_From_Flower:
                target_distance = float.MinValue;
                foreach(Collider2D enemy in enemies){
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance > target_distance){
                        target_distance = distance;
                        target = enemy.gameObject;
                    }
                }
                return target;
            case FlowerTargetingOption.Closest_To_Core:
                break;
            case FlowerTargetingOption.Furthest_From_Core:
                break;
        }

        return null;


    }

    private void Attack(){
        switch (flowerType){
            case PlantData.FlowerType.Orange:
                SpawnProjectile();
                break;
        }
    }

    private void SpawnProjectile(){
        StartCoroutine(PlayAttackAnimation());
        GameObject projectile = Instantiate(projectilePrefab);
        Debug.Log(projectile);
        projectile.transform.position = shootPoint.position;

        // TODO: Shoot the projectile forward a little bit before starting it

        projectile.GetComponent<Projectile>().Init(enemyTargetObj);
    }

    private IEnumerator PlayAttackAnimation(){
        flowerAnimator.SetBool("Attacking", true);
        yield return new WaitForSeconds(0.5f);
        flowerAnimator.SetBool("Attacking", false);
    }
}

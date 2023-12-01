using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrangeFlower : Flower
{
    [SerializeField] private float attackRange, attackSpeed;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private FlowerTargetingOption flowerTargetingOption;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint, center;
    [SerializeField] private RangeIndicator rangeIndicator;
    [SerializeField] private SpriteRenderer orangeFlowerRenderer;
    [SerializeField] private Slider ammunitionSlider, experienceSlider, healthSlider;

    private GameObject enemyTargetObj;
    private FlowerState flowerState = FlowerState.Idle;

    private int projectileSpeedUpgrades = 0, projectileDamageUpgrades = 0, projectilePiercingShotsUpgrades = 0, projectileExplosiveShotsUpgrade = 0;
    

    [System.Serializable]
    private enum FlowerTargetingOption{
        Closest_To_Flower, Furthest_From_Flower, Closest_To_Core, Furthest_From_Core
    }

    private enum FlowerState {
        Idle, Attacking
    }

    public override void Init()
    {
        base.Init();

        Glucose_Count = 15;
        StartCoroutine(OrangeFlowerRoutine());
        UpdateAmmunitionSlider();
        UpdateExperienceSlider();
        UpdateHealthSlider();
    }

    protected override void InitUpgrades()
    {
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Projectile Speed", 50, PlantData.Resource.Battle_Exp),
            new PlantData.UpgradeData("Projectile Damage", 50, PlantData.Resource.Battle_Exp),
            new PlantData.UpgradeData("Piercing Shots", 200, PlantData.Resource.Battle_Exp),
            new PlantData.UpgradeData("Explosive Shots", 200, PlantData.Resource.Battle_Exp),
        };
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        List<PlantData.UpgradeData> upgradeDatas = new List<PlantData.UpgradeData>();
        if(projectileSpeedUpgrades < 3) upgradeDatas.Add(upgrades[0]);
        if(projectileDamageUpgrades < 3) upgradeDatas.Add(upgrades[1]);
        if(projectilePiercingShotsUpgrades < 3) upgradeDatas.Add(upgrades[2]);
        if(projectileExplosiveShotsUpgrade < 3) upgradeDatas.Add(upgrades[3]);

        return upgradeDatas;
    }

    protected override void performUpgrade(int index)
    {
        PlantData.UpgradeData upgrade = getUpgrades()[index];

        switch(upgrade.name){
            case "Projectile Speed":
                projectileSpeedUpgrades++;
                upgrades[0].cost += 50;
                break;
            case "Projectile Damage":
                projectileDamageUpgrades++;
                upgrades[1].cost += 50;
                break;
            case "Piercing Shots":
                projectilePiercingShotsUpgrades++;
                upgrades[2].cost += 100;
                break;
            case "Explosive Shots":
                projectileExplosiveShotsUpgrade++;
                upgrades[3].cost += 100;
                break;
        }
    }
    protected IEnumerator OrangeFlowerRoutine()
    {
        UpdateAmmunitionSlider();
        while(true){
            switch (flowerState){
                case FlowerState.Idle:
                    if(Glucose_Count > 0){
                        enemyTargetObj = FindEnemyTarget();
                        if(enemyTargetObj != null) flowerState = FlowerState.Attacking; 
                    }
                    break;
                case FlowerState.Attacking:
                    if(Glucose_Count <= 0){
                        flowerState = FlowerState.Idle;
                        break;
                    }
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
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center.position, attackRange, enemyLayer);

        float target_distance; 
        GameObject target = null;

        switch (flowerTargetingOption){
            case FlowerTargetingOption.Closest_To_Flower:
                target_distance = float.MaxValue;
                foreach(Collider2D enemy in enemies){
                    float distance = Vector2.Distance(center.position, enemy.transform.position);
                    if (distance < target_distance && distance <= attackRange){
                        target_distance = distance;
                        target = enemy.gameObject;
                    }
                }
                return target;
            case FlowerTargetingOption.Furthest_From_Flower:
                target_distance = float.MinValue;
                foreach(Collider2D enemy in enemies){
                    float distance = Vector2.Distance(center.position, enemy.transform.position);
                    if (distance > target_distance && distance <= attackRange){
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, attackRange);
    }

    private void Attack(){
        switch (flowerType){
            case PlantData.FlowerType.Orange:
                Glucose_Count--;
                UpdateAmmunitionSlider();
                StartCoroutine(SpawnProjectile());
                break;
        }
    }

    private void UpdateAmmunitionSlider(){
        ammunitionSlider.maxValue = Max_Glucose_Count;
        ammunitionSlider.value = Glucose_Count;
    }

    private void UpdateExperienceSlider(){
        experienceSlider.maxValue = maxBattleExperience;
        experienceSlider.value = flower_battle_experience;
    }

    private void UpdateHealthSlider(){
        healthSlider.maxValue = health;
        healthSlider.value = current_health;
    }

    protected override void TakeDamageExtras()
    {
        UpdateHealthSlider();
    }

    protected override void GainExperienceExtras()
    {
        UpdateExperienceSlider();
    }

    private IEnumerator SpawnProjectile(){
        flowerAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.9f);
        if(enemyTargetObj == null){
            enemyTargetObj = FindEnemyTarget();
        }
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootPoint.position;

        // TODO: Shoot the projectile forward a little bit before starting it

        int damage = (projectileDamageUpgrades * 20) + 20;
        float moveSpeed = (projectileSpeedUpgrades * 0.5f) + 1.0f;  
        bool isExplosive = projectileExplosiveShotsUpgrade > 0;
        float explosiveRange = (projectileExplosiveShotsUpgrade * 0.5f) + 0.5f;
        int pierceCount = projectilePiercingShotsUpgrades;


        projectile.GetComponent<Projectile>().Init(enemyTargetObj, damage, moveSpeed, isExplosive, explosiveRange, pierceCount);
    }

    protected override void HighlightExtras()
    {
        rangeIndicator.gameObject.SetActive(true);
        ammunitionSlider.gameObject.SetActive(true);
        experienceSlider.gameObject.SetActive(true);
        healthSlider.gameObject.SetActive(true);

    }

    protected override void UnHighlightExtras()
    {
        rangeIndicator.gameObject.SetActive(false);
        ammunitionSlider.gameObject.SetActive(false);
        experienceSlider.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(false);
    }

    protected override void InitActives()
    {
        actives = new List<PlantData.ActiveData>(){
            new PlantData.ActiveData("Reload", 50, PlantData.Resource.Glucose),
        };
    }
    protected override void InitExtras()
    {
        rangeIndicator.Init(Color.magenta, attackRange);
    }

    protected override SpriteRenderer getRenderer()
    {
        return orangeFlowerRenderer;
    }

    public override bool CanUseActive(int index)
    {
        return Glucose_Count <= 10;
    }

    public override void UseActive(int index)
    {
        Glucose_Count = Max_Glucose_Count;
        UpdateAmmunitionSlider();
    }
}

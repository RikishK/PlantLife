using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlueFlower : Flower
{
    private int collected_experience;
    [SerializeField] private float collection_range, pollination_range;
    [SerializeField] private LayerMask experienceLayer, plantBlockLayer;

    [SerializeField] private RangeIndicator experienceCollectionRangeIndicator, pollinationRangeIndicator;

    [SerializeField] private GameObject experiencePollonPrefab, pollonPrefab, pollinationTargetIndicator;

    [SerializeField] private SpriteRenderer blueFlowerRenderer;
    [SerializeField] private Transform PollonSpawnPoint, center;
    [SerializeField] private TextMeshProUGUI collectedExpText;

    private Flower pollination_target_flower;
    private bool isSelectingPollinationTarget = false;

    private void Update() {
        if(!isSelectingPollinationTarget) return;

        if (Input.GetMouseButtonDown(0)){
            // Get the position of the mouse in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float distance = Vector2.Distance(center.position, mousePosition);
            if(distance > pollination_range) return;
            Vector3 check_pos = mousePosition;
            check_pos.z = 0;
            Collider2D[] plant_blocks = Physics2D.OverlapCircleAll(check_pos, 0.3f, plantBlockLayer);
            foreach(Collider2D plant_block in plant_blocks){
                Plant_Block plant_block_script = plant_block.GetComponent<Plant_Block>();
                if(plant_block_script.BlockType() == PlantData.BlockType.Flower){
                    Flower flower = (Flower)plant_block_script;
                    if (flower.FlowerType() != PlantData.FlowerType.Blue){
                        pollination_target_flower = flower;
                        pollinationTargetIndicator.transform.position = pollination_target_flower.transform.position;
                    }
                }
            }

            gameManager.canInteract = true;
            isSelectingPollinationTarget = false;
        }
    }
    protected override void HighlightExtras()
    {
        experienceCollectionRangeIndicator.gameObject.SetActive(true);
        pollinationRangeIndicator.gameObject.SetActive(true);
        collectedExpText.gameObject.SetActive(true);
        if(pollination_target_flower) pollinationTargetIndicator.SetActive(true);
    }

    protected override void UnHighlightExtras()
    {
        experienceCollectionRangeIndicator.gameObject.SetActive(false);
        pollinationRangeIndicator.gameObject.SetActive(false);
        collectedExpText.gameObject.SetActive(false);
        pollinationTargetIndicator.SetActive(false);
    }

    protected override void InitActives()
    {
        actives = new List<PlantData.ActiveData>(){
            new PlantData.ActiveData("Pollination Target", 20, PlantData.Resource.Glucose),
            new PlantData.ActiveData("Absorb Experience", 20, PlantData.Resource.Glucose),
            new PlantData.ActiveData("Pollon Experience", 50, PlantData.Resource.Glucose),
        };
    }

    public override List<PlantData.ActiveData> getActives()
    {
        if (collected_experience > 10) return actives;
        return actives.GetRange(0, 2);
    }
    protected override void InitExtras()
    {
        experienceCollectionRangeIndicator.Init(Color.green, collection_range);
        pollinationRangeIndicator.Init(Color.yellow, pollination_range);
    }

    protected override SpriteRenderer getRenderer()
    {
        return blueFlowerRenderer;
    }

    public override bool CanUseActive(int index)
    {
        if (index == 2) return collected_experience >= 10 && pollination_target_flower;
        return true;
    }

    public override void UseActive(int index)
    {
        switch(index){
            case 0:
                SelectPollonTarget();
                break;
            case 1:
                CollectExp();
                break;
            case 2:
                ProduceExperiencePollon();
                break;
        }
    }

    private void SelectPollonTarget(){
        gameManager.canInteract = false;
        isSelectingPollinationTarget = true;
    }

    private void CollectExp(){
        Collider2D[] experience_orbs = Physics2D.OverlapCircleAll(center.position, collection_range, experienceLayer);
        //Debug.Log(experience_orbs.Length);
        foreach(Collider2D experience_orb in experience_orbs){
            ExperienceOrb experienceOrbScript  = experience_orb.GetComponent<ExperienceOrb>();
            experienceOrbScript.Target(this);
        }
    }

    public void CollectExperienceOrb(int exp){
        collected_experience += exp;
        UpdateCollectedExpText();
    }

    private void UpdateCollectedExpText(){
        collectedExpText.text = collected_experience.ToString();
    }

    private void ProducePollon(){
        GameObject pollonObj = Instantiate(pollonPrefab);
        pollonObj.transform.position = PollonSpawnPoint.position;
    }

    private void ProduceExperiencePollon(){
        int exp_amount = 0;
        if(collected_experience > 50){
            exp_amount = 50;
            collected_experience -= 50;
        }
        else {
            exp_amount = collected_experience;
            collected_experience = 0;
        }
        GameObject experiencePollon = Instantiate(experiencePollonPrefab);
        experiencePollon.transform.position = PollonSpawnPoint.position;
        ExperiencePollon experiencePollonScript = experiencePollon.GetComponent<ExperiencePollon>();
        experiencePollonScript.Init(exp_amount, pollination_target_flower);
        UpdateCollectedExpText();
    }

}

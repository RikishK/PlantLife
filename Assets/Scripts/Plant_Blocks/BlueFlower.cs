using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlower : Flower
{
    private int collected_experience;
    [SerializeField] private float collection_range, pollination_range;
    [SerializeField] private LayerMask experienceLayer;

    [SerializeField] private RangeIndicator experienceCollectionRangeIndicator, pollinationRangeIndicator;

    [SerializeField] private GameObject experiencePollonPrefab, pollonPrefab;

    [SerializeField] private SpriteRenderer blueFlowerRenderer;
    [SerializeField] private Transform PollonSpawnPoint;

    private Flower pollination_target_flower;
    protected override void HighlightExtras()
    {
        experienceCollectionRangeIndicator.gameObject.SetActive(true);
        pollinationRangeIndicator.gameObject.SetActive(true);
    }

    protected override void UnHighlightExtras()
    {
        experienceCollectionRangeIndicator.gameObject.SetActive(false);
        pollinationRangeIndicator.gameObject.SetActive(false);
    }

    protected override void InitActives()
    {
        actives = new List<PlantData.ActiveData>(){
            new PlantData.ActiveData("Pollination Target", 20, PlantData.Resource.Glucose),
            new PlantData.ActiveData("Absorb Experience", 20, PlantData.Resource.Glucose),
            new PlantData.ActiveData("Pollon", 20, PlantData.Resource.Glucose),
            new PlantData.ActiveData("Pollon Experience", 50, PlantData.Resource.Glucose),
        };
    }

    public override List<PlantData.ActiveData> getActives()
    {
        if (collected_experience > 10) return actives;
        return actives.GetRange(0, 3);
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
        if (index == 3) return collected_experience >= 10;
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
                ProducePollon();
                break;
            case 3:
                ProduceExperiencePollon();
                break;
        }
    }

    private void SelectPollonTarget(){
        
    }

    private void CollectExp(){
        Debug.Log("Absorbing");
        Collider2D[] experience_orbs = Physics2D.OverlapCircleAll(transform.position, collection_range, experienceLayer);
        Debug.Log(experience_orbs.Length);
        foreach(Collider2D experience_orb in experience_orbs){
            Debug.Log("Collecting: " + experience_orb);
            ExperienceOrb experienceOrbScript  = experience_orb.GetComponent<ExperienceOrb>();
            experienceOrbScript.Target(this);
        }
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
        
    }

}

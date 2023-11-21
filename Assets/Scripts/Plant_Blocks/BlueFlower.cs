using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlower : Flower
{
    private int collected_experience;
    [SerializeField] private float collection_range, pollination_range;
    [SerializeField] private LayerMask experienceLayer;

    [SerializeField] private RangeIndicator experienceCollectionRangeIndicator, pollinationRangeIndicator;

    [SerializeField] private GameObject experiencePollonPrefab;

    [SerializeField] private SpriteRenderer blueFlowerRenderer;
    [SerializeField] private Transform PollonSpawnPoint;
    

    private void CollectExp(){
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, collection_range, experienceLayer);
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
        
    }

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


    protected override void InitExtras()
    {
        experienceCollectionRangeIndicator.Init(Color.green, collection_range);
        pollinationRangeIndicator.Init(Color.yellow, pollination_range);
    }

    protected override SpriteRenderer getRenderer()
    {
        return blueFlowerRenderer;
    }

}

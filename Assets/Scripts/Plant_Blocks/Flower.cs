using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Plant_Block
{
    [SerializeField] protected PlantData.FlowerType flowerType;
    [SerializeField] protected int Max_Glucose_Count, maxBattleExperience;
    [SerializeField] protected Animator flowerAnimator;
    [SerializeField] protected int Glucose_Count, flower_battle_experience = 0;

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

        Glucose_Count = 15;
    }


    public PlantData.FlowerType FlowerType(){
        return flowerType;
    }

    public void GainExperience(int experience){
        flower_battle_experience = Mathf.Clamp(flower_battle_experience + experience, 0, maxBattleExperience);
        GainExperienceExtras();
    }

    protected virtual void GainExperienceExtras(){

    }

    public int BattleExp(){
        return flower_battle_experience;
    }

    
}

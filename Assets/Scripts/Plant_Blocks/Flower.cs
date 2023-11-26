using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Plant_Block
{
    [SerializeField] protected PlantData.FlowerType flowerType;
    [SerializeField] protected int Max_Glucose_Count;
    [SerializeField] protected Animator flowerAnimator;
    protected int Glucose_Count;
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
        StartCoroutine(OrangeFlowerRoutine());
    }


    protected virtual IEnumerator OrangeFlowerRoutine(){
        yield return null;
    }

    public PlantData.FlowerType FlowerType(){
        return flowerType;
    }

    
}

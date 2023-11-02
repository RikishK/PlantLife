using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Leaf : Plant_Block
{
    [SerializeField] private GameManager gameManager;
    private PlantData.LeafState leafState = PlantData.LeafState.Small;
    private float lastGlucoseProducedTime = 0;

    private void Start() {
        lastGlucoseProducedTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastGlucoseProducedTime > 10.0f){
            lastGlucoseProducedTime = Time.time;
            ProduceGlucose();
        }
    }

    private void ProduceGlucose(){
        switch (leafState){
            case PlantData.LeafState.Small:
                gameManager.GainGlucose(3);
                break;
            case PlantData.LeafState.Medium:
                gameManager.GainGlucose(10);
                break;
            case PlantData.LeafState.Large:
                gameManager.GainGlucose(25);
                break;
        }
    }
}

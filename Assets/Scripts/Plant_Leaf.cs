using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant_Leaf : Plant_Block
{
    private PlantData.LeafState leafState = PlantData.LeafState.Small;
    private float lastGlucoseProducedTime = 0;

    [SerializeField] private Sprite SmallLeaf, MediumLeaf, LargeLeaf;
    [SerializeField] private SpriteRenderer leafRenderer;
    [SerializeField] private PlantData.LeafCollider[] leafColliders;
    [SerializeField] private BoxCollider2D leafCollider2D;

    private void Start() {
        lastGlucoseProducedTime = Time.time;
        block_name = "Leaf";
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
        Debug.Log("Leaf Is Producing");
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

    protected override void growBlock()
    {
        switch (leafState){
            case PlantData.LeafState.Small:
                leafState = PlantData.LeafState.Medium;
                break;
            case PlantData.LeafState.Medium:
                leafState = PlantData.LeafState.Large;
                break;
            case PlantData.LeafState.Large:
                break;
        }
        RenderLeaf();
        UpdateLeafCollider();
    }

    private void RenderLeaf(){
        switch (leafState){
            case PlantData.LeafState.Small:
                leafRenderer.sprite = SmallLeaf;
                break;
            case PlantData.LeafState.Medium:
                leafRenderer.sprite = MediumLeaf;
                break;
            case PlantData.LeafState.Large:
                leafRenderer.sprite = LargeLeaf;
                break;
        }
    }

    private void UpdateLeafCollider(){
        foreach(PlantData.LeafCollider leafCollider in leafColliders){
            if(leafCollider.leafState == leafState){
                leafCollider2D.size = leafCollider.plantCollider.size;
                leafCollider2D.offset = leafCollider.plantCollider.offset;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_Bud : Plant_Block
{
    private PlantData.FlowerBudState flowerBudState = PlantData.FlowerBudState.stage1;
    [SerializeField] private Sprite stage1, stage2, bloomReady;
    [SerializeField] private SpriteRenderer flowerBudRenderer;
    [SerializeField] private GameObject flowerOrange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void InitUpgrades()
    {
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Gorw", 50, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Gorw", 100, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Bloom", 200, PlantData.Resource.Nitrate)
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void growBlock()
    {
        switch (flowerBudState){
            case PlantData.FlowerBudState.stage1:
                flowerBudState = PlantData.FlowerBudState.stage2;
                break;
            case PlantData.FlowerBudState.stage2:
                flowerBudState = PlantData.FlowerBudState.bloomReady;
                break;
            case PlantData.FlowerBudState.bloomReady:
                BloomOrange();
                break;
        }
        RenderFlowerBud();
    }

    private void RenderFlowerBud(){
        switch (flowerBudState){
            case PlantData.FlowerBudState.stage1:
                flowerBudRenderer.sprite = stage1;
                break;
            case PlantData.FlowerBudState.stage2:
                flowerBudRenderer.sprite = stage2;
                break;
            case PlantData.FlowerBudState.bloomReady:
                flowerBudRenderer.sprite = bloomReady;
                break;
        }
    }

    private void BloomOrange(){
        GameObject flowerObj = Instantiate(flowerOrange);
        Plant_Block flowerScript = flowerObj.GetComponent<Plant_Block>();
        // Setup script block relations
        flowerScript.parent = parent;
        parent.children.Add(flowerScript);
        parent.children.Remove(this);
        // Setup block position and rotation
        flowerObj.transform.position = transform.position;
        flowerObj.transform.eulerAngles = transform.eulerAngles;
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (flowerBudState){
            case PlantData.FlowerBudState.stage1:
                return upgrades.GetRange(0, 1);
            case PlantData.FlowerBudState.stage2:
                return upgrades.GetRange(1, 1);
            case PlantData.FlowerBudState.bloomReady:
                return upgrades.GetRange(2, 1);
        }
        return null;
    }

    protected override SpriteRenderer getRenderer()
    {
        return flowerBudRenderer;
    }
}

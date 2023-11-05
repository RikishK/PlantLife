using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Root_Stem : Plant_Block
{
    private PlantData.RootStemState rootStemState = PlantData.RootStemState.Regular;

    [SerializeField] private Sprite RegularRootStem, ThickRootStem;
    [SerializeField] private SpriteRenderer rootStemRenderer;
    [SerializeField] private Transform extensionPoint;
    [SerializeField] private GameObject rootBranchObject;

    private void Start() {
        block_name = "Root Stem";
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Thicken", 50, PlantData.Resource.Glucose),
            new PlantData.UpgradeData("Second Shoot", 100, PlantData.Resource.Glucose)
        };
    }

    protected override void growBlock()
    {
        switch(rootStemState){
            case PlantData.RootStemState.Regular:
                rootStemState = PlantData.RootStemState.Thick;
                break;
            case PlantData.RootStemState.Thick:
                SpawnShoot();
                break;
        }
        RenderRootStem();
    }

    private void SpawnShoot(){
        GameObject new_root_branch_object = Instantiate(rootBranchObject);
        new_root_branch_object.transform.parent = extensionPoint;
        new_root_branch_object.transform.position = extensionPoint.position;
        Plant_Block new_root_branch = new_root_branch_object.GetComponent<Plant_Block>();
        new_root_branch.parent = this;
        children.Add(new_root_branch);
        new_root_branch.Init();
    }

    private void RenderRootStem(){
        switch(rootStemState){
            case PlantData.RootStemState.Regular:
                rootStemRenderer.sprite = RegularRootStem;
                break;
            case PlantData.RootStemState.Thick:
                rootStemRenderer.sprite = ThickRootStem;
                break;
        }
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch(rootStemState){
            case PlantData.RootStemState.Regular:
                return upgrades.GetRange(0, 1);
            case PlantData.RootStemState.Thick:
                return upgrades.GetRange(1, 1);
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override int upgradeCost(int index)
    {
        switch(rootStemState){
            case PlantData.RootStemState.Regular:
                return upgrades[0].cost;
            case PlantData.RootStemState.Thick:
                break;
        }
        return 0;
    }

    protected override void Highlight()
    {
        rootStemRenderer.color = hoverTint;
    }

    protected override void UnHighlight()
    {
        rootStemRenderer.color = originalColor;
    }

    public override void AttatchPlantBlock(Plant_Block other)
    {
        other.transform.position = extensionPoint.transform.position;
        other.transform.parent = extensionPoint;
    }
}

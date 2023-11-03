using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Root_Branch : Plant_Block
{
    private PlantData.RootBranchState rootBranchState = PlantData.RootBranchState.Small;
    [SerializeField] private Sprite SmallRoot, MediumRoot, LargeRoot;
    [SerializeField] private SpriteRenderer rootBranchRenderer;

    [SerializeField] private PlantData.RootBranchCollider[] rootBranchColliders;
    [SerializeField] private BoxCollider2D rootBranchCollider2D;

    private void Start() {
        block_name = "Root Branch";
        upgrades = new List<PlantData.UpgradeData>();
        PlantData.UpgradeData upgrade1 = new PlantData.UpgradeData("Grow", 10, PlantData.Resource.Glucose);
        upgrades.Add(upgrade1);
        
        PlantData.UpgradeData upgrade2 = new PlantData.UpgradeData("Grow", 20, PlantData.Resource.Glucose);
        upgrades.Add(upgrade2);
        
        PlantData.UpgradeData upgrade3 = new PlantData.UpgradeData("Grow", 30, PlantData.Resource.Glucose);
        upgrades.Add(upgrade3);
                
    }
    protected override void growBlock()
    {
        switch (rootBranchState){
            case PlantData.RootBranchState.Small:
                rootBranchState = PlantData.RootBranchState.Medium;
                break;
            case PlantData.RootBranchState.Medium:
                rootBranchState = PlantData.RootBranchState.Large;
                break;
            case PlantData.RootBranchState.Large:
                break;
        }
        RenderRootBranch();
        UpdateRootBranchCollider();
    }

    private void RenderRootBranch(){
        Debug.Log(rootBranchRenderer);
        switch (rootBranchState){
            case PlantData.RootBranchState.Small:
                rootBranchRenderer.sprite = SmallRoot;
                break;
            case PlantData.RootBranchState.Medium:
                rootBranchRenderer.sprite = MediumRoot;
                break;
            case PlantData.RootBranchState.Large:
                rootBranchRenderer.sprite = LargeRoot;
                break;
        }
    }

    private void UpdateRootBranchCollider(){
        foreach(PlantData.RootBranchCollider rootBranchCollider in rootBranchColliders){
            if(rootBranchCollider.rootBranchState == rootBranchState){
                rootBranchCollider2D.size = rootBranchCollider.plantCollider.size;
                rootBranchCollider2D.offset = rootBranchCollider.plantCollider.offset;
            }
        }
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (rootBranchState){
            case PlantData.RootBranchState.Small:
                return upgrades.GetRange(0, 1);
            case PlantData.RootBranchState.Medium:
                return upgrades.GetRange(1, 1);
            case PlantData.RootBranchState.Large:
                return upgrades.GetRange(2, 1);
        }
        Debug.Log("Default Upgrades");
        return new List<PlantData.UpgradeData>();
    }

    protected override int upgradeCost()
    {
        switch (rootBranchState){
            case PlantData.RootBranchState.Small:
                return upgrades[0].cost;
            case PlantData.RootBranchState.Medium:
                return upgrades[1].cost;
            case PlantData.RootBranchState.Large:
                return upgrades[2].cost;
        }
        return 0;
    }
}

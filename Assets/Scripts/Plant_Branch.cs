using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Branch : Plant_Block
{
    private PlantData.BranchState branchState = PlantData.BranchState.Small_Nub;
    [SerializeField] private Sprite SmallNubBranch, GrowingBranchA, GrowingBranchB, GrownNubBranch;
    [SerializeField] private SpriteRenderer branchRenderer;

    private void Start() {
        block_name = "Branch";
        upgrades = new List<PlantData.UpgradeData>();
        PlantData.UpgradeData upgrade1 = new PlantData.UpgradeData("Begin Growing Leaves", 20, PlantData.Resource.Glucose);
        upgrades.Add(upgrade1);

        PlantData.UpgradeData upgrade2 = new PlantData.UpgradeData("Grow Leaves", 20, PlantData.Resource.Glucose);
        upgrades.Add(upgrade2);
        
        PlantData.UpgradeData upgrade3 = new PlantData.UpgradeData("Finish Growing Leaves", 20, PlantData.Resource.Glucose);
        upgrades.Add(upgrade3);
               
    }

    protected override void growBlock()
    {
        switch(branchState){
            case PlantData.BranchState.Small_Nub:
                branchState = PlantData.BranchState.Growing_Leaf_Attatchments_A;
                break;
            case PlantData.BranchState.Growing_Leaf_Attatchments_A:
                branchState = PlantData.BranchState.Growing_Leaf_Attatchments_B;
                break;
            case PlantData.BranchState.Growing_Leaf_Attatchments_B:
                branchState = PlantData.BranchState.Grown_Nub;
                break;
            case PlantData.BranchState.Grown_Nub:
                break;
        }
        RenderBranch();
    }

    private void RenderBranch(){
        switch (branchState){
            case PlantData.BranchState.Small_Nub:
                branchRenderer.sprite = SmallNubBranch;
                break;
            case PlantData.BranchState.Growing_Leaf_Attatchments_A:
                branchRenderer.sprite = GrowingBranchA;
                break;
            case PlantData.BranchState.Growing_Leaf_Attatchments_B:
                branchRenderer.sprite = GrowingBranchB;
                break;
            case PlantData.BranchState.Grown_Nub:
                branchRenderer.sprite = GrownNubBranch;
                break;
        }
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (branchState){
            case PlantData.BranchState.Small_Nub:
                return upgrades.GetRange(0, 1);
            case PlantData.BranchState.Growing_Leaf_Attatchments_A:
                return upgrades.GetRange(1, 1);
            case PlantData.BranchState.Growing_Leaf_Attatchments_B:
                return upgrades.GetRange(2, 1);
            case PlantData.BranchState.Grown_Nub:
                break;
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override int upgradeCost()
    {
        switch (branchState){
            case PlantData.BranchState.Small_Nub:
                return upgrades[0].cost;
            case PlantData.BranchState.Growing_Leaf_Attatchments_A:
                return upgrades[1].cost;
            case PlantData.BranchState.Growing_Leaf_Attatchments_B:
                return upgrades[2].cost;
            case PlantData.BranchState.Grown_Nub:
                break;
        }
        return 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Branch : Plant_Block
{
    private PlantData.BranchState branchState = PlantData.BranchState.Small_Nub;
    [SerializeField] private Sprite SmallNubBranch, GrowingBranchA, GrowingBranchB, GrownNubBranch;
    [SerializeField] private SpriteRenderer branchRenderer;

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

    protected override List<PlantData.UpgradeData> getUpgrades()
    {
        List<PlantData.UpgradeData> upgrades = new List<PlantData.UpgradeData>();
        switch (branchState){
            case PlantData.BranchState.Small_Nub:
                PlantData.UpgradeData upgrade1 = new PlantData.UpgradeData("Begin Growing Leaves", 20, PlantData.Resource.Glucose);
                upgrades.Add(upgrade1);
                break;
            case PlantData.BranchState.Growing_Leaf_Attatchments_A:
                PlantData.UpgradeData upgrade2 = new PlantData.UpgradeData("Grow Leaves", 20, PlantData.Resource.Glucose);
                upgrades.Add(upgrade2);
                break;
            case PlantData.BranchState.Growing_Leaf_Attatchments_B:
                PlantData.UpgradeData upgrade3 = new PlantData.UpgradeData("Finish Growing Leaves", 20, PlantData.Resource.Glucose);
                upgrades.Add(upgrade3);
                break;
            case PlantData.BranchState.Grown_Nub:
                break;
        }
        return upgrades;
    }


}

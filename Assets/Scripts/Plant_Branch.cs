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

    
}

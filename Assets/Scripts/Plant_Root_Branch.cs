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
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;


public class Plant_Stem : Plant_Block
{ 
    private PlantData.StemState stemState = PlantData.StemState.Green;
    [SerializeField] private Sprite GreenStem, MidStem, BrownStem, ThickBrownStem;
    [SerializeField] private SpriteRenderer stemRenderer;
    [SerializeField] private PlantData.StemCollider[] stemColliders;
    [SerializeField] private BoxCollider2D stemCollider2D;

    [SerializeField] private Transform extensionPoint, stemShootExtensionPoint;

    [SerializeField] private GameObject stemShoot;

    private bool hasShoot = false;

    private void Start() {
        block_name = "Stem";
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Thicken", 100, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Thicken", 200, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Thicken", 400, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Grow Shoot", 600, PlantData.Resource.Nitrate),
        };
    }
    protected override void growBlock()
    {
        switch(stemState){
            case PlantData.StemState.Green:
                stemState = PlantData.StemState.Mid;
                break;
            case PlantData.StemState.Mid:
                stemState = PlantData.StemState.Brown;
                break;
            case PlantData.StemState.Brown:
                stemState = PlantData.StemState.Thick_Brown;
                break;
            case PlantData.StemState.Thick_Brown:
                break;
        }
        RenderStem();
        UpdateStemCollider();
    }

    private void RenderStem(){
        switch (stemState){
            case PlantData.StemState.Green:
                stemRenderer.sprite = GreenStem;
                break;
            case PlantData.StemState.Mid:
                stemRenderer.sprite = MidStem;
                break;
            case PlantData.StemState.Brown:
                stemRenderer.sprite = BrownStem;
                break;
            case PlantData.StemState.Thick_Brown:
                stemRenderer.sprite = ThickBrownStem;
                break;
        }
    }

    private void UpdateStemCollider(){
        foreach(PlantData.StemCollider stemCollider in stemColliders){
            if(stemCollider.stemState == stemState){
                stemCollider2D.size = stemCollider.plantCollider.size;
            }
        }
    }

    public override void AttatchPlantBlock(Plant_Block other)
    {
        other.transform.position = extensionPoint.transform.position;
        other.transform.eulerAngles = transform.eulerAngles;
        other.transform.parent = extensionPoint;
        children.Add(other);
    }

    protected override SpriteRenderer getRenderer()
    {
        return stemRenderer;
    }

    protected override void performUpgrade(int index)
    {
        switch (stemState){
            case PlantData.StemState.Green:
                growBlock();
                if(parent.BlockType() == PlantData.BlockType.Stem){
                    Plant_Stem parent_stem = (Plant_Stem)parent;
                    parent_stem.SetStem(PlantData.StemState.Brown);
                }
                break;
            case PlantData.StemState.Mid:
                growBlock();
                break;
            case PlantData.StemState.Brown:
                growBlock();
                break;
            case PlantData.StemState.Thick_Brown:
                growShoot();
                break;
        }
    }

    protected void growShoot(){
        GameObject stemShootObj = Instantiate(stemShoot);
        stemShootObj.transform.position = stemShootExtensionPoint.position;
        Plant_Block stemShootBlock = stemShootObj.GetComponent<Plant_Block>();
        stemShootObj.transform.parent = stemShootExtensionPoint;
        children.Add(stemShootBlock);
        stemShootBlock.parent = this;
        stemShootBlock.Init();
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (stemState){
            case PlantData.StemState.Green:
                return upgrades.GetRange(0, 1);
            case PlantData.StemState.Brown:
                return upgrades.GetRange(2, 1);
            case PlantData.StemState.Thick_Brown:
                return upgrades.GetRange(3, 1);
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override bool upgradeConditions(int index)
    {
        switch (stemState){
            case PlantData.StemState.Green:
                return CheckParentCondition();
            case PlantData.StemState.Mid:
                return false;
            case PlantData.StemState.Brown:
                return CheckParentCondition();
            case PlantData.StemState.Thick_Brown:
                return !hasShoot;
        }
        return false;
    }

    private bool CheckParentCondition(){
        if (parent.BlockType() == PlantData.BlockType.Stem){
            Plant_Stem parentStemBlock = (Plant_Stem)parent;
            switch (stemState){
                case PlantData.StemState.Green:
                    return parentStemBlock.StemState() == PlantData.StemState.Mid; 
                case PlantData.StemState.Brown:
                    return parentStemBlock.StemState() == PlantData.StemState.Thick_Brown; 
            }
        }
        else if(parent.BlockType() == PlantData.BlockType.Core){
            Plant_Core parentStemBlock = (Plant_Core)parent;
            switch (stemState){
                case PlantData.StemState.Green:
                    return parentStemBlock.CoreState() == PlantData.CoreState.Level2 || parentStemBlock.CoreState() == PlantData.CoreState.Level3; 
                case PlantData.StemState.Brown:
                    return parentStemBlock.CoreState() == PlantData.CoreState.Level3; 
            }
        }
        return false;
    }

    public PlantData.StemState StemState(){
        return stemState;
    }

    public void SetStem(PlantData.StemState state){
        stemState = state;
        RenderStem();
        UpdateStemCollider();
    }
}

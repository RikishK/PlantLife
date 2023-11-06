using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class Plant_Root_Stem : Plant_Block
{
    private PlantData.RootStemState rootStemState = PlantData.RootStemState.Regular;

    [SerializeField] private Sprite RegularRootStem, ThickRootStem;
    [SerializeField] private SpriteRenderer rootStemRenderer;
    [SerializeField] private Transform extensionPoint;
    [SerializeField] private GameObject rootBranchObject, bacteriaHub;

    private void Start() {
        block_name = "Root Stem";
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Thicken", 50, PlantData.Resource.Glucose),
            new PlantData.UpgradeData("Second Shoot", 100, PlantData.Resource.Glucose),
            new PlantData.UpgradeData("Bacteria Hub", 300, PlantData.Resource.Glucose)
        };
    }

    protected override void performUpgrade(int index)
    {
        switch(rootStemState){
            case PlantData.RootStemState.Regular:
                growBlock();
                break;
            case PlantData.RootStemState.Thick:
                if(index == 0) growBlock();
                else if(index == 1) transformBacteriaHub();
                break;
        }
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

    private void transformBacteriaHub(){
        GameObject hub = Instantiate(bacteriaHub);
        hub.transform.position = transform.position;
        hub.transform.eulerAngles = transform.eulerAngles;
        
        Plant_Block hub_block = hub.GetComponent<Plant_Block>();
        hub_block.parent = parent;
        hub_block.children = children;
        parent.children.Remove(this);
        parent.children.Add(hub_block);
        foreach(Plant_Block child in children){
            child.parent = hub_block;
        }

        int childCount = extensionPoint.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = extensionPoint.GetChild(0); // Get the first child of the source
            Plant_Bacteria_Hub bac_hub = (Plant_Bacteria_Hub)hub_block;
            child.SetParent(bac_hub.extensionPoint);
        }

        Destroy(gameObject);
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
                return upgrades.GetRange(1, 2);
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override int upgradeCost(int index)
    {
        switch(rootStemState){
            case PlantData.RootStemState.Regular:
                return upgrades[0].cost;
            case PlantData.RootStemState.Thick:
                return upgrades[index + 1].cost;
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

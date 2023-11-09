using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Core : Plant_Block
{
    [SerializeField] private GameObject overground_root, underground_root;
    [SerializeField] private SpriteRenderer coreRenderer;

    [SerializeField] private GameObject stemPrefab;
    [SerializeField] private Transform overground_sp;
    
    private PlantData.CoreState coreState = PlantData.CoreState.Level1;
    // Start is called before the first frame update
    void Start()
    {
        upgrades = new List<PlantData.UpgradeData>{
            new PlantData.UpgradeData("Grow Stem", 100, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Thicken Base", 500, PlantData.Resource.Glucose),
            new PlantData.UpgradeData("Thicken Base", 1000, PlantData.Resource.Glucose)
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Highlight(){
        coreRenderer.color = Color.red;
    }

    protected override void UnHighlight(){
        coreRenderer.color = Color.white;
    }

    public override List<PlantData.UpgradeData> getUpgrades(){
        List<PlantData.UpgradeData> available_upgrades = new List<PlantData.UpgradeData>();
        switch(coreState){
            case PlantData.CoreState.Level1:
                available_upgrades.Add(upgrades[0]);
                available_upgrades.Add(upgrades[1]);
                return available_upgrades;
            case PlantData.CoreState.Level2:
                available_upgrades.Add(upgrades[0]);
                available_upgrades.Add(upgrades[2]);
                return available_upgrades;
            case PlantData.CoreState.Level3:
                available_upgrades.Add(upgrades[0]);
                return available_upgrades;
        }
        return available_upgrades;
    }

    protected override void performUpgrade(int index)
    {
        switch(coreState){
            case PlantData.CoreState.Level1:
                if (index == 0) GrowStem();
                if (index == 1) LevelUp();
                break;
            case PlantData.CoreState.Level2:
                if (index == 0) GrowStem();
                if (index == 1) LevelUp();
                break;
            case PlantData.CoreState.Level3:
                GrowStem();
                break;
        }
    }

    private void GrowStem(){
        Debug.Log("Growing Stem");
        GameObject new_stem_obj = Instantiate(stemPrefab, gameObject.transform);
        new_stem_obj.transform.position = overground_sp.position;
        overground_root.transform.parent = new_stem_obj.transform;
        
        Plant_Block new_stem_block = new_stem_obj.GetComponentInChildren<Plant_Block>();
        new_stem_block.Init();
        
        Plant_Stem new_plant_stem_block = (Plant_Stem)new_stem_block;
        switch(coreState){
            case PlantData.CoreState.Level2:
                new_plant_stem_block.SetStem(PlantData.StemState.Brown);
                break;
            case PlantData.CoreState.Level3:
                new_plant_stem_block.SetStem(PlantData.StemState.Thick_Brown);
                break;
        }

        Plant_Block overground_root_block = overground_root.GetComponentInChildren<Plant_Block>();
        new_stem_block.children.Add(overground_root_block);
        new_stem_block.AttatchPlantBlock(overground_root_block);
        overground_root_block.parent = new_stem_block;
        new_stem_block.parent = this;
        overground_root = new_stem_obj;

    }

    protected override bool upgradeConditions(int index)
    {
        Debug.Log("Core: " + index);
        if (index != 0) return true;
        Plant_Stem base_stem = overground_root.GetComponent<Plant_Stem>();
        switch (coreState){
            case PlantData.CoreState.Level1:
                return true;
            case PlantData.CoreState.Level2:
                return base_stem.StemState() == PlantData.StemState.Mid;
            case PlantData.CoreState.Level3:
                return base_stem.StemState() == PlantData.StemState.Brown || base_stem.StemState() == PlantData.StemState.Thick_Brown;
        }
        return false;
    }

    private void LevelUp(){
        Debug.Log("Core leveled up");
        switch(coreState){
            case PlantData.CoreState.Level1:
                coreState = PlantData.CoreState.Level2;
                break;
            case PlantData.CoreState.Level2:
                coreState = PlantData.CoreState.Level3;
                break;
        }
    }

    public PlantData.CoreState CoreState(){
        return coreState;
    }

}

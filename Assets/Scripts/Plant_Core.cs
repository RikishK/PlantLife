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
            new PlantData.UpgradeData("Grow Stem", 10, PlantData.Resource.Glucose),
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
        GameObject new_overground_root = Instantiate(stemPrefab, gameObject.transform);
        new_overground_root.transform.position = overground_sp.position;
        Plant_Block new_root = new_overground_root.GetComponentInChildren<Plant_Block>();
        new_root.children = new List<Plant_Block>();
        Plant_Block overground_root_block = overground_root.GetComponentInChildren<Plant_Block>();
        new_root.children.Add(overground_root_block);
        new_root.AttatchPlantBlock(overground_root_block);
        overground_root_block.parent = new_root;
        overground_root.transform.parent = new_root.transform;
        overground_root = new_overground_root;
    }

    private void LevelUp(){
        Debug.Log("Core leveled up");
    }

}

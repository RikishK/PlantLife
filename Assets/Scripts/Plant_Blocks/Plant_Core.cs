using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Plant_Core : Plant_Block
{
    [SerializeField] private GameObject overground_root, underground_root;
    [SerializeField] private SpriteRenderer coreRenderer;

    [SerializeField] private GameObject stemPrefab;
    [SerializeField] private Transform overground_sp;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private EnemySpawner enemySpawner;
    
    private PlantData.CoreState coreState = PlantData.CoreState.Level1;
    // Start is called before the first frame update
    void Start()
    {
        InitUpgrades();
        InitActives();
        InitExtras();
    }

    protected override void InitExtras()
    {
        current_health = health;
        UpdateHealthSlider();
    }

    protected override void InitUpgrades()
    {
        upgrades = new List<PlantData.UpgradeData>{
            new PlantData.UpgradeData("Grow Stem", 100, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Level 2", 500, PlantData.Resource.Glucose),
            new PlantData.UpgradeData("Level 3", 1000, PlantData.Resource.Glucose)
        };
    }

    protected override void InitActives()
    {
        actives = new List<PlantData.ActiveData>(){
            new PlantData.ActiveData("Heal", 100, PlantData.Resource.Glucose),
        };
    }

    public override bool CanUseActive(int index)
    {
        return current_health < health;
    }

    public override void UseActive(int index)
    {
        current_health = Mathf.Clamp(current_health + 50, 0, health);
        UpdateHealthSlider();
    }

    protected override void Highlight(){
        coreRenderer.color = Color.red;
    }

    protected override void UnHighlight(){
        coreRenderer.color = Color.white;
    }

    protected override void DestroyBlock()
    {
        enemySpawner.LoseGame();
        Destroy(gameObject);
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
        GameObject new_stem_obj = Instantiate(stemPrefab, gameObject.transform);
        new_stem_obj.transform.position = overground_sp.position;
        overground_root.transform.parent = new_stem_obj.transform;
        
        Plant_Block new_stem_block = new_stem_obj.GetComponentInChildren<Plant_Block>();
        new_stem_block.Init();
        
        Plant_Stem new_plant_stem_block = (Plant_Stem)new_stem_block;
        switch(coreState){
            case PlantData.CoreState.Level2:
                new_plant_stem_block.SetStem(getGrowStemState());
                break;
            case PlantData.CoreState.Level3:
                new_plant_stem_block.SetStem(getGrowStemState());
                break;
        }

        Plant_Block overground_root_block = overground_root.GetComponentInChildren<Plant_Block>();
        new_stem_block.AttatchPlantBlock(overground_root_block);
        overground_root_block.parent = new_stem_block;
        new_stem_block.parent = this;
        overground_root = new_stem_obj;

    }

    private PlantData.StemState getGrowStemState(){
        switch(coreState){
            case PlantData.CoreState.Level2:
                if(overground_root.GetComponent<Plant_Block>().BlockType() == PlantData.BlockType.Branch) return PlantData.StemState.Green;
                if(overground_root.GetComponent<Plant_Block>().BlockType() == PlantData.BlockType.Stem){
                    switch (overground_root.GetComponent<Plant_Stem>().StemState()){
                        case PlantData.StemState.Green:
                            return PlantData.StemState.Mid;
                        case PlantData.StemState.Mid:
                            return PlantData.StemState.BrownStem;
                        case PlantData.StemState.BrownStem:
                            return PlantData.StemState.BrownStem;

                    }
                }
                break;
            case PlantData.CoreState.Level3:
                if(overground_root.GetComponent<Plant_Block>().BlockType() == PlantData.BlockType.Branch) return PlantData.StemState.Green;
                if(overground_root.GetComponent<Plant_Block>().BlockType() == PlantData.BlockType.Stem){
                    switch (overground_root.GetComponent<Plant_Stem>().StemState()){
                        case PlantData.StemState.Green:
                            return PlantData.StemState.Mid;
                        case PlantData.StemState.Mid:
                            return PlantData.StemState.BrownStem;
                        case PlantData.StemState.BrownStem:
                            return PlantData.StemState.BrownLink;
                        case PlantData.StemState.BrownLink:
                            return PlantData.StemState.Thick_Brown;
                        case PlantData.StemState.Thick_Brown:
                            return PlantData.StemState.Thick_Brown;

                    }
                }
                break;
        }
        return PlantData.StemState.Green;
    }

    protected override bool upgradeConditions(int index)
    {
        // if (index != 0) return true;
        // if(overground_root.GetComponent<Plant_Block>().BlockType() == PlantData.BlockType.Branch){
            
        //     return true;
        // }
        // Plant_Stem base_stem = overground_root.GetComponent<Plant_Stem>();
        // switch (coreState){
        //     case PlantData.CoreState.Level1:
        //         return true;
        //     case PlantData.CoreState.Level2:
        //         return base_stem.StemState() == PlantData.StemState.Mid || base_stem.StemState() == PlantData.StemState.BrownStem;
        //     case PlantData.CoreState.Level3:
        //         return base_stem.StemState() == PlantData.StemState.BrownLink || base_stem.StemState() == PlantData.StemState.Thick_Brown;
        // }
        // return false;
        return true;
    }

    private void LevelUp(){
        Debug.Log("Core leveled up");
        switch(coreState){
            case PlantData.CoreState.Level1:
                coreState = PlantData.CoreState.Level2;
                health = 1000;
                current_health = Mathf.Clamp(current_health + 200, 0, health);
                UpdateHealthSlider();
                break;
            case PlantData.CoreState.Level2:
                coreState = PlantData.CoreState.Level3;
                health = 2000;
                current_health = Mathf.Clamp(current_health + 500, 0, health);
                UpdateHealthSlider();
                break;
        }
    }

    public PlantData.CoreState CoreState(){
        return coreState;
    }

    public int CountBlock(PlantData.BlockType blockType){
        Plant_Block start_block = getSearchRoot(blockType);
        int block_count = CountBlockInChildren(blockType, start_block);
        return block_count;
    }

    private int CountBlockInChildren(PlantData.BlockType blockType, Plant_Block parent){
        int count = 0;
        if (parent.BlockType() == blockType) count++;
        if (parent.children.Count == 0) return count;
        foreach(Plant_Block childBlock in parent.children){
            count += CountBlockInChildren(blockType, childBlock);
        }
        return count;
    }

    public List<Plant_Block> GetBlocksOfType(PlantData.BlockType targetBlockType){
        List<Plant_Block> targetBlocks = new List<Plant_Block>();
        Plant_Block start_block = getSearchRoot(targetBlockType);
        Queue<Plant_Block> search_queue = new Queue<Plant_Block>();
        search_queue.Enqueue(start_block);
        while(search_queue.Count > 0){
            Plant_Block current_block = search_queue.Dequeue();
            if (current_block.BlockType() == targetBlockType) targetBlocks.Add(current_block);
            foreach(Plant_Block child_block in current_block.children){
                search_queue.Enqueue(child_block);
            }
        }
        return targetBlocks;
    }
    
    private Plant_Block getSearchRoot(PlantData.BlockType blockType){
        switch (blockType){
            case PlantData.BlockType.Stem:
                return overground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Stem_Shoot:
                return overground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Branch:
                return overground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Leaf:
                return overground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Root_Stem:
                return underground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Root_Branch:
                return underground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Bacteria_Hub:
                return underground_root.GetComponent<Plant_Block>();
            case PlantData.BlockType.Nitrate_Intake:
                return underground_root.GetComponent<Plant_Block>();
        }
        return null;
    }

    private void UpdateHealthSlider(){
        healthSlider.maxValue = health;
        healthSlider.value = current_health;
    }

    protected override void TakeDamageExtras()
    {
        UpdateHealthSlider();
    }
}

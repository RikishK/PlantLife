using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Stem_Shoot : Plant_Block
{
    [SerializeField] private SpriteRenderer stemShootRenderer;
    [SerializeField] private Sprite babyShoot, midShoot, fullShoot;

    [SerializeField] private GameObject branch, stem;
    [SerializeField] private Transform extensionPoint;
    private PlantData.StemShootState stemShootState;
    private bool isPlacing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlacing) return;

        // Get the position of the mouse in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the object to the mouse
        Vector3 direction = mousePosition - transform.position;

        // Calculate the angle in radians
        float angle = Mathf.Atan2(direction.y, direction.x);

        // Convert the angle to degrees and rotate the object
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90f);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            isPlacing = false;
            stemShootRenderer.color = originalColor;
            gameManager.canInteract = true;
            SummonInitialBranch();
        }
    }

    public override void Init()
    {
        Debug.Log("Shoot Init");
        base.Init();
        // Lock rotation to mouse and allow player to place
        stemShootRenderer.color = Color.green;
        stemShootState = PlantData.StemShootState.Baby;
        gameManager.canInteract = false;
        isPlacing = true;
        // Summon branch and attatch
    }

    protected override void InitUpgrades()
    {
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Grow Stem", 100, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Thicken Shoot", 200, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Grow Stem", 200, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Thicken Shoot", 400, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Grow Stem", 400, PlantData.Resource.Nitrate),
        };
    }

    private void SummonInitialBranch(){
        GameObject branchObj = Instantiate(branch);
        branchObj.transform.position = extensionPoint.position;
        branchObj.transform.parent = extensionPoint;
        branchObj.transform.eulerAngles = transform.eulerAngles;

        Plant_Block branchBlock = branchObj.GetComponent<Plant_Block>();
        children.Add(branchBlock);
        branchBlock.Init();
    }

    protected override SpriteRenderer getRenderer()
    {
        return stemShootRenderer;
    }

    protected override void growBlock()
    {
        Plant_Stem child_stem = (Plant_Stem)children[0];
        switch (stemShootState){
            case PlantData.StemShootState.Baby:
                stemShootState = PlantData.StemShootState.Mid;
                child_stem.SetStem(PlantData.StemState.Mid);
                break;
            case PlantData.StemShootState.Mid:
                stemShootState = PlantData.StemShootState.Full;
                child_stem.SetStem(PlantData.StemState.BrownStem);
                Plant_Stem grandchild_stem = (Plant_Stem)child_stem.children[0];
                grandchild_stem.SetStem(PlantData.StemState.Mid);
                break;
        }
        RenderShoot();
    }

    protected override void performUpgrade(int index)
    {
        switch (stemShootState){
            case PlantData.StemShootState.Baby:
                if (index == 0) growStem();
                if (index == 1) growBlock();
                break;
            case PlantData.StemShootState.Mid:
                if (index == 0) growStem();
                if (index == 1) growBlock();
                break;
            case PlantData.StemShootState.Full:
                if (index == 0) growStem();
                break;
        }
    }

    private void growStem(){
        GameObject newStemObj = Instantiate(stem);
        newStemObj.transform.position = extensionPoint.position;
        newStemObj.transform.eulerAngles = transform.eulerAngles;
        newStemObj.transform.parent = extensionPoint;

        Plant_Block newStemBlock = newStemObj.GetComponent<Plant_Block>();
        newStemBlock.Init();
        newStemBlock.AttatchPlantBlock(children[0]);
        children[0] = newStemBlock;
        newStemBlock.parent = this;


        Plant_Stem newStem = (Plant_Stem)newStemBlock;
        switch (stemShootState){
            case PlantData.StemShootState.Mid:
                newStem.SetStem(PlantData.StemState.Mid);
                break;
            case PlantData.StemShootState.Full:
                newStem.SetStem(PlantData.StemState.BrownLink);
                break;
        }
    }
    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (stemShootState){
            case PlantData.StemShootState.Baby:
                return upgrades.GetRange(0, 2);
            case PlantData.StemShootState.Mid:
                return upgrades.GetRange(2, 2);
            case PlantData.StemShootState.Full:
                return upgrades.GetRange(4, 1);
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override bool upgradeConditions(int index)
    {
        switch (stemShootState){
            case PlantData.StemShootState.Baby:
                if(index == 0) return true;
                else if(index == 1) return children[0].BlockType() == PlantData.BlockType.Stem;
                break;
            case PlantData.StemShootState.Mid:
                if(index == 0) return true;
                else if(index == 1){
                    if (children[0].BlockType() == PlantData.BlockType.Stem){
                        Plant_Stem child_stem = (Plant_Stem)children[0];
                        Debug.Log(child_stem.children[0]);
                        return child_stem.StemState() == PlantData.StemState.Mid && child_stem.children[0].BlockType() == PlantData.BlockType.Stem;
                    }
                }
                break;
            case PlantData.StemShootState.Full:
                if(index == 0) return true;
                break;
        }
        return false;
    }

    private void RenderShoot(){
        switch (stemShootState){
            case PlantData.StemShootState.Baby:
                stemShootRenderer.sprite = babyShoot;
                break;
            case PlantData.StemShootState.Mid:
                stemShootRenderer.sprite = midShoot;
                break;
            case PlantData.StemShootState.Full:
                stemShootRenderer.sprite = fullShoot;
                break;
        }
    }
}

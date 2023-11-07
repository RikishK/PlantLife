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
    [SerializeField] private GameObject RootStemPrefab;
    private bool isPlacing = false;

    private void Start() {
        block_name = "Root Branch";
        upgrades = new List<PlantData.UpgradeData>();
        PlantData.UpgradeData upgrade1 = new PlantData.UpgradeData("Grow", 10, PlantData.Resource.Glucose);
        upgrades.Add(upgrade1);
        
        PlantData.UpgradeData upgrade2 = new PlantData.UpgradeData("Grow", 20, PlantData.Resource.Glucose);
        upgrades.Add(upgrade2);
        
        PlantData.UpgradeData upgrade3 = new PlantData.UpgradeData("Grow", 30, PlantData.Resource.Glucose);
        upgrades.Add(upgrade3);
                
    }

    public override void Init()
    {
        base.Init();
        isPlacing = true;
        rootBranchRenderer.color = Color.green;
        gameManager.canInteract = false;
    }

    private void Update() {
        if(!isPlacing) return;

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
            rootBranchRenderer.color = originalColor;
            gameManager.canInteract = true;
        }
    }

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
                SpawnNewRoot();
                break;
        }
        RenderRootBranch();
        UpdateRootBranchCollider();
    }

    private void SpawnNewRoot(){
        // Make root stem
        GameObject root_stem_object = Instantiate(RootStemPrefab);
        // Set root stem transform parent
        root_stem_object.transform.parent = transform.parent;
        // Set root stem pos and rotation to my pos and rotation
        root_stem_object.transform.position = transform.position;
        root_stem_object.transform.eulerAngles = transform.eulerAngles;
        // Set the root_stem parent block to my parent, set root_stem as parents child
        Plant_Block root_stem = root_stem_object.GetComponentInChildren<Plant_Block>();
        root_stem.Init();
        root_stem.parent = parent;
        parent.children.Remove(this);
        parent.children.Add(root_stem);
        // Change my parent to the root_stem and my transform parent to root_stem
        parent = root_stem;
        // Attatch to new root_stem
        root_stem.AttatchPlantBlock(this);
        root_stem.children.Add(this);
        rootBranchState = PlantData.RootBranchState.Small;
        isPlacing = true;
        rootBranchRenderer.color = Color.green;
        gameManager.canInteract = false;
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
                rootBranchCollider2D.offset = rootBranchCollider.plantCollider.offset;
            }
        }
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (rootBranchState){
            case PlantData.RootBranchState.Small:
                return upgrades.GetRange(0, 1);
            case PlantData.RootBranchState.Medium:
                return upgrades.GetRange(1, 1);
            case PlantData.RootBranchState.Large:
                return upgrades.GetRange(2, 1);
        }
        
        return new List<PlantData.UpgradeData>();
    }

    protected override void Highlight()
    {
        rootBranchRenderer.color = hoverTint;
    }

    protected override void UnHighlight()
    {
        rootBranchRenderer.color = originalColor;
    }
}

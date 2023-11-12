using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Branch : Plant_Block
{
    private PlantData.BranchState branchState = PlantData.BranchState.Small_Nub;
    [SerializeField] private Sprite SmallNubBranch, GrowingBranchA, GrowingBranchB, GrownNubBranch;
    [SerializeField] private SpriteRenderer branchRenderer;
    [SerializeField] private Transform leaf_spawn_point_A, leaf_spawn_point_B;
    [SerializeField] private GameObject LeafPrefab;

    private void Start() {
        block_name = "Branch";
        upgrades = new List<PlantData.UpgradeData>(){
            new PlantData.UpgradeData("Begin Growing Leaves", 20, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Grow Leaves", 20, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Finish Growing Leaves", 20, PlantData.Resource.Nitrate)
        };
               
    }

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
                SpawnLeaves();
                break;
            case PlantData.BranchState.Grown_Nub:
                break;
        }
        RenderBranch();
    }

    private void SpawnLeaves(){
        Vector3 baseRotation = gameObject.transform.eulerAngles;
        Debug.Log(baseRotation);
        GameObject L_A = GameObject.Instantiate(LeafPrefab, leaf_spawn_point_A);
        L_A.transform.position = leaf_spawn_point_A.position;
        Vector3 rotationA = baseRotation;
        rotationA.z += 45;
        L_A.transform.eulerAngles = rotationA;
        GameObject L_B = GameObject.Instantiate(LeafPrefab, leaf_spawn_point_B);
        L_B.transform.position = leaf_spawn_point_B.position;
        Vector3 rotationB = baseRotation;
        rotationB.z += -45;
        L_B.transform.eulerAngles = rotationB;
        children = new List<Plant_Block>
        {
            L_A.GetComponent<Plant_Block>(),
            L_B.GetComponent<Plant_Block>()
        };
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

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (branchState){
            case PlantData.BranchState.Small_Nub:
                return upgrades.GetRange(0, 1);
            case PlantData.BranchState.Growing_Leaf_Attatchments_A:
                return upgrades.GetRange(1, 1);
            case PlantData.BranchState.Growing_Leaf_Attatchments_B:
                return upgrades.GetRange(2, 1);
            case PlantData.BranchState.Grown_Nub:
                break;
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override void Highlight()
    {
        branchRenderer.color = hoverTint;
    }

    protected override void UnHighlight()
    {
        branchRenderer.color = originalColor;
    }

}

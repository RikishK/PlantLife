using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Plant_Block : MonoBehaviour
{
    public GameManager gameManager;
    public string block_name = "Plant Block";
    public Plant_Block parent;
    public List<Plant_Block> children;
    protected List<PlantData.UpgradeData> upgrades;
    [SerializeField] protected PlantData.BlockType blockType;

    protected Color hoverTint = Color.red, originalColor = Color.white;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlantData.BlockType GetBlockType(){
        return blockType;
    }

    protected virtual void growBlock(){
        Debug.Log("Growing");
    }

    private void OnMouseDown() {
        //Grow();
    }

    private void OnMouseOver()
    {
        Highlight();
        if (Input.GetMouseButtonDown(1)) 
        {
            UnHighlight();
            gameManager = FindObjectOfType<GameManager>();
            Debug.Log(gameManager);
            gameManager.current_selection = this;
            gameManager.ShowUpgrades(getUpgrades(), block_name);
        }
    }

    private void OnMouseExit() {
        UnHighlight();
    }

    protected virtual void Highlight(){
        Debug.Log("Highlighting");
    }

    protected virtual void UnHighlight(){
        Debug.Log("UnHighlighting");
    }

    public bool CanUpgrade(int index){
        return upgradeConditions(index) && canAfford(index);
    }

    public void Upgrade(int index){
        Debug.Log("Upgrading " + block_name);
        gameManager.GainGlucose(-upgradeCost(index));
        performUpgrade(index);
    }

    protected virtual void performUpgrade(int index){
        growBlock();
    }

    protected virtual bool upgradeConditions(int index){
        return true;
    }

    protected bool canAfford(int index){
        return upgradeCost(index) <= gameManager.CurrentGlucose();
    }

    protected virtual int upgradeCost(int index){
        return upgrades[index].cost;
    }

    public virtual List<PlantData.UpgradeData> getUpgrades(){
        return null;
    }

    public virtual void AttatchPlantBlock(Plant_Block other){
        
    }
    
}

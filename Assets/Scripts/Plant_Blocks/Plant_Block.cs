using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
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
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init(){
        gameManager = FindAnyObjectByType<GameManager>();
        children = new List<Plant_Block>();
        InitUpgrades();
    }

    protected virtual void InitUpgrades(){
        
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
        if(!gameManager.canInteract) return;
        Highlight();
        if (Input.GetMouseButtonDown(1)) 
        {
            UnHighlight();
            gameManager = FindObjectOfType<GameManager>();
            gameManager.current_selection = this;
            gameManager.ShowUpgrades(getUpgrades(), block_name);
        }
    }

    private void OnMouseExit() {
        UnHighlight();
    }

    protected virtual void Highlight(){
        getRenderer().color = hoverTint;
    }

    protected virtual void UnHighlight(){
        getRenderer().color = originalColor;
    }

    protected virtual SpriteRenderer getRenderer(){
        return null;
    }

    public bool CanUpgrade(int index){
        return upgradeConditions(index);
    }

    public void Upgrade(int index){
        performUpgrade(index);
    }

    protected virtual void performUpgrade(int index){
        growBlock();
    }

    protected virtual bool upgradeConditions(int index){
        return true;
    }

    public virtual List<PlantData.UpgradeData> getUpgrades(){
        return new List<PlantData.UpgradeData>();
    }

    public virtual void AttatchPlantBlock(Plant_Block other){
        
    }

    public PlantData.BlockType BlockType(){
        return blockType;
    }
    
}

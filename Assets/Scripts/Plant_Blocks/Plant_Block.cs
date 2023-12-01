using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Plant_Block : MonoBehaviour
{
    public GameManager gameManager;
    public string block_name = "Plant Block";
    public Plant_Block parent;
    public List<Plant_Block> children;
    protected List<PlantData.UpgradeData> upgrades;

    protected List<PlantData.ActiveData> actives;
    [SerializeField] protected int health = 100;
    protected int current_health;
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

    public void TakeDamage(int damage){
        current_health -= damage;
        TakeDamageExtras();
        if (current_health <= 0) DestroyBlock();
    }

    protected virtual void TakeDamageExtras(){

    }
    protected virtual void DestroyBlock(){
        Debug.Log("Block Destroyed: " + block_name);
        Destroy(gameObject);
    }

    public virtual void Init(){
        gameManager = FindAnyObjectByType<GameManager>();
        children = new List<Plant_Block>();
        current_health = health;
        InitUpgrades();
        InitActives();
        InitExtras();
    }

    protected virtual void InitUpgrades(){
        
    }

    protected virtual void InitActives(){
        actives = new List<PlantData.ActiveData>();
    }

    protected virtual void InitExtras(){

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
            gameManager.current_selection = this;
            gameManager.ShowUpgrades(getUpgrades(), block_name);
            return;
        }
        else if (Input.GetMouseButtonDown(0)){
            UnHighlight();
            gameManager.current_selection = this;
            gameManager.ShowActives(getActives(), block_name);
            return;
        }
    }


    private void OnMouseExit() {
        UnHighlight();
    }

    protected virtual void Highlight(){
        getRenderer().color = hoverTint;
        HighlightExtras();
    }

    protected virtual void UnHighlight(){
        getRenderer().color = originalColor;
        UnHighlightExtras();
    }

    protected virtual void HighlightExtras(){

    }
    
    protected virtual void UnHighlightExtras(){

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

    public virtual List<PlantData.ActiveData> getActives(){
        return actives;
    }

    public virtual bool CanUseActive(int index){
        return true;
    }

    public virtual void UseActive(int index){
        
    }

    public virtual void AttatchPlantBlock(Plant_Block other){
        
    }

    public PlantData.BlockType BlockType(){
        return blockType;
    }
    
}

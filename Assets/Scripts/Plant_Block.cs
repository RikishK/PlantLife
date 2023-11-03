using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Block : MonoBehaviour
{
    public GameManager gameManager;
    public string block_name = "Plant Block";
    protected Plant_Block parent;
    protected List<Plant_Block> children;
    protected List<PlantData.UpgradeData> upgrades;
    [SerializeField] protected PlantData.BlockType blockType;
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
        if (Input.GetMouseButtonDown(1)) 
        {
            gameManager = FindObjectOfType<GameManager>();
            Debug.Log(gameManager);
            gameManager.current_selection = this;
            gameManager.ShowUpgrades(getUpgrades(), block_name);
        }
    }

    public bool CanUpgrade(){
        return upgradeConditions() && canAfford();
    }

    public void Upgrade(){
        Debug.Log("Upgrading " + block_name);
        gameManager.GainGlucose(-upgradeCost());
        growBlock();
    }

    protected virtual bool upgradeConditions(){
        return true;
    }

    protected bool canAfford(){
        return upgradeCost() <= gameManager.CurrentGlucose();
    }

    protected virtual int upgradeCost(){
        return 0;
    }

    public virtual List<PlantData.UpgradeData> getUpgrades(){
        return null;
    }
    
}

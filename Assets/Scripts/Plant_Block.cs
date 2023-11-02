using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Block : MonoBehaviour
{
    protected GameManager gameManager;
    protected Plant_Block parent;
    protected List<Plant_Block> children;
    [SerializeField] protected PlantData.BlockType blockType;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlantData.BlockType GetBlockType(){
        return blockType;
    }

    public void Grow(){
        growBlock();
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
            gameManager.ShowUpgrades(getUpgrades());
        }
    }

    protected virtual List<PlantData.UpgradeData> getUpgrades(){
        return null;
    }
    
}

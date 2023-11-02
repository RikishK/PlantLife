using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Block : MonoBehaviour
{
    protected Plant_Block parent;
    protected List<Plant_Block> children;
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

    public void Grow(){
        growBlock();
    }

    protected virtual void growBlock(){
        Debug.Log("Growing");
    }

    private void OnMouseDown() {
        Grow();
    }


    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Plant_Leaf : Plant_Block
{
    private PlantData.LeafState leafState = PlantData.LeafState.Small;
    private float lastGlucoseProducedTime = 0;

    [SerializeField] private Sprite SmallLeaf, MediumLeaf, LargeLeaf;
    [SerializeField] private SpriteRenderer leafRenderer;
    [SerializeField] private PlantData.LeafCollider[] leafColliders;
    [SerializeField] private BoxCollider2D leafCollider2D;
    [SerializeField] private GameObject glucosePopupText;
    [SerializeField] private Transform textSpot;
    private int nitrate_value  = 0;

    private void Start() {
        lastGlucoseProducedTime = Time.time;
        block_name = "Leaf";
        gameManager = FindObjectOfType<GameManager>();
        upgrades = new List<PlantData.UpgradeData>{
            new PlantData.UpgradeData("Gorw", 50, PlantData.Resource.Nitrate),
            new PlantData.UpgradeData("Gorw", 100, PlantData.Resource.Nitrate)
        };
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastGlucoseProducedTime > 10.0f){
            lastGlucoseProducedTime = Time.time;
            ProduceGlucose();
        }
    }

    private void ProduceGlucose(){
        int height_bonus = (int)transform.position.y + 1;
        switch (leafState){
            case PlantData.LeafState.Small:
                nitrate_value += 1;
                StartCoroutine(gainText(3  + height_bonus, 5));
                gameManager.GainResource(PlantData.Resource.Glucose, 3 + height_bonus);
                break;
            case PlantData.LeafState.Medium:
                nitrate_value += 3;
                StartCoroutine(gainText(10  + height_bonus*3, 8));
                gameManager.GainResource(PlantData.Resource.Glucose, 10 + height_bonus*2);
                break;
            case PlantData.LeafState.Large:
                nitrate_value += 10;
                StartCoroutine(gainText(25  + height_bonus*5, 13));
                gameManager.GainResource(PlantData.Resource.Glucose, 25 + height_bonus*3);
                break;
        }
    }

    private IEnumerator gainText(int gain, int size){
        GameObject popup = Instantiate(glucosePopupText);
        popup.transform.position = textSpot.position;
        popup.GetComponent<TextMeshPro>().text = "+" + gain.ToString();
        popup.GetComponent<TextMeshPro>().fontSize = size;
        yield return new WaitForSeconds(1.5f);
        Destroy(popup);
    }

    protected override void growBlock()
    {
        switch (leafState){
            case PlantData.LeafState.Small:
                leafState = PlantData.LeafState.Medium;
                break;
            case PlantData.LeafState.Medium:
                leafState = PlantData.LeafState.Large;
                break;
            case PlantData.LeafState.Large:
                break;
        }
        RenderLeaf();
        UpdateLeafCollider();
    }

    private void RenderLeaf(){
        switch (leafState){
            case PlantData.LeafState.Small:
                leafRenderer.sprite = SmallLeaf;
                break;
            case PlantData.LeafState.Medium:
                leafRenderer.sprite = MediumLeaf;
                break;
            case PlantData.LeafState.Large:
                leafRenderer.sprite = LargeLeaf;
                break;
        }
    }

    private void UpdateLeafCollider(){
        foreach(PlantData.LeafCollider leafCollider in leafColliders){
            if(leafCollider.leafState == leafState){
                leafCollider2D.size = leafCollider.plantCollider.size;
                leafCollider2D.offset = leafCollider.plantCollider.offset;
            }
        }
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        switch (leafState){
            case PlantData.LeafState.Small:
                return upgrades.GetRange(0, 1);
            case PlantData.LeafState.Medium:
                return upgrades.GetRange(1, 1);
            case PlantData.LeafState.Large:
                break;
        }
        return new List<PlantData.UpgradeData>();
    }

    protected override void Highlight()
    {
        leafRenderer.color = hoverTint;
    }

    protected override void UnHighlight()
    {
        leafRenderer.color = originalColor;
    }

    public int EatLeaf(int max_intake){
        int intake = Mathf.Min(max_intake, nitrate_value);
        switch(leafState){
            case PlantData.LeafState.Medium:
                leafState = PlantData.LeafState.Small;
                RenderLeaf();
                UpdateLeafCollider();
                nitrate_value -= intake;
                return intake;
            case PlantData.LeafState.Large:
                leafState = PlantData.LeafState.Medium;
                RenderLeaf();
                UpdateLeafCollider();
                nitrate_value -= intake;
                return intake;
        }
        return 0;
    }

    public PlantData.LeafState LeafState(){
        return leafState;
    }
}

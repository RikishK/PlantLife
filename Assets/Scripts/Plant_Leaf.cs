using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private void Start() {
        lastGlucoseProducedTime = Time.time;
        block_name = "Leaf";
        gameManager = FindObjectOfType<GameManager>();
        upgrades = new List<PlantData.UpgradeData>{
            new PlantData.UpgradeData("Gorw", 50, PlantData.Resource.Glucose),
            new PlantData.UpgradeData("Gorw", 100, PlantData.Resource.Glucose)
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
        switch (leafState){
            case PlantData.LeafState.Small:
                StartCoroutine(gainText(3, 5));
                gameManager.GainGlucose(3);
                break;
            case PlantData.LeafState.Medium:
                StartCoroutine(gainText(10, 8));
                gameManager.GainGlucose(10);
                break;
            case PlantData.LeafState.Large:
                StartCoroutine(gainText(25, 13));
                gameManager.GainGlucose(25);
                break;
        }
    }

    private IEnumerator gainText(int gain, int size){
        GameObject popup = Instantiate(glucosePopupText);
        popup.transform.position = gameObject.transform.position;
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

    protected override int upgradeCost(int index)
    {
        switch (leafState){
            case PlantData.LeafState.Small:
                return upgrades[0].cost;
            case PlantData.LeafState.Medium:
                return upgrades[1].cost;
            case PlantData.LeafState.Large:
                break;
        }
        return 0;
    }

    protected override void Highlight()
    {
        leafRenderer.color = hoverTint;
    }

    protected override void UnHighlight()
    {
        leafRenderer.color = originalColor;
    }
}

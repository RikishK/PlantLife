using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Dictionary<PlantData.Resource, int> resources;
    [SerializeField] private TextMeshProUGUI GlucoseText, NitrateText;
    [SerializeField] private GameObject UpgradeMenu;

    public bool canInteract = true;

    public Plant_Block current_selection;
    // Start is called before the first frame update
    void Start()
    {
        resources = new Dictionary<PlantData.Resource, int>(){
            {PlantData.Resource.Glucose, 300},
            {PlantData.Resource.Nitrate, 100}
        };
        GlucoseText.text = $"Glucose: {resources[PlantData.Resource.Glucose]}";
        NitrateText.text = $"Nitrate: {resources[PlantData.Resource.Nitrate]}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainResource(PlantData.Resource resource, int gain){
        resources[resource] += gain;
        GlucoseText.text = $"Glucose: {resources[PlantData.Resource.Glucose]}";
        NitrateText.text = $"Nitrate: {resources[PlantData.Resource.Nitrate]}";

    }


    public int CurrentResource(PlantData.Resource resource){
        return resources[resource];
    }

    public void ShowUpgrades(List<PlantData.UpgradeData> upgradeDatas, string block_name){
        UpgradeMenu.SetActive(true);
        UpgradeMenu.GetComponent<UpgradeMenu>().ShowUpgrades(upgradeDatas, block_name);
    }

    public void Upgrade(PlantData.UpgradeData upgrade, int index){
        if(canAfford(upgrade) && current_selection.CanUpgrade(index)){
            current_selection.Upgrade(index);
            UpgradeMenu.GetComponent<UpgradeMenu>().ShowUpgrades(current_selection.getUpgrades(), current_selection.block_name);
            GainResource(upgrade.resource, -upgrade.cost);
        }
        else Debug.Log("Cant Upgrade");
    }

    private bool canAfford(PlantData.UpgradeData upgrade){
        return resources[upgrade.resource] >= upgrade.cost;
    }
}

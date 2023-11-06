using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Dictionary<PlantData.Resource, int> resources;
    [SerializeField] private TextMeshProUGUI GlucoseText;
    [SerializeField] private GameObject UpgradeMenu;

    public bool canInteract = true;

    public Plant_Block current_selection;
    // Start is called before the first frame update
    void Start()
    {
        resources = new Dictionary<PlantData.Resource, int>(){
            {PlantData.Resource.Glucose, 100},
            {PlantData.Resource.Nitrate, 100}
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainResource(PlantData.Resource resource, int gain){
        resources[resource] += gain;
    }


    public int CurrentResource(PlantData.Resource resource){
        return resources[resource];
    }

    public void ShowUpgrades(List<PlantData.UpgradeData> upgradeDatas, string block_name){
        UpgradeMenu.SetActive(true);
        UpgradeMenu.GetComponent<UpgradeMenu>().ShowUpgrades(upgradeDatas, block_name);
    }

    public void Upgrade(int index){
        if(current_selection.CanUpgrade(index)){
            current_selection.Upgrade(index);
            UpgradeMenu.GetComponent<UpgradeMenu>().ShowUpgrades(current_selection.getUpgrades(), current_selection.block_name);
        }
        else Debug.Log("Cant Upgrade");
    }
}

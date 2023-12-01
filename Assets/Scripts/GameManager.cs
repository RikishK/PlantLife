using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Dictionary<PlantData.Resource, int> resources;
    [SerializeField] private TextMeshProUGUI GlucoseText, NitrateText;
    [SerializeField] private GameObject UpgradeMenu, ActivesMenu, PauseMenu;
    [SerializeField] BackgroundMusic backgroundMusic;

    public bool canInteract = true;

    public Plant_Block current_selection;
    // Start is called before the first frame update
    void Start()
    {
        resources = new Dictionary<PlantData.Resource, int>(){
            {PlantData.Resource.Glucose, 20000},
            {PlantData.Resource.Nitrate, 20000}
        };
        GlucoseText.text = $"{resources[PlantData.Resource.Glucose]}";
        NitrateText.text = $"{resources[PlantData.Resource.Nitrate]}";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            backgroundMusic.Pause();
            PauseMenu.GetComponent<PauseMenu>().Open();
        }
    }

    public void GainResource(PlantData.Resource resource, int gain){
        resources[resource] += gain;
        GlucoseText.text = $"{resources[PlantData.Resource.Glucose]}";
        NitrateText.text = $"{resources[PlantData.Resource.Nitrate]}";

    }


    public int CurrentResource(PlantData.Resource resource){
        return resources[resource];
    }

    public void ShowUpgrades(List<PlantData.UpgradeData> upgradeDatas, string block_name){
        canInteract = false;
        if (upgradeDatas == null){
            canInteract = true;
            return;
        }
        UpgradeMenu.SetActive(true);
        UpgradeMenu.GetComponent<UpgradeMenu>().ShowUpgrades(upgradeDatas, block_name);
    }

    public bool Upgrade(PlantData.UpgradeData upgrade, int index){
        if(canAfford(upgrade) && current_selection.CanUpgrade(index)){
            current_selection.Upgrade(index);
            UpgradeMenu.GetComponent<UpgradeMenu>().ShowUpgrades(current_selection.getUpgrades(), current_selection.block_name);
            
            if(upgrade.resource == PlantData.Resource.Battle_Exp){
                Flower flowerScript = (Flower)current_selection;
                flowerScript.GainExperience(-upgrade.cost);
            }
            else{
                GainResource(upgrade.resource, -upgrade.cost);
            }

            return true;
        }
        Debug.Log("Cant Upgrade");
        Debug.Log("Can Afford: " + canAfford(upgrade));
        Debug.Log("Can Upgrade: " + current_selection.CanUpgrade(index));

        return false;
    }

    private bool canAfford(PlantData.UpgradeData upgrade){
        if(upgrade.resource == PlantData.Resource.Battle_Exp){
            Flower flowerScript = (Flower)current_selection;
            return flowerScript.BattleExp() >= upgrade.cost;
        }
        return resources[upgrade.resource] >= upgrade.cost;
    }

    public void ShowActives(List<PlantData.ActiveData> activesDatas, string block_name){
        canInteract = false;
        if(activesDatas == null){
            canInteract = true;
            return;
        }
        ActivesMenu.SetActive(true);
        ActivesMenu.GetComponent<ActivesMenu>().ShowActives(activesDatas, block_name);
    }

    public bool UseActive(PlantData.ActiveData activeData, int index){
        if(canAffordActive(activeData) && current_selection.CanUseActive(index)){
            //Debug.Log("Can use active");
            current_selection.UseActive(index);
            GainResource(activeData.resource, -activeData.cost);
            return true;
        }
        return false;
    }

    private bool canAffordActive(PlantData.ActiveData activeData){
        return resources[activeData.resource] >= activeData.cost;
    }
}

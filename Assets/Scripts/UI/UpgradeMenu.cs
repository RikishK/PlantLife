using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeSlots;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameManager gameManager;

    private List<PlantData.UpgradeData> currentUpgrades;

    public void ShowUpgrades(List<PlantData.UpgradeData> upgradeDatas, string block_name){
        nameText.text = block_name;
        for(int i=0; i<3; i++){
            if(i>=upgradeDatas.Count){
                upgradeSlots[i].SetActive(false);
            }
            else{
                upgradeSlots[i].SetActive(true);
                upgradeSlots[i].GetComponent<UpgradeSlot>().Setup(upgradeDatas[i]);
            }
        }
        currentUpgrades = upgradeDatas;
    }

    public void Close(){
        gameManager.canInteract = true;
        gameObject.SetActive(false);
    }

    public void Upgrade(int index){
        if(gameManager.Upgrade(currentUpgrades[index], index)) Close();
    }
}

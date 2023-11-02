using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeSlots;

    public void ShowUpgrades(List<PlantData.UpgradeData> upgradeDatas){
        for(int i=0; i<3; i++){
            if(i>=upgradeDatas.Count){
                upgradeSlots[i].SetActive(false);
            }
            else{
                upgradeSlots[i].SetActive(true);
                upgradeSlots[i].GetComponent<UpgradeSlot>().Setup(upgradeDatas[i]);
            }
        }
    }

    public void Close(){
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, costText;

    
    public void Setup(PlantData.UpgradeData upgradeData){
        nameText.text = upgradeData.name;
        costText.text = upgradeData.resource.ToString() + ": " + upgradeData.cost.ToString();
    }
}

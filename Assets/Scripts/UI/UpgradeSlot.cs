using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, costText;
    [SerializeField] private Image costResourceIcon;
    [SerializeField] private Sprite GlucoseIcon, NitrateIcon, BattleExpIcon;
    private PlantData.UpgradeData upgradeData;

    
    public void Setup(PlantData.UpgradeData upgradeData){
        nameText.text = upgradeData.name;
        costText.text = upgradeData.cost.ToString();
        SetIcon(upgradeData.resource);
        this.upgradeData = upgradeData;
    }

    private void SetIcon(PlantData.Resource resource){
        switch(resource){
            case PlantData.Resource.Glucose:
                costResourceIcon.sprite = GlucoseIcon;
                break;
            case PlantData.Resource.Nitrate:
                costResourceIcon.sprite = NitrateIcon;
                break;
            case PlantData.Resource.Battle_Exp:
                costResourceIcon.sprite = BattleExpIcon;
                break;
        }
    }
}

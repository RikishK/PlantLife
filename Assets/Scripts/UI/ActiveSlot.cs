using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, costText;
    [SerializeField] private Image costResourceIcon;
    [SerializeField] private Sprite GlucoseIcon, NitrateIcon;

    private PlantData.ActiveData activeData;

    public void Setup(PlantData.ActiveData activeData){
        //Debug.Log(upgradeData.resource + " : " + upgradeData.cost);
        nameText.text = activeData.name;
        costText.text = activeData.cost.ToString();
        SetIcon(activeData.resource);
        this.activeData = activeData;
    }

    private void SetIcon(PlantData.Resource resource){
        switch(resource){
            case PlantData.Resource.Glucose:
                costResourceIcon.sprite = GlucoseIcon;
                break;
            case PlantData.Resource.Nitrate:
                costResourceIcon.sprite = NitrateIcon;
                break;
        }
    }
}

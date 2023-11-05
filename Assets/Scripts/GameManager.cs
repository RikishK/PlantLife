using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int Glucose = 10000;
    [SerializeField] private TextMeshProUGUI GlucoseText;
    [SerializeField] private GameObject UpgradeMenu;

    public bool canInteract = true;

    public Plant_Block current_selection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainGlucose(int gain){
        Glucose += gain;
        GlucoseText.text = "Glucose: " + Glucose.ToString();
    }

    public int CurrentGlucose(){
        return Glucose;
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

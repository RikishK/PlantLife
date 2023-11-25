using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivesMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] activesSlots;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameManager gameManager;
    
    private List<PlantData.ActiveData> currentActives;

    public void ShowActives(List<PlantData.ActiveData> activesData, string block_name){
        nameText.text = block_name;
        for(int i=0; i<3; i++){
            if(i>=activesData.Count){
                activesSlots[i].SetActive(false);
            }
            else{
                activesSlots[i].SetActive(true);
                activesSlots[i].GetComponent<ActiveSlot>().Setup(activesData[i]);
            }
        }
        currentActives = activesData;
    }

    public void Close(){
        gameObject.SetActive(false);
    }

    public void UseActive(int index){
        Debug.Log("Using Active");
        if(gameManager.UseActive(currentActives[index], index)) Close();
    }
}

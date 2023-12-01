using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawnerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyInfoSlots;
    [SerializeField] private Slider prepTimer;
    [SerializeField] private TextMeshProUGUI waveNumberText;

    private float prep_start_time, prep_time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpawnerUI(float prep_time, EnemyData.EnemySpawnData[] enemySpawnDatas, int wave_number){
        prep_start_time = Time.time;
        this.prep_time = prep_time;
        waveNumberText.text = "Wave: " + wave_number.ToString();
        StartCoroutine(PrepTimer());
        UpdateEnemyInfoSlots(enemySpawnDatas);
    }

    private IEnumerator PrepTimer(){
        while(prep_time - (Time.time - prep_start_time) > 0){
            UpdatePrepTimerSlider();
            yield return null;
        }
    }

    private void UpdatePrepTimerSlider(){
        prepTimer.maxValue = prep_time;
        prepTimer.value = prep_time - (Time.time - prep_start_time);
    }

    private void UpdateEnemyInfoSlots(EnemyData.EnemySpawnData[] enemySpawnDatas){
        for(int i=0; i<6; i++){
            if(i < enemySpawnDatas.Length ){
                enemyInfoSlots[i].SetActive(true);
                // Update Enemy Slot
                enemyInfoSlots[i].GetComponent<WaveSpawnerEnemySlot>().Setup(enemySpawnDatas[i].enemyType, enemySpawnDatas[i].count);
            }
            else{
                enemyInfoSlots[i].SetActive(false);
            }
        }
    }
}

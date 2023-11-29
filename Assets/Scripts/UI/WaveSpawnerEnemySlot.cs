using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawnerEnemySlot : MonoBehaviour
{
    [SerializeField] private Image enemyIcon;
    [SerializeField] private TextMeshProUGUI countText;

    [SerializeField] private IconData[] enemyIconsInfo;

    [System.Serializable]
    private class IconData{
        public EnemyData.EnemyType EnemyType;
        public Sprite icon;
        public Color overColor;
        public bool doColor = false;
    }

    public void Setup(EnemyData.EnemyType enemyType, int count){
        countText.text = count.ToString();
        foreach(IconData enemyIconInfo in enemyIconsInfo){
            if(enemyIconInfo.EnemyType == enemyType){
                enemyIcon.sprite = enemyIconInfo.icon;
                if(enemyIconInfo.doColor) enemyIcon.color = enemyIconInfo.overColor;    
            }
        }
    }

}

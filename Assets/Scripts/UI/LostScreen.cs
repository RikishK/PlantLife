using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    // Start is called before the first frame update
    void Start()
    {
        int wave_number = PlayerPrefs.GetInt("WaveNumber");
        WaveSetup(wave_number);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue(){
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    public void WaveSetup(int wave_number){
        waveText.text = "Wave: " + wave_number;
    }
}

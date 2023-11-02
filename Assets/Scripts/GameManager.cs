using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int Glucose = 0;
    [SerializeField] private TextMeshProUGUI GlucoseText;
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
}

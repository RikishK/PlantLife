using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private BackgroundMusic backgroundMusic;
    [SerializeField] private Slider musicVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open(){
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void Close(){
        backgroundMusic.Resume();
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void ChangeMusicSound(){
        backgroundMusic.Volume(musicVolumeSlider.value);
    }

    public void Quit(){
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    
}

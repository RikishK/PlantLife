using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuideMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] screens;
    private int curr_screen = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScreen(){
        screens[curr_screen].SetActive(false);
        curr_screen++;
        if(curr_screen == screens.Length) curr_screen = 0;
        screens[curr_screen].SetActive(true);
    }

    public void PrevScreen(){
        screens[curr_screen].SetActive(false);
        curr_screen--;
        if(curr_screen < 0) curr_screen = screens.Length - 1;
        screens[curr_screen].SetActive(true);
    }

    public void Back(){
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainScene()
    {
        // Load the "Main" scene
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void LoadGuideScene(){
        SceneManager.LoadScene("GuideScene", LoadSceneMode.Single);

    }
}

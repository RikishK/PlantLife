using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundSongs;
    [SerializeField] private AudioSource audioSource;
    private int curr_song = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BackgroundMusicRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator BackgroundMusicRoutine(){
        while(true){
            audioSource.Stop();
            audioSource.clip = backgroundSongs[curr_song];
            audioSource.Play();
            yield return new WaitForSeconds(backgroundSongs[curr_song].length);
            curr_song++;
            if(curr_song == backgroundSongs.Length) curr_song = 0;
        }
    }

    public void Pause(){
        audioSource.Pause();
    }

    public void Resume(){
        audioSource.Play();
    }

    public void Volume(float volume){
        audioSource.volume = volume;
    }
}

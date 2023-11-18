using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float waitTime, animationLength;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual IEnumerator PlayAnimation(){
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("Play");
        yield return new WaitForSeconds(animationLength);
        Destroy(gameObject);
    }
}

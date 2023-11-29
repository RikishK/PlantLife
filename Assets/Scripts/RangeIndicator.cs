using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private SpriteRenderer RangeIndicatorRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public void Init(Color color, float size){
        RangeIndicatorRenderer.color = color;
        transform.localScale = new Vector3(size * 2, size * 2, 1f);
    }
}

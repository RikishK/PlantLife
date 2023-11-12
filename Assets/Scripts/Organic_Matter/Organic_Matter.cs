using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organic_Matter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer organicMatterRenderer;
    [SerializeField] private Sprite[] stageSprites;
    
    [SerializeField] private int current_stage;
    [SerializeField] private int nitrate_per_bite;

    private void Start() {
        current_stage = stageSprites.Length;
    }
    
    public void Consume(){
        current_stage--;
        if(current_stage == 0){
            Destroy(gameObject);
        }
        organicMatterRenderer.sprite = stageSprites[stageSprites.Length - current_stage];
    }

    public int Harvest(){
        return nitrate_per_bite;
    }

    public void Setup(int nitrate_per_bite){
        this.nitrate_per_bite = nitrate_per_bite;
    }

    private void Update() {
        float moveSpeed = transform.position.y > -1f ? 2.0f: 0.01f;
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}

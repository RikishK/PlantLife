using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;


public class Plant_Stem : Plant_Block
{ 
    private PlantData.StemState stemState = PlantData.StemState.Green;
    [SerializeField] private Sprite GreenStem, MidStem, BrownStem, ThickBrownStem;
    [SerializeField] private SpriteRenderer stemRenderer;
    [SerializeField] private PlantData.StemCollider[] stemColliders;
    [SerializeField] private BoxCollider2D stemCollider2D;
    protected override void growBlock()
    {
        switch(stemState){
            case PlantData.StemState.Green:
                stemState = PlantData.StemState.Mid;
                break;
            case PlantData.StemState.Mid:
                stemState = PlantData.StemState.Brown;
                break;
            case PlantData.StemState.Brown:
                stemState = PlantData.StemState.Thick_Brown;
                break;
            case PlantData.StemState.Thick_Brown:
                break;
        }
        RenderStem();
        UpdateStemCollider();
    }

    private void RenderStem(){
        switch (stemState){
            case PlantData.StemState.Green:
                stemRenderer.sprite = GreenStem;
                break;
            case PlantData.StemState.Mid:
                stemRenderer.sprite = MidStem;
                break;
            case PlantData.StemState.Brown:
                stemRenderer.sprite = BrownStem;
                break;
            case PlantData.StemState.Thick_Brown:
                stemRenderer.sprite = ThickBrownStem;
                break;
        }
    }

    private void UpdateStemCollider(){
        foreach(PlantData.StemCollider stemCollider in stemColliders){
            if(stemCollider.stemState == stemState){
                stemCollider2D.size = stemCollider.plantCollider.size;
            }
        }
    }
}

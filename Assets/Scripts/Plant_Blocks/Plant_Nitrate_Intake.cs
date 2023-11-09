using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Nitrate_Intake : Plant_Block
{
    public Transform extensionPoint;
    public string fungiManTag = "FungiMan"; // Tag for FungiMan objects

    [SerializeField] SpriteRenderer nitrateIntakeRenderer;

    private void Update()
    {
        
    }

    private void OnMouseDown(){
        // Find all GameObjects with the specified tag
        GameObject[] fungiManObjects = GameObject.FindGameObjectsWithTag(fungiManTag);

        // Call Interrupt on each FungiMan script
        foreach (GameObject fungiManObject in fungiManObjects)
        {
            FungiMan fungiManScript = fungiManObject.GetComponent<FungiMan>();
            if (fungiManScript != null)
            {
                fungiManScript.Interrupt(transform.position);
            }
        }
    }

    protected override SpriteRenderer getRenderer()
    {
        return nitrateIntakeRenderer;
    }
}

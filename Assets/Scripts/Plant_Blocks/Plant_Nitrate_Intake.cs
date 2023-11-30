using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Nitrate_Intake : Plant_Block
{
    public Transform extensionPoint;

    [SerializeField] SpriteRenderer nitrateIntakeRenderer;

    private void Update()
    {
        
    }

    protected override SpriteRenderer getRenderer()
    {
        return nitrateIntakeRenderer;
    }

    protected override void InitActives()
    {
        actives = new List<PlantData.ActiveData>(){
            new PlantData.ActiveData("Call Fungi", 20, PlantData.Resource.Glucose),
        };
    }

    public override bool CanUseActive(int index)
    {
        return true;
    }

    public override void UseActive(int index)
    {
        // Find all GameObjects with the specified tag
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        List<Creature> fungiList = new List<Creature>();
        foreach(GameObject creature in creatures){
            Creature creatureScript = creature.GetComponent<Creature>();
            if (creatureScript.creatureType == CreatureSpawnData.CreatureType.Fungi) fungiList.Add(creatureScript);
        }

        // Call Interrupt on each FungiMan script
        foreach (Creature fungi in fungiList)
        {
            FungiMan fungiManScript = (FungiMan)fungi;
            if (fungiManScript != null)
            {
                fungiManScript.Interrupt(transform.position);
            }
        }
    }
}

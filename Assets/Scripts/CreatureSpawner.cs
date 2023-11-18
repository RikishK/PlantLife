using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] CreatureSpawnData.SpawnData[] spawnDatas;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Plant_Core plant_Core;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnerLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnerLoop(){
        while(true){
            foreach(CreatureSpawnData.SpawnData spawnData in spawnDatas){
                foreach(CreatureSpawnData.SpawnCondition spawnCondition in spawnData.spawnConditions){
                    bool plant_conditions_met = true;
                    // Loop through all plant_conditions and make sure they are met
                    foreach (CreatureSpawnData.PlantCondition plantCondition in spawnCondition.plantConditions){
                        if (!CheckPlantCondition(plantCondition)){
                            plant_conditions_met = false;
                            break;
                        }
                    }

                    bool world_conditions_met = true;
                    // Loop through all plant_conditions and make sure they are met
                    foreach (CreatureSpawnData.WorldCreaturesCondition worldCreaturesCondition in spawnCondition.worldCreaturesConditions){
                        if (!CheckWorldCreatureCondition(worldCreaturesCondition)){
                            world_conditions_met = false;
                            break;
                        }
                    }


                    if(plant_conditions_met && world_conditions_met){
                        int chance = Random.Range(1, 101);
                        if (chance <= spawnData.spawnChance){
                            // Gather spawn behaviour data and determine how much to spawn
                            Debug.Log("Spawning on condition:" + spawnCondition.spawnConditionName);
                            Spawn(spawnCondition.spawnBehaviour, spawnData.creatureObject);
                        }
                    }
                }
                
            }
            yield return new WaitForSeconds(10f);
        }
    }

    private bool CheckPlantCondition(CreatureSpawnData.PlantCondition plant_condition){
        if (!plant_condition.use) return true;
        int target_block_count = CountPlantBlock(plant_condition.plantBlockType);
        return EvaluateCondition(plant_condition.constriction, target_block_count, plant_condition.constrictionConstants);
    }

    private bool CheckWorldCreatureCondition(CreatureSpawnData.WorldCreaturesCondition world_creature_condition){
        if (!world_creature_condition.use) return true;
        int creature_count = CountCreature(world_creature_condition.creatureType);
        return EvaluateCondition(world_creature_condition.constriction, creature_count, world_creature_condition.worldCreatureConditionConstants);
    }

    private int CountCreature(CreatureSpawnData.CreatureType creatureType){
        int count = 0;
        GameObject[] creatureObjects = GameObject.FindGameObjectsWithTag("Creature");
        foreach(GameObject creatureObject in creatureObjects){
            Creature creatureScript = creatureObject.GetComponent<Creature>();
            if (creatureScript.creatureType == creatureType) count++;
        }
        return count;
    }

    private bool EvaluateCondition(CreatureSpawnData.Constriction constriction, int target_count, int[] constrictionConstants){
        switch (constriction){
            case CreatureSpawnData.Constriction.Less_Than:
                return target_count < constrictionConstants[0];
            case CreatureSpawnData.Constriction.More_Than:
                return target_count > constrictionConstants[0];
            case CreatureSpawnData.Constriction.Between:
                return target_count > constrictionConstants[0] && target_count < constrictionConstants[1];
        }
        return true;
    }
    private int CountPlantBlock(PlantData.BlockType blockType){
        // traverse plant and count block
        int block_count = plant_Core.CountBlock(blockType);
        Debug.Log(blockType + " : " + block_count);
        return block_count;
    }

    private void Spawn(CreatureSpawnData.SpawnBehaviour spawnBehaviour, GameObject spawnCreature){
        int spawn_count = 0;
        if(spawnBehaviour.conditionType == CreatureSpawnData.ConditionType.Plant_Based){
            spawn_count = SpawnCountPlantBased(spawnBehaviour.plantBasedSpawnAlgorithm);
            SpawnCreatures(spawn_count, spawnBehaviour, spawnCreature);
        }
        else if(spawnBehaviour.conditionType == CreatureSpawnData.ConditionType.World_Creatures_Based){
            spawn_count = SpawnCountWorldCreatureBased(spawnBehaviour.worldCreatureBasedSpawnAlgorithm);
            SpawnCreatures(spawn_count, spawnBehaviour, spawnCreature);
        }

    }

    private void SpawnCreatures(int count, CreatureSpawnData.SpawnBehaviour spawnBehaviour, GameObject spawnCreature){
        switch (spawnBehaviour.spawnLocationType){
            case CreatureSpawnData.SpawnLocationType.onBlock:
                List<Plant_Block> valid_blocks = plant_Core.GetBlocksOfType(spawnBehaviour.targetSpawnBlock);
                for(int i=0; i<count; i++){
                    Plant_Block random_block = valid_blocks[Random.Range(0, valid_blocks.Count)];
                    GameObject creature = Instantiate(spawnCreature);
                    creature.transform.position = random_block.transform.position;
                }
                break;
            case CreatureSpawnData.SpawnLocationType.randomInArea:
                for(int i=0; i<count; i++){
                    GameObject creature = Instantiate(spawnCreature);
                    float random_x = Random.Range(spawnBehaviour.spawnXLimits[0], spawnBehaviour.spawnXLimits[1]);
                    float random_y = Random.Range(spawnBehaviour.spawnYLimits[0], spawnBehaviour.spawnYLimits[1]);
                    creature.transform.position = new Vector3(random_x, random_y, 0f);
                }
                break;
        }
    }

    private int SpawnCountPlantBased(CreatureSpawnData.PlantBasedSpawnAlgorithm plantBasedSpawnAlgorithm){
        int block_count = plant_Core.CountBlock(plantBasedSpawnAlgorithm.targetBlock);
        int algorithmEvaluation = block_count;
        for(int i=0; i<plantBasedSpawnAlgorithm.algorithmExecutions.Length; i++){
            switch(plantBasedSpawnAlgorithm.algorithmExecutions[i]){
                case CreatureSpawnData.AlgorithmExecution.Plus:
                    algorithmEvaluation += plantBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
                case CreatureSpawnData.AlgorithmExecution.Minus:
                    algorithmEvaluation -= plantBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
                case CreatureSpawnData.AlgorithmExecution.Divide:
                    algorithmEvaluation = algorithmEvaluation / plantBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
                case CreatureSpawnData.AlgorithmExecution.Multiply:
                    algorithmEvaluation = algorithmEvaluation * plantBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
            }
        }
        return algorithmEvaluation;
    }

    private int SpawnCountWorldCreatureBased(CreatureSpawnData.WorldCreatureBasedSpawnAlgorithm worldCreatureBasedSpawnAlgorithm){
        GameObject[] creature_objects = GameObject.FindGameObjectsWithTag("Creature");
        int creature_count = 0;
        foreach(GameObject creature_object in creature_objects){
            Creature creatureScript = creature_object.GetComponent<Creature>();
            if (creatureScript != null && creatureScript.creatureType == worldCreatureBasedSpawnAlgorithm.creatureType){
                creature_count++;
            }
        }

        int algorithmEvaluation = creature_count;
        for(int i=0; i<worldCreatureBasedSpawnAlgorithm.algorithmExecutions.Length; i++){
            switch(worldCreatureBasedSpawnAlgorithm.algorithmExecutions[i]){
                case CreatureSpawnData.AlgorithmExecution.Plus:
                    algorithmEvaluation += worldCreatureBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
                case CreatureSpawnData.AlgorithmExecution.Minus:
                    algorithmEvaluation -= worldCreatureBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
                case CreatureSpawnData.AlgorithmExecution.Divide:
                    algorithmEvaluation = algorithmEvaluation / worldCreatureBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
                case CreatureSpawnData.AlgorithmExecution.Multiply:
                    algorithmEvaluation = algorithmEvaluation * worldCreatureBasedSpawnAlgorithm.algorithmConstants[i];
                    break;
            }
        }
        return algorithmEvaluation;
    }
}

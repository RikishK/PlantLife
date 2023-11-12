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
                    bool plant_condition_met = CheckPlantCondition(spawnCondition.plantCondition);
                    bool world_condition_met = CheckWorldCreatureCondition(spawnCondition.worldCreaturesCondition);
                    if(plant_condition_met && world_condition_met){
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
        int target_block_count = CountPlantBlock(plant_condition.plantBlockType);
        return EvaluateCondition(plant_condition.constriction, target_block_count, plant_condition.constrictionConstants);
    }

    private bool CheckWorldCreatureCondition(CreatureSpawnData.WorldCreaturesCondition world_creature_condition){
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
}

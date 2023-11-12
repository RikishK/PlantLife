using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawnData
{
    [System.Serializable]
    public class SpawnData {
        public GameObject creatureObject;
        public int spawnChance;
        public SpawnCondition[] spawnConditions;
    }

    [System.Serializable]
    public class SpawnCondition{
        public string spawnConditionName;
        public PlantCondition plantCondition;
        public WorldCreaturesCondition worldCreaturesCondition;

        public SpawnBehaviour spawnBehaviour;
    }

    public enum ConditionType {
        Plant_Based,
        World_Creatures_Based
    }

    [System.Serializable]
    public class Condition {

    }

    [System.Serializable]
    public class PlantCondition : Condition {
        public PlantData.BlockType plantBlockType;
        public Constriction constriction;
        public int[] constrictionConstants;
    }

    [System.Serializable]
    public class WorldCreaturesCondition : Condition {
        public CreatureType creatureType;
        public Constriction constriction;
        public int[] worldCreatureConditionConstants;
    }

    public enum Constriction{
        Less_Than,
        More_Than,
        Between
    }

    public enum CreatureType {
        Aphid, Worm, Fungi, Bacteria
    }

    [System.Serializable]
    public class SpawnBehaviour {
        public ConditionType conditionType;
        public PlantBasedSpawnAlgorithm plantBasedSpawnAlgorithm;
        public SpawnLocationType spawnLocationType;
        public PlantData.BlockType targetSpawnBlock;
        public int[] spawnXLimits, spawnYLimits;
    }

    [System.Serializable]
    public class SpawnAlgorithm {
        public AlgorithmExecution[] algorithmExecutions;
        public int[] algorithmConstants;
    }

    [System.Serializable]
    public class PlantBasedSpawnAlgorithm : SpawnAlgorithm  {
        public PlantData.BlockType targetBlock;
    }

    public enum AlgorithmExecution {
        Plus,
        Minus,
        Divide,
        Multiply
    }

    public enum SpawnLocationType{
        onBlock, randomInArea
    }
}

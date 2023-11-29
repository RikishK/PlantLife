using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public enum EnemyType {
        AphidEnemy
    }

    [System.Serializable]
    public class EnemySpawnData{
        public EnemyType enemyType;
        public int count;
        public float spawnGap;
        public EnemySpawnLocationType enemySpawnLocationType;
    }

    [System.Serializable]
    public class EnemyWave {
        public float prepTime;
        public EnemySpawnData[] enemySpawnDatas;
    }

    public enum EnemySpawnLocationType {
        Outer_Grounds
    }
}

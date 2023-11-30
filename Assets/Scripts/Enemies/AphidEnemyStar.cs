using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidEnemyStar : Enemy
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private int count;

    protected override void DieExtras()
    {
        for(int i=0; i<count; i++){
            GameObject newEnemyObj = Instantiate(enemyToSpawn);
            Vector3 spawnPos = transform.position;
            spawnPos.x += Random.Range(-0.5f, 0.5f);
            spawnPos.y += Random.Range(-0.5f, 0.5f);
            newEnemyObj.transform.position = spawnPos;
        }
    }
}

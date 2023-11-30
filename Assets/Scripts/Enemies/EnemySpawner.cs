using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyData.EnemyWave[] waves;
    [SerializeField] private GameObject redAphidEnemyPrefab, orangeAphidEnemyPrefab, purpleAphidEnemyPrefab, pinkAphidEnemyPrefab;
    [SerializeField] private WaveSpawnerUI waveSpawnerUI;
    private int current_wave;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnWaves(){
        // Delay for player wave prep time
        //Debug.Log("Wave: " + current_wave + " starts in " + waves[current_wave].prepTime + " seconds");
        waveSpawnerUI.gameObject.SetActive(true);
        waveSpawnerUI.UpdateSpawnerUI(waves[current_wave].prepTime, waves[current_wave].enemySpawnDatas, current_wave+1);
        yield return new WaitForSeconds(waves[current_wave].prepTime);
        waveSpawnerUI.gameObject.SetActive(false);

        // Spawn enemies
        foreach(EnemyData.EnemySpawnData enemySpawnData in waves[current_wave].enemySpawnDatas){
            for(int i=0; i < enemySpawnData.count; i++){
                SpawnEnemy(enemySpawnData);
                yield return new WaitForSeconds(enemySpawnData.spawnGap);
            }
        }

        // Continue if more waves
        current_wave++;
        if (current_wave < waves.Length) StartCoroutine(SpawnWaves());

    }

    private void SpawnEnemy(EnemyData.EnemySpawnData enemySpawnData){
        switch(enemySpawnData.enemySpawnLocationType){
            case EnemyData.EnemySpawnLocationType.Outer_Grounds:
                GameObject enemyObject = Instantiate(GetEnemyPrefab(enemySpawnData.enemyType));
                float spawn_x = Random.Range(7f, 10f);
                bool spawnLeft = Random.Range(0f, 100f) > 50f;
                if (spawnLeft) spawn_x *= -1;
                Vector3 spawn_location = new Vector3(spawn_x, -1, 0);
                enemyObject.transform.position = spawn_location;
                //Debug.Log("Spawned Enemy at: " + spawn_location);
                break;
        }
    }

    private GameObject GetEnemyPrefab(EnemyData.EnemyType enemyType){
        switch (enemyType){
            case EnemyData.EnemyType.RedAphidEnemy:
                return redAphidEnemyPrefab;
            case EnemyData.EnemyType.OrangeAphidEnemy:
                return orangeAphidEnemyPrefab;
            case EnemyData.EnemyType.PurpleAphidEnemy:
                return purpleAphidEnemyPrefab;
            case EnemyData.EnemyType.PinkAphidEnemy:
                return pinkAphidEnemyPrefab;
        }
        return null;
    }
}

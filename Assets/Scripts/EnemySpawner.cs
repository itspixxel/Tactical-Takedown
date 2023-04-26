using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    private int enemiesToSpawn;
    private int enemiesRemaining;
    float nextSpawnTime;

    private Wave currentWave;
    private int currentWaveNum;

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float spawnRate;
    }

    private void Start()
    {
        NextWave();    
    }

    private void Update()
    {
        if (enemiesToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesToSpawn--;
            nextSpawnTime = Time.time + currentWave.spawnRate;

            Enemy newEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
            newEnemy.onDeath += onEnemyDeath;
        }
    }
    
    void NextWave()
    {
        currentWaveNum++;

        Debug.Log("Wave: " + currentWaveNum);

        // Make sure it doesn't go out of the number of waves index
        if(currentWaveNum - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNum - 1];

            enemiesToSpawn = currentWave.enemyCount;
            enemiesRemaining = enemiesToSpawn;
        }

    }

    void onEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0) 
        {
            NextWave();
        }
    }
}

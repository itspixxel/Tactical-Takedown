using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;
    public Color flashColor;

    Entity playerEntity;
    private Transform playerTransform;

    private int enemiesToSpawn;
    private int enemiesRemaining;
    float nextSpawnTime;

    private Wave currentWave;
    private int currentWaveNum;

    TileSpawner tileSpawner;

    public event System.Action<int> OnNewWave;

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float spawnRate;
    }

    private void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerTransform = playerEntity.transform;
        tileSpawner = FindObjectOfType<TileSpawner>();
        NextWave();    
    }

    private void Update()
    {
        if (enemiesToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesToSpawn--;
            nextSpawnTime = Time.time + currentWave.spawnRate;

            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1f;
        float tileFlashRate = 4f;
        
        Transform randomTile = tileSpawner.GetRandomOpenTile();
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color initialCol = tileMat.color;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialCol, flashColor, Mathf.PingPong(spawnTimer * tileFlashRate, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy newEnemy = Instantiate(enemy, randomTile.position + Vector3.up, Quaternion.identity);
        newEnemy.OnDeath += onEnemyDeath;
    }

    void NextWave()
    {
        currentWaveNum++;

        // Make sure it doesn't go out of the number of waves index
        if (currentWaveNum - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNum - 1];

            enemiesToSpawn = currentWave.enemyCount;
            enemiesRemaining = enemiesToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNum);
                ResetPlayerPos();
            }
        }
    }

    void ResetPlayerPos()
    {
        playerTransform.position = tileSpawner.GetRandomOpenTile().position + Vector3.up * 2f;
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

using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class EnemySpaawner : MonoBehaviour
{
[System.Serializable]
    public class SpawnableEnemy
    {
        public string name;
        public GameObject enemyPrefab;
        public int count;
        public float interval;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<SpawnableEnemy> enemiesInWave = new List<SpawnableEnemy>();
    }

    public List<Wave> waves = new List<Wave>();

    [Header("Random Spawn Area")]
    public Transform spawnPointA;  // Bottom-left corner of spawn area
    public Transform spawnPointB;  // Top-right corner of spawn area

    public float timeBetweenWaves = 5f;
    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"Starting Wave {currentWaveIndex + 1}: {currentWave.waveName}");

            yield return StartCoroutine(SpawnWaveRandomized(currentWave));

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("All waves completed!");
    }

    IEnumerator SpawnWaveRandomized(Wave wave)
    {
        List<SpawnableEnemy> spawnQueue = new List<SpawnableEnemy>();

        foreach (var enemy in wave.enemiesInWave)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                spawnQueue.Add(enemy);
            }
        }

        Shuffle(spawnQueue);

        foreach (var enemy in spawnQueue)
        {
            SpawnEnemy(enemy.enemyPrefab);
            yield return new WaitForSeconds(enemy.interval);
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (!spawnPointA || !spawnPointB)
        {
            Debug.LogWarning("Spawn points A and B must be assigned!");
            return Vector3.zero;
        }

        float randomX = Random.Range(spawnPointA.position.x, spawnPointB.position.x);
        float randomY = Random.Range(spawnPointA.position.y, spawnPointB.position.y);
        float randomZ = Random.Range(spawnPointA.position.z, spawnPointB.position.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

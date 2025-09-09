using UnityEngine;

public class DirtSpawnManager : MonoBehaviour
{
    public GameObject dirtPrefab;
    public int spawnCount = 10;
    public float spawnRange = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnDirt(spawnCount);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnDirt(int dirtToSpawn){
        for(int i = 0; i<dirtToSpawn; i++){
            //spawn range will be adgusted to fit when art is imported
            float spawnPosX = Random.Range(-spawnRange, spawnRange+1);
            float spawnPosY = Random.Range(-spawnRange, spawnRange);
            Vector3 randomPos = new Vector3(spawnPosX, spawnPosY, 0);

            Instantiate(dirtPrefab, randomPos, Quaternion.identity);
        }
    }
}

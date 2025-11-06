using UnityEngine;

public class DirtSpawnManager : MonoBehaviour
{
    //public GameObject spawnPlace;

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
        //Vector3 randomRot = new Vector3(0,0,Random.Range(-90,90));

        for(int i = 0; i<(dirtToSpawn)/2; i++){
            //spawn range will be adgusted to fit when art is imported
            float spawnPosX = Random.Range(1.0f, 5f);
            float spawnPosY = Random.Range(1.7f, 0f);
            Vector3 randomPos = new Vector3(spawnPosX, spawnPosY, 0);

            Instantiate(dirtPrefab, randomPos, Quaternion.Euler(0,0,Random.Range(-90,90)));
        }
        for(int i = 0; i<(dirtToSpawn)/2; i++){
            //spawn range will be adgusted to fit when art is imported
            float spawnPosX = Random.Range(3.5f, 5.5f);
            float spawnPosY = Random.Range(-2.3f, -4.3f);
            Vector3 randomPos = new Vector3(spawnPosX, spawnPosY, 0);
            

            Instantiate(dirtPrefab, randomPos, Quaternion.Euler(0,0,Random.Range(-90,90)));
        }
    }
}

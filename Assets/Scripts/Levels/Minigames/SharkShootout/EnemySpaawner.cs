using System.Collections;
using UnityEngine;

public class EnemySpaawner : MonoBehaviour
{
    [SerializeField] private GameObject slowFish;
    [SerializeField] private GameObject normalFish;
    [SerializeField] private GameObject fastFish;

    [SerializeField] private float slowInterval = 5.5f;
    [SerializeField] private float normalInterval = 3.5f;
    [SerializeField] private float fastInterval = 2f;
    void Start()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}

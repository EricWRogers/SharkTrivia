using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentGenerator : MonoBehaviour
{
    [Header("Generator")]
    public GameObject[] segments; // Array of segment prefabs
    public int maxSegments = 10; // Total segments before placing the final one
    private int index = 4; // Index of the final segment

    [SerializeField] int zPos = 50;
    [SerializeField] float waitTime = 5f;

    private bool segmentCreate = false;
    public int generatedCount = 0; // Tracks how many segments have been spawned

    private List<int> availableSegments; // List of indices to pick from without repeats
    
    void Start()
    {
        // Initialize the list of available segments (excluding the final segment)
        availableSegments = new List<int>();
        for (int i = 0; i < segments.Length; i++)
        {
            if (i != index) // exclude final segment
                availableSegments.Add(i);
        }
    }

    void Update()
    {
        if (!segmentCreate && generatedCount < maxSegments + 1)
        {
            segmentCreate = true;
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        // If we haven't reached max segments
        if (generatedCount < maxSegments)
        {
            // Shuffle available segments if empty
            if (availableSegments.Count == 0)
            {
                for (int i = 0; i < segments.Length; i++)
                {
                    if (i != index) // exclude final segment
                        availableSegments.Add(i);
                }
            }

            // Pick a random segment index from available segments
            int randListIndex = Random.Range(0, availableSegments.Count);
            int segmentIndex = availableSegments[randListIndex];

            // Remove the chosen index to avoid immediate repeats
            availableSegments.RemoveAt(randListIndex);

            Instantiate(segments[segmentIndex], new Vector3(0, 0, zPos), Quaternion.identity);
        }
        
        else
        {
            // Spawn the final segment
            Instantiate(segments[index], new Vector3(0, 0, zPos), Quaternion.identity);
        }
        
        generatedCount++;
        zPos += 50;

        yield return new WaitForSeconds(waitTime);
        segmentCreate = false;
    }
}

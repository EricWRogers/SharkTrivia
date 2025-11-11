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
            int segmentNum = Random.Range(0, segments.Length - 1); // Excludes last segment
            Instantiate(segments[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
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

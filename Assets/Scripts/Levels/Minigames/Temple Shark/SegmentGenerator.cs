using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentGenerator : MonoBehaviour
{
    [Header("Generator")]
    public GameObject[] segment;

    [SerializeField] int zPos = 50;
    [SerializeField] bool segmentCreate = false;
    [SerializeField] int segmentNum; 

    void Update()
    {
        if (segmentCreate == false)
        {
            segmentCreate = true;
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        segmentNum = Random.Range(0, 3);
        Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        zPos += 50;
        yield return new WaitForSeconds(3);
        segmentCreate = false;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentLoader : MonoBehaviour
{
    [Header("Processor")]
    public List<GameObject> segments;

    [Header("Loading time")]
    public int seconds = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SegmentLoad());
    }

    IEnumerator SegmentLoad()
    {
        foreach (GameObject obj in segments)
        {
            Debug.Log($"Loading segment: {obj.name}");
            yield return new WaitForSeconds(seconds);
            obj.SetActive(true);
        }
        Debug.Log("Segment loading completed");
    }
}

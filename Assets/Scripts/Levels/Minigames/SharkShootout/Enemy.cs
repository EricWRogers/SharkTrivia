using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal;
using System;


public class Enemy : MonoBehaviour

{


    public GameObject objectToDestroy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("FunctionToDestroy", 30f);
        StartCoroutine(DestroyCoroutine());
    }


    void FunctionToDestroy()
    {
        Destroy(objectToDestroy);
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(30f);
        Destroy(objectToDestroy);
    }
    
   
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -20){
            Destroy(gameObject);
            GetComponent<SpriteRenderer>().enabled = false;
        }
       
    }
}

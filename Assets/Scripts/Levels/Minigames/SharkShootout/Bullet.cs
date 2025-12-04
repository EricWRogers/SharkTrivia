using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Bullet : MonoBehaviour
{

    
    public float Speed = 5f;
    public float life = 1f;
    public GameObject particles;

    // SFX for bullet


    void Start()
    {
        Destroy(gameObject, life);

    }
    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * Speed;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {

            Destroy(collision.gameObject);
        
            Pop(); 

            // SFX
               GameObject particle = Instantiate(particles, transform.position, Quaternion.identity); 
               particle.GetComponent<ParticleSystem>().Play();  

            Destroy(gameObject);
        } 

    
    }

    void Pop()
    {
        //Instantiate(PopEffect, transform.position, transform.rotation);
    }
}



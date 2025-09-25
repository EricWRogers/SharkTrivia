using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour
{
    public float speed;
    private float move;
    private Rigidbody2D rb;
    

    // Gun Variable
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] public float fireRate = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        
         void Shoot() 
         {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
         }
    }
}
 
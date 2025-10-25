using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public float speed;
    private float move;
    private Rigidbody2D rb;
    

    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] public float fireRate = 1f;
    private float nextFire = 0f;

    public Sprite MouthClose;
    public Sprite MouthOpen;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);



        if ((Input.GetMouseButtonDown(0)) && Time.time >= nextFire)
        {
            Shoot();

            nextFire = Time.time + fireRate;
            
        }
        
        
         void Shoot()
        {

            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            gameObject.GetComponent<SpriteRenderer>().sprite = MouthOpen;
          
        }
         
        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = MouthClose;
        }


    }
}
 
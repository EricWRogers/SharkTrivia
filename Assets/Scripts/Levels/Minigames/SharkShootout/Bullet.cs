using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float Speed = 5f;
    public float life = 1f;

    void Start()
    {
        Destroy(gameObject, life);
    }
    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
        Destroy(collision.gameObject);
        Destroy(gameObject);
        } 
    }
}

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

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}

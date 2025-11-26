using UnityEngine;

public class Bullet : MonoBehaviour
{

    
    public float Speed = 5f;
    public float life = 1f;

    // SFX for bullet
    private string popSFXName = "Pop";

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
            Destroy(gameObject);

            // SFX
            AudioManager.instance.PlaySFX(popSFXName);
        } 
    }
}

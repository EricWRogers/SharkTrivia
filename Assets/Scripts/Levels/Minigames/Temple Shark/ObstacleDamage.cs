using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObstacleDamage : MonoBehaviour
{
    public int damage = 1;
    public bool isTrigger = true;
    public bool destroyOnHit = false;

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true; // default to trigger so the shark slides past
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isTrigger || !other.CompareTag("Player")) return;
        ScoreAndLives.Instance?.TakeHit(damage);
        if (destroyOnHit) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTrigger || !collision.collider.CompareTag("Player")) return;
        ScoreAndLives.Instance?.TakeHit(damage);
        if (destroyOnHit) Destroy(gameObject);
    }
}


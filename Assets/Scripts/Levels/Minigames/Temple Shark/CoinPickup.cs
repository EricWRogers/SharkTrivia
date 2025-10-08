using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinPickup : MonoBehaviour
{
    public int value = 1;
    public int spinSpeedY = 90;

    void Update()
    {
        transform.Rotate(0, spinSpeedY * Time.deltaTime, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ScoreAndLives.Instance?.AddScore(value);
        Destroy(gameObject);
    }
}


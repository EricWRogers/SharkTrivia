using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ScoreAndLives.Instance?.TakeHit(1); // lose 1 life and respawn
    }
}


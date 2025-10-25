using UnityEngine;
using TMPro;

public class Collectibles : MonoBehaviour
{
    [SerializeField] int rotationSpeed = 5;
    public int value = 1; 
    [SerializeField] AudioClip collectSound; // Sound to play on collection

    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Use ScoreManager instead of local totalScore
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddPoints(value);
            }
            else
            {
                Debug.LogWarning("ScoreManager instance not found!");
            }

            // Play collection sound
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            
            // Destroy object after collected
            Destroy(gameObject);
        }
    }
}

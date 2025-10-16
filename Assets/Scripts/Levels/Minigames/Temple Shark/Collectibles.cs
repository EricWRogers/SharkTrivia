using UnityEngine;
using System.Collections;
using TMPro;

public class Collectibles : MonoBehaviour
{
    [SerializeField] int rotationSpeed = 5;
    public int value = 1;
    public TMP_Text scoreText; // Temporary
    public static int totalScore = 0; // Shared score across all coins
    [SerializeField] AudioClip collectSound; // Sound to play on collection

    void Start()
    {
        UpdateScoreDisplay();
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    public void AddScore(int points)
    {
        totalScore += points;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddScore(value); // Adds this coin's value to totalScore (temporary)

            // Call something to add the collectible's value
            // Example: ScoreManager.Instance.AddScore(value);

            // Play collection sound
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            
            // Destroy object after collected
            Destroy(gameObject);
        }
    }

}

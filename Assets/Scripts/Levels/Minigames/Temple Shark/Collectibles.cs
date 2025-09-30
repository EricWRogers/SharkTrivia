using UnityEngine;
using System.Collections;
using TMPro;

public class Collectibles : MonoBehaviour
{
    [SerializeField] int rotationSpeed = 5;
    public int value = 1;
    public TMP_Text scoreText; // Temporary
    private int currentScore = 0;

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
        currentScore += points;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddScore(value); // Temporary
            
            // Call something to add the collectible's value
            // Example: ScoreManager.Instance.AddScore(value);

            // Destroy object after collected
            Destroy(gameObject);
        }
    }
}

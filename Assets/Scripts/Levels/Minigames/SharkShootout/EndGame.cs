using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [Header("Settings")]
    public int maxHits = 3; // How many hits before ending the game
    public string endSceneName = "LoseScene"; // Name of your end screen scene

    private int hitCount = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food")) // Customize this tag
        {
            hitCount++;

            Debug.Log("Hit #" + hitCount);

            if (hitCount >= maxHits)
            {
                Lose();
            }
        }

    }

    void Lose()
    {
        Debug.Log("Game Over! Loading end screen...");
        SceneManager.LoadScene(endSceneName);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public WinScreen winScreen;

    [Header("Settings")]
    public int maxHits = 3; 
    public string endSceneName = "LoseScene"; 

    private int hitCount = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
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
        Debug.Log("Game Over!");
        winScreen.DisplayWinResults();

        winScreen.StopGame();
        //LevelManager.LoadBackStage();
    }
}

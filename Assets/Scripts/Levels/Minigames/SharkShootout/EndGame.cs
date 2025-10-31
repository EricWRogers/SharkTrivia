using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public WinScreen winScreen;
    public Timer timer;

    [Header("Settings")]
    public int maxHits = 3; 
    public string endSceneName = "LoseScene";

    private int hitCount = 0;
    public void Update()
    {
        if (!timer.timeRunning)
        {
            winScreen.DisplayWinResults(ScoreManager.instance.score);
        }
    }

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
        //winScreen.DisplayWinResults();

        winScreen.StopGame();
        //LevelManager.LoadBackStage();
    }
}

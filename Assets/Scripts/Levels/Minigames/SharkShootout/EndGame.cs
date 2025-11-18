using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    private TakingDamage takingdamage;
    public WinScreen winScreen;
    public Timer timer;

    [Header("Settings")]
    public int maxHits = 3; 
    public string endSceneName = "LoseScene";

    private int hitCount = 0;

    void Start()
    {
        
            takingdamage = GameObject.FindGameObjectWithTag("Player").GetComponent<TakingDamage>();
            takingdamage.endgame = this;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            hitCount++;

            takingdamage.TakeDamage(hitCount);

            Debug.Log("Hit #" + hitCount);
            if (hitCount >= maxHits)
            {
                winScreen.DisplayLoseResults();
            }

        }

    }

    void Lose()
    {
        Debug.Log("Game Over!");
        winScreen.DisplayLoseResults();
        //winScreen.StopGame();
    }
    public void Win()
    {
        //winScreen.DisplayWinResults(); 
        winScreen.DisplayWinResults(ScoreManager.instance.score);
    }
     
}

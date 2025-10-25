using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishLine : MonoBehaviour
{
    //public GameObject WinScreen;
    public Timer timer;
    public Collectibles collectibles;
    public WinScreen winScreen;

    [Header("Text Elements")]
    public TMP_Text winTimerText; // Text element to display the final time
    public TMP_Text winScoreText; // Text element to display the final score

    [Header("Objects To Deactivate")]
    public MonoBehaviour[] objectsToDeactivate;

    public void ReturnToMain()
    {
        LevelManager.LoadBackStage(); // Loads the main menu
        Time.timeScale = 1f; // Resume the game
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("WIN! You made it to the end.");
        //WinScreen.SetActive(true);

        winScreen.DisplayWinResults();


        Time.timeScale = 0f; // Pause the game

        // Deactivate specified objects
        if (objectsToDeactivate != null)
        {
            foreach (var obj in objectsToDeactivate)
            {
                if (obj != null)
                    obj.enabled = false;
            }
        }

        // Stop the timer
        timer.StopTimer();

        // Update TMPro text with final time
        if (winTimerText != null && timer != null)
        {
            var timerScript = timer.GetComponent<Timer>();
            if (timerScript != null)
            {
                winTimerText.text = $"Time: {timer.GetFormattedTime()}" ;
            }
        }

        // Update stats text
        if (winScoreText != null)
        {
            var collectCount = Collectibles.totalScore;
            winScoreText.text = $"Coins Collected: {collectCount}";
        }
    }
}

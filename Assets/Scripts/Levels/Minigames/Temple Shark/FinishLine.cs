using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public GameObject WinScreen;
    public Timer timer;

    [Header("Text Elements")]
    public TMP_Text timerText;
    public TMP_Text statsText;

    [Header("Objects To Deactivate")]
    public MonoBehaviour[] objectsToDeactivate;

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Kaden's Scene");
        Cursor.visible = true;
        Time.timeScale = 1f; // Resume the game
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("WIN! You made it to the end.");
        WinScreen.SetActive(true);
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
        if (timerText != null && timer != null)
        {
            var timerScript = timer.GetComponent<Timer>();
            if (timerScript != null)
            {
                timerText.text = $"Time: {timer.GetComponentInChildren<TextMeshProUGUI>().text}" ;
            }
        }

        // Update stats text
        /*if (statsText != null)
        {
            var acornCount = FindObjectOfType<AcornCounter>()?.GetAcornCount() ?? 0;
            statsText.text = $"Acorns Collected: {acornCount}";
        }*/
    }
}

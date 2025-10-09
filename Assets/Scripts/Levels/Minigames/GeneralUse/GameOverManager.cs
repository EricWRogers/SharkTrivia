using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject[] objectsToDisable; // Assign ball, pins, lane in Inspector (or anything else you want to disable)

    public GameObject loseScreen;

    public void GameOverShow()
    {
        LoseScreen();
        foreach (var obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    public void BackStage()
    {
        Time.timeScale = 1f;
        LevelManager.LoadBackStage();// Loads the main menu
    }
    public void Quit()
    {
        Application.Quit(); // Quits the build application
    }

    public void LoseScreen()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.visible = true;
    }
}
//WIP
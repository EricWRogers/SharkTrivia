using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject pins;
    public GameObject lane;

    public GameObject loseScreen;

    public void GameOverShow()
    {
        LoseScreen();
        ball.SetActive(false);
        pins.SetActive(false);
        lane.SetActive(false);
    }
    public void Restart()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    public void BackStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("BackStage"); // Loads the main menu
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
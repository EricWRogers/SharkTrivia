using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject pins;
    public GameObject lane;
    
    public void GameOverShow()
    {
        SceneManager.LoadScene("BackStage");
        ball.SetActive(false);
        pins.SetActive(false);
        lane.SetActive(false);
    }
    public void Restart()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    public void MainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("TopScreen"); // Loads the main menu
    }
    public void Quit()
    {
        Application.Quit(); // Quits the build application
    }
}
//WIP
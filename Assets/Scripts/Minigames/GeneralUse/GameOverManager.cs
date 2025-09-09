using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public GameObject goUI;
    
    public void GameOverShow()
    {
        goUI.SetActive(true); // Activate the Game Over UI
        Time.timeScale = 0f; // Pause the game
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
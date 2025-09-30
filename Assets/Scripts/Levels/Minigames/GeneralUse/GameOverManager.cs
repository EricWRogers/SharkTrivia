using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public UnityEvent ActiveSwitch;
    public GameObject LoseScreen;

    public void GameOverShow()
    {
        LoseScreen.SetActive(true);
        ActiveState();
    }

    public void ActiveState()
    {
        ActiveSwitch.Invoke();
    }
    public void Restart()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    public void MainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); // Loads the main menu
    }
    public void BackStage()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("BackStage"); // Loads the backstage
    }
    public void Quit()
    {
        Application.Quit(); // Quits the build application
    }
}
//WIP
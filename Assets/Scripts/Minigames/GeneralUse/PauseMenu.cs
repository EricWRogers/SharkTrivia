using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("Game Objects")]
    public GameObject goUI;
    public GameObject ball;
    public GameObject pins;
    public GameObject lane; 

    [Header("Audio(s):")]
    public AudioSource audioSource;
    public AudioClip pauseSFX;

    [Header("Menu and Script(s):")]
    public GameObject pauseMenu;

    public void Home()
    {
        Debug.Log("Loading Main Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Resume()
    {
        // Activate game objects
        ball.SetActive(true);
        pins.SetActive(true);
        lane.SetActive(true);

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }    
    public void Quit()
    {
        Application.Quit();
        Debug.Log("You've quit the game!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Pause()
    {
        // Deactivate game objects
        ball.SetActive(false);
        pins.SetActive(false);
        lane.SetActive(false);

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Play clip once
        if (pauseSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(pauseSFX);
        }
    }
}

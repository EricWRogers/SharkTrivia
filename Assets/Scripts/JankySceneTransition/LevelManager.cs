using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    private static LevelManager Instance;
    public static bool GameIsPaused = false;
    public static bool allowPause = true;
    [SerializeField] public static string[] miniGameLevels = { "Bowling", "Temple Shark", "MINIGTeethCleaning", "SharkShootout" };
    public static GameObject pauseMenuUI;
    public Animator animator;
    static int oldMinigameNum = 0;
    static string oldSceneName;

/*if youre here because its not loading the right scene copy paste whichever you need
to load the back stage do -- LevelManager.LoadBackStage();
to load the main menu do -- LevelManager.LoadMainMenu();
to load a Random MiniGame do -- LevelManager.LoadRandMiniGame();
to load a specific scene not mentioned do -- LevelManager.LoadSpecificScene("SceneName");*/
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
        pauseMenuUI = Instance.transform.GetChild(0).gameObject;
    }
    public static void LoadBackStage()
    {
        StaticResume();
        Instance.StartCoroutine(Instance.LoadLevel("BackStage"));
    }
    public static void LoadMainMenu()
    {
        StaticResume();
        Instance.StartCoroutine(Instance.LoadLevel("MainMenu"));
    }
    public static void LoadSettingMenu()
    {
        StaticResume();
        Instance.StartCoroutine(Instance.LoadLevel("Settings"));
    }
    public static void LoadAudioMenu()
    {
        StaticResume();
        Instance.StartCoroutine(Instance.LoadLevel("AudioSettings"));
    }
    public static void LoadRandMiniGame()
    {
        StaticResume();
        int newMinigameNum = Random.Range(0, miniGameLevels.Length);
        while (newMinigameNum == oldMinigameNum)
        {
            newMinigameNum = Random.Range(0, miniGameLevels.Length);
        }
        string newGame = miniGameLevels[newMinigameNum];
        oldMinigameNum = newMinigameNum;
        Instance.StartCoroutine(Instance.LoadLevel(newGame));
    }
    public static void LoadSpecificScene(string newScene)
    {
        StaticResume();
        Instance.StartCoroutine(Instance.LoadLevel(newScene));
    }
    public static void ReturnPrevScene()
    {
        StaticResume();
        Instance.StartCoroutine(Instance.LoadLevel(oldSceneName));
    }


    void Update()
    {
        if (!allowPause) return; // Skip pausing if disabled

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }
    public static void StaticResume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1.0f;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1.0f;
    }
    void Paused()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void Quite()
    {
        Application.Quit();
    }
    public static void ReturnToMenu()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;

        Instance.StartCoroutine(Instance.LoadLevel("BackStage"));
        Time.timeScale = 1.0f;
    }

    IEnumerator LoadLevel(string levelName)
    {
        oldSceneName = SceneManager.GetActiveScene().name;
        if (animator != null)
        {
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(levelName);
        animator.SetTrigger("End");
    }
}

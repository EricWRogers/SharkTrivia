using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;       // Identifier
    public AudioClip clip;    // Audio clip
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 3f)]
    public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    public AudioSource musicSource;
    public Sound[] musicClips;

    [Header("SFX")]
    public AudioSource sfxSource;
    public Sound[] sfxClips;

    [Header("UI Buttons")]
    [SerializeField] private string buttonClickSFX = "ButtonClick";

    // Track buttons that already have listeners
    private readonly List<Button> registeredButtons = new List<Button>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded -= OnSceneLoaded; // prevents double registration
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopMusic();

        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic("MenuTheme");
                break;
            case "IntroCutscene":
                PlayMusic("CutsceneTheme");
                break;
            case "TriviaR1":
                PlayMusic("TestTriviaTheme");
                break;
            case "TriviaR2":
                PlayMusic("TestTriviaTheeme");
                break;
            case "TestTriviaLimitedCorrect":
                PlayMusic("TriviaLimitedCorrectTheme");
                break;
            case "TestTriviaLimitedGuesses":
                PlayMusic("TriviaLimitedGuessesTheme");
                break;
            case "TestTriviaTimer":
                PlayMusic("TriviaTimerTheme");
                break;
            case "BackStage":
                PlayMusic("BackStageTheme");
                break;
            case "Bowling":
                PlayMusic("BowlingTheme");
                break;
            case "MINIGTeethCleaning":
                PlayMusic("TeethCleaningTheme");
                break;
            case "SharkShootout":
                PlayMusic("SharkShootoutTheme");
                break;
            case "Temple Shark":
                PlayMusic("TempleSharkTheme");
                break;
            default:
                StopMusic();
                break;
        }

        // Register all buttons that exist at scene load
        RegisterAllButtons();
    }

    private void Update()
    {
        // Dynamically register new buttons that appear during gameplay
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            if (registeredButtons.Contains(btn))
                continue;

            // Exclude prefabs named "EnterJournal" or "Exit"
            if (btn.gameObject.name == "EnterJournal" || btn.gameObject.name == "Exit")
                continue;

            btn.onClick.AddListener(() => PlaySFX(buttonClickSFX));
            registeredButtons.Add(btn);
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = System.Array.Find(musicClips, s => s.name == name);
        if (sound != null)
        {
            musicSource.Stop();
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume;
            musicSource.pitch = sound.pitch;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music not found: " + name);
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = System.Array.Find(sfxClips, s => s.name == name);
        if (sound != null)
        {
            sfxSource.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }

    public void StopSFX()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }

    // Register all buttons present at scene load, excluding certain prefabs
    private void RegisterAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            if (registeredButtons.Contains(btn))
                continue;

            if (btn.gameObject.name == "EnterJournal" || btn.gameObject.name == "Exit")
                continue;

            btn.onClick.AddListener(() => PlaySFX(buttonClickSFX));
            registeredButtons.Add(btn);
        }
    }
}
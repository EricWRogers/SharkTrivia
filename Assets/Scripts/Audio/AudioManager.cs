using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

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
            case "Level1":
                PlayMusic("Level1Theme");
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

            default:
                StopMusic();
                break;
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = System.Array.Find(musicClips, s => s.name == name);
        if (sound != null)
        {
            // Always switch music when a scene loads
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

    // ⬅ ADDED: new method to stop music
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
}
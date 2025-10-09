using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Mixer")]
    public AudioMixer masterMixer;

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

    //Called when Slider is moved
    void musicVolume(float sliderValue)
    {
        musicSource.volume = sliderValue;
    }

    // Change the music volumes
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    // Mute button
    public void ToggleMusicMute(bool isMuted)
    {
        if (isMuted)
        {
            masterMixer.SetFloat("MusicVolume", -80f); // Mute
        }
        else
        {
            // Set to a default or previously saved volume
            masterMixer.SetFloat("MusicVolume", 0f);
        }
    }

    // Mute button
    public void ToggleSFXMute(bool isMuted)
    {
        if (isMuted)
        {
            masterMixer.SetFloat("SFXVolume", -80f); // Mute
        }
        else
        {
            // Set to a default or previously saved volume
            masterMixer.SetFloat("SFXVolume", 0f);
        }
    }
}
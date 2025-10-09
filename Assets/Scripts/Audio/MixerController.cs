using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class MixerController : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer masterMixer;
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

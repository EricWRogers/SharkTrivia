using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;


public class VideoPlayerScript : MonoBehaviour
{
    public VideoClip videoToPlay;
    public RawImage rawImageDisplay;
    public float playDuration = 5f; // Set the desired duration in seconds
    public List<VideoClip> videoClips; // Assign VideoClip assets here
    private VideoPlayer videoPlayer;
    private int currentVideoIndex = 0;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        // Set the video clip
        videoPlayer.clip = videoToPlay;
    }
    //Select the video to play
    public void PlayVideoClip(int index)
    {
        if (index >= 0 && index < videoClips.Count)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClips[index];
            PlayVideoAndStop();
            currentVideoIndex = index;
        }
        else
        {
            Debug.LogWarning("Invalid video clip index.");
        }
    }
    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
    }
    IEnumerator PlayVideoAndStop()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(playDuration);
        videoPlayer.Stop();
    }
}
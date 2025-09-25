using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;


public class VideoPlayerScript : MonoBehaviour
{
    public VideoClip videoToPlay;
    public RawImage rawImageDisplay;
    public BowlingManager bowlingManager;
    public float playDuration = 5f; // Set the desired duration in seconds
    public List<VideoClip> videoClips;
    private VideoPlayer videoPlayer;
    private bool isPlaying = false;
    public int currentVideoIndex = 0;

    void Start()
    {
        // Initialize VideoPlayer component if one doesn't exist
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
            Debug.Log("VideoPlayer component added.");
        }
    }
    //Select the video to play
    public void SelectVideoClip(int index)
    {
        // Ensure the index is within bounds
        if (index >= 0 && index < videoClips.Count)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClips[index];
            currentVideoIndex = index;
            Debug.Log("Video Clip" + index + "Loaded");
        }
        else
        {
            Debug.LogWarning("Invalid video clip index.");
        }
    }
    // Play the video for a set duration then reset to clear texture
    public IEnumerator PlayVideoAndStop()
    {
        rawImageDisplay.gameObject.SetActive(true);
        isPlaying = true;
        videoPlayer.Play();
        yield return new WaitForSeconds(playDuration);
        videoPlayer.Stop();
        Debug.Log("Video Stopped");
        isPlaying = false;
        // Clear the RenderTexture when the object is disabled
        if (isPlaying == false)
        {
            rawImageDisplay.gameObject.SetActive(false);
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
            Debug.Log("RenderTexture cleared.");
        }
    }
}

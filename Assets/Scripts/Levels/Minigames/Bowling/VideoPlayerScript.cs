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
    public CanvasPathFollwer canvasPathFollwer;
    public RenderTexture renderTexture;
    public float playDuration = 5f; // Desired duration in seconds for the video
    public List<VideoClip> videoClips; // List of Video Clips
    public int currentVideoIndex = 0;
    public bool isPlaying = false;
    private VideoPlayer videoPlayer;
    
    
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
    // Play the video and stop after a set duration if at the endpoint
    public IEnumerator PlayVideoAndStop()
    {
        // Activate the RawImage to display the video
        rawImageDisplay.gameObject.SetActive(true);
        isPlaying = true;
        // Play the video for 5 seconds/Whatever the duration wanted is then stop.
        videoPlayer.Play();
        yield return new WaitForSeconds(playDuration);
        videoPlayer.Stop();
        // Log when the video stops
        Debug.Log("Video Stopped");
        isPlaying = false;
        // Clear the RenderTexture when the object is disabled
        if (isPlaying == false)
        {
            ClearOutRenderTexture(renderTexture);
            Debug.Log("RenderTexture cleared.");
            canvasPathFollwer.ResetPath();
        }
    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
}


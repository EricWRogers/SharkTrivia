using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;


public class VideoPlayerScript : MonoBehaviour
{
    public VideoClip videoToPlay;
    public RawImage rawImageDisplay;
    public List<VideoClip> videoClips; // Assign VideoClip assets here
    private List<string> videoNames; // For ease of referencing the videos
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;
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

        // If displaying on a RawImage, set up the Render Texture
        if (rawImageDisplay != null)
        {
            // Create a new Render Texture if one doesn't exist
            if (renderTexture == null)
            {
                renderTexture = new RenderTexture(1920, 1080, 24); // Adjust resolution as needed
            }
            videoPlayer.targetTexture = renderTexture;
            rawImageDisplay.texture = renderTexture;
        }
        else // If not using RawImage, render to a material on a 3D object
        {
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        }

        // Prepare the video player
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared; 
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // Start playing the video once prepared
        videoPlayer.Play();
    }

    public void SelectVideo()
    {
        
    }
    //Select the video to play
    public void PlayVideoClip(int index)
    {
        if (index >= 0 && index < videoClips.Count)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
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
}
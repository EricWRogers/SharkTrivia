using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoClip videoToPlay;
    public RawImage rawImageDisplay;
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    void Start()
    {
        // Get or add the VideoPlayer component
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
            // Ensure the VideoPlayer is set to render to a Mesh Renderer
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            // You would typically assign a material to a MeshRenderer on this GameObject
            // and the VideoPlayer would automatically update its texture.
        }

        // Prepare the video player
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared; // Subscribe to the prepare completed event
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // Start playing the video once prepared
        videoPlayer.Play();
    }

    // Example functions for controlling playback
    public void PlayVideo()
    {
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.Prepare(); // Prepare if not already
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
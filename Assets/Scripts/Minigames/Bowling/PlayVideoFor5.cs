using UnityEngine;
using UnityEngine.Video;
using System.Collections;
public class PlayVideoFor5 : MonoBehaviour

{
    public VideoPlayer videoPlayer; // Assign this in the Inspector
    public float playDuration = 5f; // Set the desired duration in seconds

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                    Debug.LogError("VideoPlayer component not found on this GameObject or assigned in Inspector.");
                    return;
            }
        }

        StartCoroutine(PlayVideoAndStop());
    }

    IEnumerator PlayVideoAndStop()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(playDuration);
        videoPlayer.Stop();
    }
}

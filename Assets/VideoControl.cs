using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public MenuToggleScript menu;
    public GameObject screen;
    private SpriteRenderer sr;

    void Start()
    {
        // Get the VideoPlayer component if not assigned in the Inspector
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Attach a callback for when the video completes
        videoPlayer.loopPointReached += OnVideoEnd;
        //screen.SetActive(false);
        sr = screen.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void PlayPauseVideo()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        //screen.SetActive(false);
        sr.enabled = false;
        Debug.Log("Video has ended!");
    }

    public void Play()
    {
        menu.DisableMenu();
        //screen.SetActive(true);
        sr.enabled = true;
        videoPlayer.Play();
    }
}

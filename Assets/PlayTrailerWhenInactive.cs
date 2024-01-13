using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Video;

public class PlayTrailerWhenInactive : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private float countdownTimer;
    public float timeLimitSeconds = 120f;
    public GameObject square;

    void Start()
    {
        // Get the VideoPlayer component if not assigned in the Inspector
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Attach a callback for when the video completes
        videoPlayer.loopPointReached += OnVideoEnd;
        square.SetActive(false);
        countdownTimer = timeLimitSeconds;
        videoPlayer.isLooping = true;
    }

    void Update()
    {
        // Check for mouse/cursor movement
        if (Mouse.current.delta.ReadValue() != Vector2.zero || Input.anyKeyDown)
        {
            MovementDetected();
        }

        // Update and check the countdown timer
        UpdateTimer();
    }

    void UpdateTimer()
    {
        // Update the countdown timer
        countdownTimer -= Time.deltaTime;
        Debug.Log("Countdown: " + countdownTimer);

        // Check if the countdown timer has reached zero
        if (countdownTimer <= 0f)
        {
            Debug.Log("Countdown timer reached zero!");
            ResetTimer();
            // TIMER MUST BE LONGER THAN TRAILER!!!

            // PLAY TRAILER
            PlayTrailer();            
        }
    }

    private void MovementDetected()
    {
        ResetTimer();
        videoPlayer.Stop();
        square.SetActive(false);
    }

    void ResetTimer()
    {
        // Reset the countdown timer to 3 minutes
        countdownTimer = timeLimitSeconds;
    }

    //public void PlayPauseVideo()
    //{
    //    if (videoPlayer.isPlaying)
    //        videoPlayer.Pause();
    //    else
    //        videoPlayer.Play();
    //}

    private void PlayTrailer()
    {
        Debug.Log("videoPlayer time: " + videoPlayer.time);
        ResetTimer();

        // Play the video
        square.SetActive(true);
        videoPlayer.Play();
        // Set the time to 0 (beginning of the clip)
        videoPlayer.time = 0;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.Stop();
        Debug.Log("Video has ended!");
        square.SetActive(false);
        ResetTimer();
    }
}

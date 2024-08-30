using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Video;

public class PlayTrailerWhenInactive : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private float countdownTimer;
    public float timeLimitSeconds;
    private SpriteRenderer square;

    void Start()
    {
        if (timeLimitSeconds <= 5)
            timeLimitSeconds = 5;
        videoPlayer = GetComponent<VideoPlayer>();
        square = GetComponent<SpriteRenderer>();
        // Attach a callback for when the video completes
        videoPlayer.loopPointReached += OnVideoEnd;
        square.enabled = false;
        countdownTimer = timeLimitSeconds;
        videoPlayer.isLooping = true;
        videoPlayer.Prepare();
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
        if (countdownTimer <= 0f && !videoPlayer.isPlaying)
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
        square.enabled = false;
        GameManager.Instance.UnpauseBGMusic();
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
        GameManager.Instance.PauseBGMusic();
        Debug.Log("videoPlayer time: " + videoPlayer.time);
        ResetTimer();

        // Play the video
        square.enabled = true;
        videoPlayer.Play();
        // Set the time to 0 (beginning of the clip)
        videoPlayer.time = 0;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        GameManager.Instance.UnpauseBGMusic();
        videoPlayer.Stop();
        Debug.Log("Video has ended!");
        square.enabled = false;
        ResetTimer();
    }
}

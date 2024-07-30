using UnityEngine;
using UnityEngine.Video;

public class AudioLoopController : MonoBehaviour
{
    private AudioSource audioSource;
    public VideoPlayer videoPlayer;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        //if (videoPlayer == null)
        //{
        //    Debug.LogError("VideoPlayer component not assigned!");
        //    return;
        //}
        //videoPlayer.loopPointReached += OnVideoEnd;

        // Start looping the audio
        PlayAudioLoop();
    }

    //void OnVideoEnd(VideoPlayer vp)
    //{
    //    Debug.Log("Video has ended!");
    //    PlayAudioLoop();
    //}

    //void Update()
    //{
    //    // Check if the VideoPlayer is currently playing
    //    if (videoPlayer.isPlaying)
    //    {
    //        // Pause the audio when the video is playing
    //        PauseAudio();
    //    }
    //    else
    //    {
    //        // Resume or start the audio loop when the video is not playing
    //        if (!audioSource.isPlaying)
    //        {
    //            PlayAudioLoop();
    //        }
    //    }
    //}

    void PlayAudioLoop()
    {
        // Set the audio to loop and play
        audioSource.loop = true;
        audioSource.Play();
    }

    //void PauseAudio()
    //{
    //    // Pause the audio playback
    //    if (audioSource.isPlaying)
    //        audioSource.Pause();
    //}
}

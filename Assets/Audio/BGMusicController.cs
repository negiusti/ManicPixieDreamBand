using System.Collections.Generic;
using UnityEngine;

public class BGMusicController : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private AudioSource audioSource;
    private int currentClipIndex = -1;

    //public static BGMusicController Instance;
    private bool isPaused;
    private bool isFocused;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        audioSource = GetComponent<AudioSource>();
    //        //if (audioSource == null)
    //        //{
    //        //    audioSource = gameObject.AddComponent<AudioSource>();
    //        //}
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomClip();
    }

    private void OnDestroy()
    {
        audioSource.Stop();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !isPaused && isFocused)
        {
            PlayRandomClip();
        }
        if (audioSource.isPlaying && isPaused)
        {
            Debug.Log("What the fuck");
            PauseAudio();
        }
            
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocused = focus;
    }

    private void PlayRandomClip()
    {
        if (audioClips.Count == 0)
            return;

        int newClipIndex;
        do
        {
            newClipIndex = Random.Range(0, audioClips.Count);
        } while (newClipIndex == currentClipIndex);
        currentClipIndex = newClipIndex;
        Debug.Log("Playing: " + audioClips[currentClipIndex].name);

        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();
    }


    public void PauseAudio()
    {
        Debug.Log("Pause audio");
        isPaused = true;
        audioSource.Pause();
    }

    public void UnpauseAudio()
    {
        Debug.Log("Unpause audio");
        audioSource.UnPause();
        isPaused = false;
    }
}

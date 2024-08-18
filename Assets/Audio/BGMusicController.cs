using System.Collections.Generic;
using UnityEngine;

public class BGMusicController : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private AudioSource audioSource;
    private int currentClipIndex = -1;

    private static BGMusicController instance;
    private bool isPaused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayRandomClip();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !isPaused)
        {
            PlayRandomClip();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        isPaused = !focus;
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
        audioSource.Pause();
        isPaused = true;
    }

    public void UnpauseAudio()
    {
        audioSource.UnPause();
        isPaused = false;
    }
}

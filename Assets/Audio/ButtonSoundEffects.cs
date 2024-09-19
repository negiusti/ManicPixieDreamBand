using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffects : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private AudioSource audioSource;
    private int currentClipIndex = -1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
    }

    public void PlayRandomClip()
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrator : MonoBehaviour
{
    private AudioSource audioSource; // Assign your AudioSource in the Inspector
    public AudioClip audioClip; // Assign your AudioClip in the Inspector
    public List<float> beatTimes; // List of beat times in seconds
    private float lagAvg = 0f;
    private float lagRunningSum = 0f;

    private int currentBeatIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (isPlaying)
        {
            DetectInput();
        }
    }

    void StartGame()
    {
        audioSource.Play();
        isPlaying = true;
        currentBeatIndex = 0;
    }

    void DetectInput()
    {
        if (currentBeatIndex < beatTimes.Count)
        {
            float currentTime = audioSource.time;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float inputTime = currentTime;
                float beatTime = beatTimes[currentBeatIndex];

                float lag = inputTime - beatTime;
                lagRunningSum += lag;
                lagAvg = lagRunningSum / (currentBeatIndex + 1);

                Debug.Log("Beat Time: " + beatTime + ", Input Time: " + inputTime + ", Lag: " + lag);

                currentBeatIndex++;
            }

            // Automatically move to the next beat if we have passed the current beat time
            if (currentTime > beatTimes[currentBeatIndex])
            {
                currentBeatIndex++;
            }
        }
        else
        {
            // Stop the game or loop
            isPlaying = false;
            audioSource.Stop();
        }
    }
}

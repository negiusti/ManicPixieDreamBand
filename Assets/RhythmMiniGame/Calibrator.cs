using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class Calibrator : MonoBehaviour
{
    private AudioSource audioSource; // Assign your AudioSource in the Inspector
    private List<float> beatTimes = new List<float>(); // List of beat times in seconds
    private float lagAvg = 0f;
    private float lagRunningSum = 0f;

    private int currentBeatIndex = 0;
    private bool isPlaying = false;
    private bool ready;
    private void OnLoadCompleted(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string timez = obj.Result.text;
            foreach (string s in timez.Split(new char[] { '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
                try
                {
                    beatTimes.Add(float.Parse(s));
                }
                catch (FormatException)
                {
                    Debug.LogError("Invalid float format.");
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load file. Error: " + obj.OperationException);
        }
        Addressables.Release(obj);
        Debug.Log("HELLO" + beatTimes);
        ready = true;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        string addressablePath = "Assets/RhythmGameNotes/Metronome/metronome.txt";

        // Load the text file asynchronously
        AsyncOperationHandle<TextAsset> asyncOperation = Addressables.LoadAssetAsync<TextAsset>(addressablePath);

        // Register a callback for when the load operation completes
        asyncOperation.Completed += OnLoadCompleted;
    }

    void Update()
    {
        if (!isPlaying && ready && Input.GetKeyDown(KeyCode.Return))
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

                Debug.Log("Beat Time: " + beatTime + ", Input Time: " + inputTime + ", Lag: " + lag + ", lagAvg: " + lagAvg);

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
            //MinCloseMiniGame()
        }
    }
}

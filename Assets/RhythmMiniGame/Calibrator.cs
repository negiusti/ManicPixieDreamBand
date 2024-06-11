using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class Calibrator : MonoBehaviour
{
    private AudioSource audioSource; // Assign your AudioSource in the Inspector
    private List<float> beatTimes = new List<float>(); // List of beat times in seconds
    private List<float> lagVals = new List<float>();
    private CalibrationMiniGame mg;

    private int currentBeatIndex = 0;
    private bool isPlaying = false;
    private bool assetsLoaded;
    private bool ready;

    private void OnDisable()
    {
        ready = false;
    }

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
        assetsLoaded = true;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        string addressablePath = "Assets/RhythmGameNotes/Metronome/Metronome.txt";

        // Load the text file asynchronously
        AsyncOperationHandle<TextAsset> asyncOperation = Addressables.LoadAssetAsync<TextAsset>(addressablePath);

        // Register a callback for when the load operation completes
        asyncOperation.Completed += OnLoadCompleted;
        mg = GetComponentInParent<CalibrationMiniGame>();
    }

    public void Begin()
    {
        ready = true;
    }

    void Update()
    {
        if (!isPlaying && assetsLoaded && ready)
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

            if (currentBeatIndex + 1 < beatTimes.Count && currentTime >= beatTimes[currentBeatIndex +1])
            {
                Debug.Log("Missed a beat");
                currentBeatIndex++;
            } else if (Input.GetKeyDown(KeyCode.Space))
            {
                float inputTime = currentTime;
                float beatTime = beatTimes[currentBeatIndex];

                float lag = inputTime - beatTime;
                lagVals.Add(lag);

                Debug.Log("Beat Time: " + beatTime + ", Input Time: " + inputTime + ", Lag: " + lag);

                currentBeatIndex++;
            }
        }
        else
        {
            // Stop the game or loop
            lagVals.Sort();
            int medianIdx = lagVals.Count / 2;
            Debug.Log("med lag Time: " + lagVals[medianIdx]);
            ES3.Save("LagCalibration", lagVals[medianIdx]);
            isPlaying = false;
            audioSource.Stop();
            mg.CloseMiniGame();
        }
    }
}

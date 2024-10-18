using UnityEngine;

public class PauseAudioWhenStationary : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource; // Reference to the AudioSource component
    private Vector3 lastPosition;   // Store the last position of the GameObject
    private float stationaryTime;   // Timer to track how long the GameObject has been stationary
    public float stationaryThreshold = 0.5f; // Time in seconds before pausing the audio
    public float movementThreshold = 0.01f;  // Minimum movement threshold to detect movement
    public bool onlyWhenMouseDown;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Check if the object has moved
        if (Vector3.Distance(transform.position, lastPosition) > movementThreshold)
        {
            // Object is moving
            stationaryTime = 0f; // Reset the stationary timer

            if (!audioSource.isPlaying && (!onlyWhenMouseDown || Input.GetMouseButton(0)))
            {
                audioSource.UnPause(); // Unpause the audio if it was paused
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
        }
        else
        {
            // Object is not moving
            stationaryTime += Time.deltaTime;

            // If the object has been stationary for more than the threshold, pause the audio
            if (stationaryTime > stationaryThreshold && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        // Update the last position for the next frame
        lastPosition = transform.position;
    }
}

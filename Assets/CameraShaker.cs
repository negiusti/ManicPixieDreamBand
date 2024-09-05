using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float shakeDuration = 0.3f;  // Duration of the camera shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    private Vector3 originalPos;         // Store the original position of the camera
    private float shakeTimeRemaining = 0f;

    void Start()
    {
        originalPos = transform.localPosition;  // Get the initial position of the camera
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;

            if (shakeTimeRemaining <= 0f)
            {
                shakeTimeRemaining = 0f;
                transform.localPosition = originalPos; // Reset to the original position
            }
        }
    }

    public void CameraShake()
    {
        shakeTimeRemaining = shakeDuration;  // Start shaking for the set duration
    }
}

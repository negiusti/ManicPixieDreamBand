using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject background; // Reference to the background GameObject
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement

    private float minX, maxX; // Minimum and maximum x positions for the camera

    void Start()
    {
        if (background != null)
        {
            // Calculate the camera bounds based on the background's collider
            Bounds backgroundBounds = background.GetComponent<Collider2D>().bounds;
            Camera mainCamera = Camera.main;

            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            minX = backgroundBounds.min.x + cameraHalfWidth;
            maxX = backgroundBounds.max.x - cameraHalfWidth;
        }
        else
        {
            Debug.LogError("Background GameObject not assigned.");
        }
    }

    void LateUpdate()
    {
        // Get the current position of the GameObject
        Vector3 desiredPosition = new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, smoothSpeed);
        Camera.main.transform.position = smoothedPosition;

        // Clamp the camera position to stay within the defined bounds
        Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, minX, maxX),
                                                     Camera.main.transform.position.y,
                                                     Camera.main.transform.position.z);
    }
}
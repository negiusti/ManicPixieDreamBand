using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject background; // Reference to the background GameObject
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    private Bounds backgroundBounds;
    private Camera mainCamera;
    private float minX, maxX; // Minimum and maximum x positions for the camera

    void Start()
    {
        if (background != null)
        {
            // Calculate the camera bounds based on the background's collider
            backgroundBounds = background.GetComponent<Collider2D>().bounds;
            mainCamera = Camera.main;
            Follow();
        }
        else
        {
            Debug.LogError("Background GameObject not assigned.");
        }
    }

    private void Follow()
    {
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        minX = backgroundBounds.min.x + cameraHalfWidth;
        maxX = backgroundBounds.max.x - cameraHalfWidth;

        // Get the current position of the GameObject
        Vector3 desiredPosition = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);
        mainCamera.transform.position = smoothedPosition;

        // Clamp the camera position to stay within the defined bounds
        mainCamera.transform.position = new Vector3(Mathf.Clamp(mainCamera.transform.position.x, minX, maxX),
                                                     mainCamera.transform.position.y,
                                                     mainCamera.transform.position.z);
    }

    void LateUpdate()
    {
        Follow();
    }
}
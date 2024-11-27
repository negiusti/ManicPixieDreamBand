using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject background; // Reference to the background GameObject
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public GameObject whoToFollow;
    private Bounds backgroundBounds;
    private Camera cam;
    private float minX, maxX, minY; // Minimum and maximum x positions for the camera

    void Start()
    {
        if (background != null)
        {
            // Calculate the camera bounds based on the background's collider
            backgroundBounds = background.GetComponent<Collider2D>().bounds;
            cam = this.GetComponent<Camera>();
            Follow();
        }
        else
        {
            Debug.LogError("Background GameObject not assigned.");
        }
    }

    private void Follow()
    {
        float cameraHalfWidth = cam.orthographicSize * cam.aspect;
        minX = backgroundBounds.min.x + cameraHalfWidth;
        maxX = backgroundBounds.max.x - cameraHalfWidth;
        minY = backgroundBounds.min.y + cam.orthographicSize;
        // Get the current position of the GameObject
        Vector3 desiredPosition = new Vector3(whoToFollow.transform.position.x, cam.transform.position.y, cam.transform.position.z);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(cam.transform.position, desiredPosition, smoothSpeed);
        cam.transform.position = smoothedPosition;

        // Clamp the camera position to stay within the defined bounds
        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x, minX, maxX),
                                                     minY,///cam.transform.position.y,
                                                     cam.transform.position.z);
    }

    void LateUpdate()
    {
        Follow();
    }
}
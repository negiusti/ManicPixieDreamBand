using UnityEngine;

public class FollowMouseWithinBounds : MonoBehaviour
{
    public BoxCollider2D boundsCollider; // The 2D Box Collider representing the movement bounds

    private Camera cam; // The camera component attached to this GameObject
    private Vector3 minBounds; // Minimum bounds of the box collider
    private Vector3 maxBounds; // Maximum bounds of the box collider

    void Start()
    {
        // Get the Camera component attached to this GameObject
        cam = GetComponent<Camera>();

        if (boundsCollider == null)
        {
            Debug.LogError("Please assign a BoxCollider2D for bounds.");
            return;
        }

        // Calculate the bounds of the BoxCollider2D in world space
        minBounds = boundsCollider.bounds.min;
        maxBounds = boundsCollider.bounds.max;
    }

    void Update()
    {
        if (cam == null || boundsCollider == null) return;

        // Get the mouse position in world space
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Make sure to keep the z-axis constant in 2D

        // Clamp the mouse position within the bounds of the BoxCollider2D
        float clampedX = Mathf.Clamp(mousePos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(mousePos.y, minBounds.y, maxBounds.y);

        // Set the GameObject's position to the clamped position
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    //private void Follow()
    //{
    //    float cameraHalfWidth = cam.orthographicSize * cam.aspect;
    //    minX = backgroundBounds.min.x + cameraHalfWidth;
    //    maxX = backgroundBounds.max.x - cameraHalfWidth;

    //    // Get the current position of the GameObject
    //    Vector3 desiredPosition = new Vector3(whoToFollow.transform.position.x, cam.transform.position.y, cam.transform.position.z);

    //    // Smoothly move the camera towards the desired position
    //    Vector3 smoothedPosition = Vector3.Lerp(cam.transform.position, desiredPosition, smoothSpeed);
    //    cam.transform.position = smoothedPosition;

    //    // Clamp the camera position to stay within the defined bounds
    //    cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x, minX, maxX),
    //                                                 cam.transform.position.y,
    //                                                 cam.transform.position.z);
    //}
}

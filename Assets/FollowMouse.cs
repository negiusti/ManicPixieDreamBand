using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Camera mainCamera; // Assign the main camera in the Inspector
    public float followSpeed = 2f; // Speed at which the GameObject follows the mouse

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // If no camera is assigned, use the main camera
        }
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition; // Get the mouse position in screen coordinates
        mousePosition.z = mainCamera.nearClipPlane; // Ensure the mouse position is in the same plane as the camera

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // Convert the mouse position to world coordinates

        // Optionally smooth the movement
        transform.position = Vector3.Lerp(transform.position, worldPosition, followSpeed * Time.deltaTime);
    }
}

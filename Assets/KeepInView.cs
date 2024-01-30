using UnityEngine;

public class KeepInView : MonoBehaviour
{
    private RectTransform rectTransform;
    private float originalX;
    private float offset;
    public float movementSpeed = 5f;

    void Start()
    {
        // Get the RectTransform component of the object
        rectTransform = GetComponent<RectTransform>();
        // Store the original x position
        originalX = rectTransform.position.x;//rectTransform.TransformPoint(rectTransform.anchoredPosition).x;
    }

    void Update()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found. Make sure the main camera is tagged as 'MainCamera'.");
            return;
        }

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not found. Make sure the object has a RectTransform component.");
            return;
        }

        // Move the RectTransform to stay within the camera view on the x-axis
        MoveWithinCameraView();
    }

    void MoveWithinCameraView()
    {
        Vector3 minWorldPos = rectTransform.TransformPoint(new Vector3(rectTransform.rect.xMin, 0, 0));
        Vector3 maxWorldPos = rectTransform.TransformPoint(new Vector3(rectTransform.rect.xMax, 0, 0));
        offset = (maxWorldPos.x - minWorldPos.x);
        offset = (rectTransform.rect.width * rectTransform.lossyScale.x)/2;

        // Calculate the target x position to stay within the camera view
        float targetX = Mathf.Clamp(originalX, Camera.main.ScreenToWorldPoint(Vector3.zero).x + offset, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - offset);
        float currentX = rectTransform.position.x;
        // Interpolate towards the target x position to create smooth movement
        float newX = Mathf.Lerp(currentX, targetX, Time.deltaTime * movementSpeed);

        // Update the RectTransform's x position
        rectTransform.position = new Vector3(newX, rectTransform.position.y);
    }
}
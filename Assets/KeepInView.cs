using UnityEngine;

public class KeepInView : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform rectTransform;
    private float originalX;
    private float offset;
    public float movementSpeed = 5f;

    void Start()
    {
        // Assuming the main camera is tagged as "MainCamera"
        mainCamera = Camera.main;

        // Get the RectTransform component of the object
        rectTransform = GetComponent<RectTransform>();

        // Store the original x position
        originalX = rectTransform.anchoredPosition.x;

        Vector3 minWorldPos = rectTransform.TransformPoint(new Vector3(rectTransform.rect.xMin, 0, 0));
        Vector3 maxWorldPos = rectTransform.TransformPoint(new Vector3(rectTransform.rect.xMax, 0, 0));
        offset = (maxWorldPos.x - minWorldPos.x) / 2;
    }

    void Update()
    {
        if (mainCamera == null)
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
        // Calculate the target x position to stay within the camera view
        float targetX = Mathf.Clamp(originalX, mainCamera.ScreenToWorldPoint(Vector3.zero).x + offset, mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - offset);

        // Interpolate towards the target x position to create smooth movement
        float newX = Mathf.Lerp(rectTransform.anchoredPosition.x, targetX, Time.deltaTime * movementSpeed);

        // Update the RectTransform's x position
        rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
    }
}
//    private Camera mainCamera;
//    private RectTransform rectTransform;
//    private float originalXPos;


//    void Start()
//    {
//        // Assuming the main camera is tagged as "MainCamera"
//        mainCamera = Camera.main;
//        rectTransform = GetComponent<RectTransform>();
//        originalXPos = rectTransform.position.x;
//    }

//    void Update()
//    {
//        if (mainCamera == null)
//        {
//            Debug.LogError("Main camera not found. Make sure the main camera is tagged as 'MainCamera'.");
//            return;
//        }

//        if (rectTransform == null)
//        {
//            Debug.LogError("RectTransform not found. Make sure the object has a RectTransform component.");
//            return;
//        }

//        Vector3[] corners = new Vector3[4];
//        rectTransform.GetWorldCorners(corners);

//        float xMin = float.MaxValue;
//        float xMax = float.MinValue;

//        for (int i = 0; i < 4; i++)
//        {
//            float screenX = mainCamera.WorldToScreenPoint(corners[i]).x;
//            xMin = Mathf.Min(xMin, screenX);
//            xMax = Mathf.Max(xMax, screenX);
//        }

//        // Check if any part of the x-axis bounds of the RectTransform is outside the camera view
//        if (!IsObjectInView())
//        {
//            Debug.Log("Any part of the x-axis bounds of the RectTransform is outside the view of the main camera!");
//            if (ShouldMoveToTheLeft())
//            {
//                rectTransform.Translate(new Vector3(1f, 0f, 0f));
//            } else
//            {
//                rectTransform.Translate(new Vector3(1f, 0f, 0f));
//            }
//        }
//    }

//    bool IsObjectInView()
//    {
//        // Calculate the x-axis bounds in screen space manually
//        Vector3[] corners = new Vector3[4];
//        rectTransform.GetWorldCorners(corners);

//        float xMin = float.MaxValue;
//        float xMax = float.MinValue;

//        for (int i = 0; i < 4; i++)
//        {
//            float screenX = mainCamera.WorldToScreenPoint(corners[i]).x;
//            xMin = Mathf.Min(xMin, screenX);
//            xMax = Mathf.Max(xMax, screenX);
//        }

//        // Check if any part of the x-axis bounds of the RectTransform is within the camera's view
//        return xMin >= mainCamera.pixelRect.x && xMax <= mainCamera.pixelRect.xMax;
//    }

//    bool ShouldMoveToTheLeft()
//    {
//        // Calculate the x-axis bounds in screen space manually
//        Vector3[] corners = new Vector3[4];
//        rectTransform.GetWorldCorners(corners);

//        float xMin = float.MaxValue;
//        float xMax = float.MinValue;

//        for (int i = 0; i < 4; i++)
//        {
//            float screenX = mainCamera.WorldToScreenPoint(corners[i]).x;
//            xMin = Mathf.Min(xMin, screenX);
//            xMax = Mathf.Max(xMax, screenX);
//        }

//        // Check if any part of the x-axis bounds of the RectTransform is within the camera's view
//        return xMin > mainCamera.pixelRect.x;
//    }
//}

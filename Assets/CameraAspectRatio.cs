using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    //// Set this to your target aspect ratio, eg. (16, 9) or (4, 3).
    //public Vector2 targetAspect = new Vector2(16, 9);
    //Camera _camera;

    //void Start()
    //{
    //    _camera = GetComponent<Camera>();
    //    UpdateCrop();
    //}

    //// Call this method if your window size or target aspect change.
    //public void UpdateCrop()
    //{
    //    // Determine ratios of screen/window & target, respectively.
    //    float screenRatio = Screen.width / (float)Screen.height;
    //    float targetRatio = targetAspect.x / targetAspect.y;

    //    if (Mathf.Approximately(screenRatio, targetRatio))
    //    {
    //        // Screen or window is the target aspect ratio: use the whole area.
    //        _camera.rect = new Rect(0, 0, 1, 1);
    //    }
    //    else if (screenRatio > targetRatio)
    //    {
    //        // Screen or window is wider than the target: pillarbox.
    //        float normalizedWidth = targetRatio / screenRatio;
    //        float barThickness = (1f - normalizedWidth) / 2f;
    //        _camera.rect = new Rect(barThickness, 0, normalizedWidth, 1);
    //    }
    //    else
    //    {
    //        // Screen or window is narrower than the target: letterbox.
    //        float normalizedHeight = screenRatio / targetRatio;
    //        float barThickness = (1f - normalizedHeight) / 2f;
    //        _camera.rect = new Rect(0, barThickness, 1, normalizedHeight);
    //    }
    //}

    //private void Update()
    //{
    //    UpdateCrop();
    //}


    // The desired aspect ratio (16:9)
    private float targetAspect = 16.0f / 9.0f;
    private float screenWidth;
    private float screenHeight;

    // Reference to the camera component
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        SetCameraSize();
        //StartCoroutine(UpdateCameraViewport());
    }

    private void OnEnable()
    {
        if (cam == null)
            Start();
        SetCameraSize();
    }

    void Update()
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            SetCameraSize();
            //StartCoroutine(UpdateCameraViewport());
        }
    }
    //private IEnumerator UpdateCameraViewport()
    //{
    //    yield return new WaitForEndOfFrame();
    //    cam.enabled = false;
    //    SetCameraSize();
    //    yield return new WaitForEndOfFrame();
    //    cam.enabled = true;
    //}

    private void SetCameraSize()
    {

        // Get the current screen aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Calculate the scaling factor to maintain the target aspect ratio
        float scaleHeight = windowAspect / targetAspect;

        // If the window aspect ratio is wider than the target aspect ratio
        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            cam.rect = rect;
        }
        else // If the window aspect ratio is taller than the target aspect ratio
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void OnDisable()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;
    }

    //void OnPreCull()
    //{
    //    if (Application.isEditor) return;
    //    Rect wp = Camera.main.rect;
    //    Rect nr = new Rect(0, 0, 1, 1);

    //    Camera.main.rect = nr;
    //    GL.Clear(true, true, Color.black);

    //    Camera.main.rect = wp;
    //}
}
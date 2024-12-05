using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator PanCameraTo(Vector3 targetLocalPosition, float targetOrthographicSize, float duration)
    {
        Vector3 startPosition = transform.localPosition;
        float startOrthographicSize = cam.orthographicSize;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);

            transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, factor);
            cam.orthographicSize = Mathf.Lerp(startOrthographicSize, targetOrthographicSize, factor);

            yield return null;
        }


        transform.localPosition = targetLocalPosition;
    }
    public IEnumerator PanCameraTo(float targetOrthographicSize, float duration)
    {
        if (cam == null)
            Start();
        float startOrthographicSize = cam.orthographicSize;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);

            cam.orthographicSize = Mathf.Lerp(startOrthographicSize, targetOrthographicSize, factor);

            yield return null;
        }


    }
}

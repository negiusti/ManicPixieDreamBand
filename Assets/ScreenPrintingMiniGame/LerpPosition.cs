using System.Collections;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    // This needs to be public so other scripts can access it
    public bool finishedLerp;
    public bool finishedRotationLerp;

    public IEnumerator Lerp(Vector3 targetLocalPosition, float duration, bool destroyAfterLerp = false)
    {
        Debug.Log("lerpin");
        Vector3 startPosition = transform.localPosition;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);

            transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, factor);

            yield return null;
        }

        finishedLerp = true;

        transform.localPosition = targetLocalPosition;

        if (destroyAfterLerp)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LerpRotation(Vector3 targetLocalRotation, float duration, bool destroyAfterLerp = false)
    {
        Vector3 startPosition = transform.localEulerAngles;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);

            transform.localEulerAngles = Vector3.Lerp(startPosition, targetLocalRotation, factor);

            yield return null;
        }

        finishedRotationLerp = true;

        transform.localEulerAngles = targetLocalRotation;

        if (destroyAfterLerp)
        {
            Destroy(gameObject);
        }
    }
}
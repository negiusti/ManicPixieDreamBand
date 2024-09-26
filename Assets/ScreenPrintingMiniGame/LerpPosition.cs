using System.Collections;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    // This needs to be public so other scripts can access it
    public bool finishedLerp;
    public bool finishedRotationLerp;
    public bool finishedColorLerp;

    public IEnumerator Lerp(Vector3 targetLocalPosition, float duration, bool destroyAfterLerp = false)
    {
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

    public IEnumerator LerpColor(Vector3 targetLocalColor, float duration, bool destroyAfterLerp = false)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 startColor = new Vector3(sr.color.r, sr.color.g, sr.color.b);

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);
            Vector3 newColor = Vector3.Lerp(startColor, targetLocalColor, factor);
            sr.color = new Color(newColor.x, newColor.y, newColor.z, sr.color.a);

            yield return null;
        }

        finishedColorLerp = true;

        sr.color = new Color(targetLocalColor.x, targetLocalColor.y, targetLocalColor.z, sr.color.a);

        if (destroyAfterLerp)
        {
            Destroy(gameObject);
        }
    }
}
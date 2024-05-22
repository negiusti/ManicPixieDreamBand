using System.Collections;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    // This needs to be public so other scripts can access it
    public bool finishedLerp;

    public IEnumerator Lerp(Vector3 targetLocalPosition, float duration, bool destroyAfterLerp)
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
}
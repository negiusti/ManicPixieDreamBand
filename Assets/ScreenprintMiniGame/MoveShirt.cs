using System.Collections;
using UnityEngine;

public class MoveShirt : MonoBehaviour
{
    public IEnumerator LerpPosition(Vector3 targetLocalPosition, float duration, bool destroyAfterLerp)
    {
        Vector3 startPosition = transform.localPosition;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);

            transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, factor);

            yield return null;
        }

        transform.localPosition = targetLocalPosition;

        if (destroyAfterLerp)
        {
            Destroy(gameObject);
        }
    }
}
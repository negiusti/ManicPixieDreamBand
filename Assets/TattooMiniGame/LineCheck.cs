using UnityEngine;
using System.Collections;

public class LineCheck : MonoBehaviour
{
    public bool inGuideline;

    public TattooMiniGame tattooMiniGame;

    private void Start()
    {
        tattooMiniGame = FindFirstObjectByType<TattooMiniGame>(FindObjectsInactive.Include);

        StartCoroutine(DestroySelf());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if this object is within the guideline
        if (collision.gameObject == tattooMiniGame.guideline)
        {
            inGuideline = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "TattooGuidelineCheck" && inGuideline)
        {
            Destroy(collision.gameObject);
        }
    }

    // Add to the mistakes counter if this object isn't in the guidelines and destroy itself after a moment
    private IEnumerator DestroySelf()
    {
        yield return new WaitForSecondsRealtime(0.125f);

        if (!inGuideline)
        {
            tattooMiniGame.outOfGuidelines++;
        }

        Destroy(gameObject);
    }
}
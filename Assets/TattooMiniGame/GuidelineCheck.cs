using UnityEngine;
using System.Collections;

public class GuidelineCheck : MonoBehaviour
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

    // This code is called in OnTriggerStay2D() because it needs to run after the OnTriggerEnter2D() code
    private void OnTriggerStay2D(Collider2D collision)
    {
        // The completion checks are spawned on the guideline's collider and check whether or not the player has finished drawing the design
        if (collision.gameObject.tag == "TattooCompletionCheck" && inGuideline)
        {
            // If this object is in the guidelines and overlapping with a completion check, destroy that completion check
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
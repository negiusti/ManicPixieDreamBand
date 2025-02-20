using UnityEngine;
using System.Collections;

public class GuidelineCheck : MonoBehaviour
{
    public bool inGuideline;

    private bool onArm;

    public TattooMiniGame tattooMiniGame;
    public TattooArcadeGame tattooArcadeGame;

    private void Start()
    {
        //tattooMiniGame = FindFirstObjectByType<TattooMiniGame>(FindObjectsInactive.Include);
        tattooMiniGame = GetComponentInParent<TattooMiniGame>();
        tattooArcadeGame = GetComponentInParent<TattooArcadeGame>();

        StartCoroutine(DestroySelf());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if this object is within the guideline
        if ((tattooMiniGame != null && collision.gameObject == tattooMiniGame.guideline) || (tattooArcadeGame != null && collision.gameObject == tattooArcadeGame.guideline))
        {
            inGuideline = true;
        }

        // Check if this object is on the arm
        if (collision.gameObject.tag == "Obstacle")
        {
            onArm = true;
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

            // Add to the total number of checks that were spawned
            if (tattooMiniGame != null)
                tattooMiniGame.checksSpawned++;
            if (tattooArcadeGame != null)
                tattooArcadeGame.checksSpawned++;
        }
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSecondsRealtime(0.125f);

        if (!inGuideline && onArm)
        {
            // Add to the total number of checks that were spawned
            // And add to the total number of checks that were not in the guideline
            if (tattooMiniGame != null)
            {
                tattooMiniGame.checksSpawned++;
                tattooMiniGame.checksOutOfGuideline++;
            }           
                
            if (tattooArcadeGame != null)
            {
                tattooArcadeGame.checksSpawned++;
                tattooArcadeGame.checksOutOfGuideline++;
            }
        }

        // Destroy itself after waiting a moment
        Destroy(gameObject);
    }
}
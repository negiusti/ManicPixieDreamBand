using System.Collections.Generic;
using UnityEngine;

public class SkateObstacle : MonoBehaviour
{
    public GameObject background; // Reference to the background GameObject
    public List<float> potentialPositions;
    private Bounds backgroundBounds;
    private float minX, maxX; // Minimum and maximum x positions for the obstacle

    void Start()
    {
        // if (background != null)
        // {
        //     // Calculate the camera bounds based on the background's collider
        //     backgroundBounds = background.GetComponent<Collider2D>().bounds;
        //     minX = backgroundBounds.min.x + 5;
        //     maxX = backgroundBounds.max.x - 5;
        //     transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
        // }
        // else
        // {
        //     Debug.Log("Background GameObject not assigned. Not going to randomize position.");
        // }
    }

    private void OnEnable()
    {
        // if (backgroundBounds == null)
        //     return;

        // Random transform x position
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        if (potentialPositions == null || potentialPositions.Count == 0)
            return; 
        transform.position = new Vector3(potentialPositions[Random.Range(0, potentialPositions.Count)], transform.position.y, transform.position.z);
        // if (backgroundBounds == null)
        //     return;
        // transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
    }

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.gameObject.TryGetComponent(out SkateObstacle _) || collision.CompareTag("LRamp") || collision.CompareTag("RRamp"))
    //         RandomizePosition();
    // }
}
using UnityEngine;

public class SkateObstacle : MonoBehaviour
{
    public GameObject background; // Reference to the background GameObject
    private Bounds backgroundBounds;
    private float minX, maxX; // Minimum and maximum x positions for the obstacle

    void Start()
    {
        if (background != null)
        {
            // Calculate the camera bounds based on the background's collider
            backgroundBounds = background.GetComponent<Collider2D>().bounds;
            minX = backgroundBounds.min.x + 5;
            maxX = backgroundBounds.max.x - 5;
            transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
        }
        else
        {
            Debug.LogError("Background GameObject not assigned.");
        }
    }

    private void OnEnable()
    {
        if (backgroundBounds == null)
            return;

        // Random transform x position
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<SkateObstacle>(out SkateObstacle s))
            RandomizePosition();
    }
}
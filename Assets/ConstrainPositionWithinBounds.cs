using UnityEngine;

public class ConstrainPositionWithinBounds : MonoBehaviour
{
    public BoxCollider2D boundsCollider;

    private Vector3 startingPosition;

    void Start()
    {
        // Store the starting position when the script first runs
        startingPosition = transform.position;
    }

    void OnEnable()
    {
        // Reset position to the starting position when the GameObject is enabled
        // Randomize the starting position within the bounds of the BoxCollider2D
        Bounds bounds = boundsCollider.bounds;
        startingPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            transform.position.z // Keep the original z position
        );

        // Set the position to the randomized starting position
        transform.position = startingPosition;
        //ConstrainPosition();
    }

    void Update()
    {
        // Continuously constrain the position within bounds
        ConstrainPosition();
    }

    void ConstrainPosition()
    {
        if (boundsCollider != null)
        {
            // Get the bounds of the BoxCollider2D
            Bounds bounds = boundsCollider.bounds;

            // Get the current position
            Vector3 position = transform.position;

            // Clamp the position within the bounds of the BoxCollider2D
            position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
            position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);

            // Apply the clamped position back to the transform
            transform.position = position;
        }
        else
        {
            Debug.LogWarning("Bounds Collider not set.");
        }
    }
}

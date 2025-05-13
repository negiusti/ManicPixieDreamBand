using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxSpeed; // Adjust this value to change the parallax speed

    private float initialPosition;

    void Start()
    {
        initialPosition = transform.position.x; // Save the initial position of the background
        if (parallaxSpeed == 0f) {
            parallaxSpeed = 0.5f;
        }
    }

    void Update()
    {
        if (Camera.main == null)
            return;
        // Calculate the distance to move based on the camera's movement
        float distanceToMove = (Camera.main.transform.position.x - initialPosition) * parallaxSpeed;

        // Move the background
        transform.position = new Vector3(initialPosition + distanceToMove, transform.position.y, transform.position.z);
    }
}

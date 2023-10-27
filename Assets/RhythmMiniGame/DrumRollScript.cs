using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumRollScript : MonoBehaviour
{
    private float upSpeed = 15f; // Adjust the movement speed as needed
    private float downSpeed = 2.5f; // Adjust the movement speed as needed
    private float maxY = -0.02f;      // Define your maximum Y position
    private float minY = -4.14f;      // Define your minimum Y position
    private float minGood = -3f;
    private float maxGood = -1f;
    private SpriteRenderer sr;

        // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float newYPosition = transform.position.y;
        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Calculate the new Y position
            newYPosition = transform.position.y + upSpeed * Time.deltaTime;
        } else
        {
            newYPosition = transform.position.y - downSpeed * Time.deltaTime;
        }
        // Clamp the Y position within the specified range
        newYPosition = Mathf.Clamp(newYPosition, minY, maxY);

        // Update the game object's position
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

        if (newYPosition > minGood && newYPosition < maxGood)
            sr.color = Color.green;
        else
            sr.color = Color.red;
    }


}

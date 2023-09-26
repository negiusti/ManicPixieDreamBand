using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SqueegeeMoverScript : MonoBehaviour
{
    private float minX = -6.19f; // Minimum X position
    private float maxX = 6.19f;  // Maximum X position
    private float speed = 10f; // Movement speed
    private KeyCode keyCode = KeyCode.Space;
    private bool isCentered;
    private bool moving;
    public TextMeshPro tm;

    private bool movingRight = true;

    void Start()
    {
        isCentered = false;
        moving = true;
        tm.text = "";
    }

    void Update()
    {
        if (moving)
        {
            // Calculate the new position based on the current direction
            float newX = movingRight ? transform.position.x + speed * Time.deltaTime : transform.position.x - speed * Time.deltaTime;

            // Check if we've reached the minimum or maximum X value
            if (newX < minX)
            {
                newX = minX;
                movingRight = true; // Change direction to move right
            }
            else if (newX > maxX)
            {
                newX = maxX;
                movingRight = false; // Change direction to move left
            }

            // Update the GameObject's position
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(keyCode))
        {
            moving = false;
            if (isCentered)
            {
                tm.text = "Nice";
            } else
            {
                tm.text = "u suck";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            isCentered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            isCentered = false;
    }
}
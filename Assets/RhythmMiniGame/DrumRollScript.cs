using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumRollScript : MonoBehaviour
{
    public GameObject leftDrumStick;
    public GameObject rightDrumStick;
    private float inSpeed = 15f; 
    private float outSpeed = 2.5f; 
    private float maxRightX = 15.41f;
    private float minRightX = 11.36f; // TBD!!!
    private float minLeftX = -1.07f;
    private float maxLeftX = 2.95f; // TBD!!!!
    private float minGood = 13f;
    private float maxGood = 1f;
    private SpriteRenderer sr;
    bool toggleAlternateSticks = false;
    private Vector3 leftStartPos;
    private Vector3 rightStartPos;

        // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        leftStartPos = leftDrumStick.transform.position;
        rightStartPos = rightDrumStick.transform.position;
       // leftDrumStick.transform.localScale *= 2.0f;
        rightDrumStick.transform.localScale *= 0.5f;
    }
    void Update()
    {
        float newLeftXPosition = leftDrumStick.transform.position.x;
        float newRightXPosition = rightDrumStick.transform.position.x;

        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            newLeftXPosition += inSpeed * Time.deltaTime;
            newRightXPosition -= inSpeed * Time.deltaTime;
            if (toggleAlternateSticks)
            {
                leftDrumStick.transform.localScale *= 2.0f;
                rightDrumStick.transform.localScale *= 0.5f;
            } else
            {
                leftDrumStick.transform.localScale *= 0.5f;
                rightDrumStick.transform.localScale *= 2.0f;
            }
            toggleAlternateSticks = !toggleAlternateSticks;
        } else
        {
            newLeftXPosition -= outSpeed * Time.deltaTime;
            newRightXPosition += outSpeed * Time.deltaTime;
        }
        // Clamp the X position within the specified range
        newLeftXPosition = Mathf.Clamp(newLeftXPosition, minLeftX, maxLeftX);
        newRightXPosition = Mathf.Clamp(newRightXPosition, minRightX, maxRightX);

        // Update the game object's position
        rightDrumStick.transform.position = new Vector3(newRightXPosition, rightDrumStick.transform.position.y, rightDrumStick.transform.position.z);
        leftDrumStick.transform.position = new Vector3(newLeftXPosition, leftDrumStick.transform.position.y, leftDrumStick.transform.position.z);

        /*if (newYPosition > minGood && newYPosition < maxGood)
            sr.color = Color.green;
        else
            sr.color = Color.red;*/
    }


}


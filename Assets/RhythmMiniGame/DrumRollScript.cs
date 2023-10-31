using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumRollScript : MonoBehaviour
{
    public DrumstickScript leftDrumStick;
    public DrumstickScript rightDrumStick;
    private float inSpeed = 15f; 
    private float outSpeed = 2.5f; 
    private float maxRightX = 15.41f;
    private float minRightX = 11.36f; // TBD!!!
    private float minLeftX = -1.07f;
    private float maxLeftX = 2.95f; // TBD!!!!
    //private float minGood = 13f;
    //private float maxGood = 1f;
    private SpriteRenderer sr;
    bool toggleAlternateSticks = false;
    private Vector3 leftStartScale;
    private Vector3 rightStartScale;

        // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        leftStartScale = leftDrumStick.transform.localScale;
        rightStartScale = rightDrumStick.transform.localScale;
        //rightDrumStick.transform.localScale *= 0.75f;
    }
    void Update()
    {
        if (leftDrumStick.Shaking() || rightDrumStick.Shaking())
            return; 
        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (toggleAlternateSticks)
            {
                leftDrumStick.transform.localScale = leftStartScale;
                rightDrumStick.transform.localScale *= 0.75f;
            } else
            {
                leftDrumStick.transform.localScale *= 0.75f;
                rightDrumStick.transform.localScale = rightStartScale;
            }
            toggleAlternateSticks = !toggleAlternateSticks;
        }
    }


}


using UnityEngine;

public class DrumRollScript : MonoBehaviour
{
    public DrumstickScript leftDrumStick;
    public DrumstickScript rightDrumStick;
    //private SpriteRenderer sr;
    bool toggleAlternateSticks = false;
    private Vector3 leftStartScale;
    private Vector3 rightStartScale;

        // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        leftStartScale = leftDrumStick.transform.localScale;
        rightStartScale = rightDrumStick.transform.localScale;
    }
    void Update()
    {
        //if (leftDrumStick.Shaking() || rightDrumStick.Shaking())
        //    return; 
        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (toggleAlternateSticks)
            {
                leftDrumStick.transform.localScale = leftStartScale;
                rightDrumStick.transform.localScale *= 0.85f;
            } else
            {
                leftDrumStick.transform.localScale *= 0.85f;
                rightDrumStick.transform.localScale = rightStartScale;
            }
            toggleAlternateSticks = !toggleAlternateSticks;
        }
    }


}


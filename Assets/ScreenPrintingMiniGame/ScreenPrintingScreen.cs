using System.Collections;
using UnityEngine;

public class ScreenPrintingScreen : MonoBehaviour
{
    private ScreenPrintingMiniGame screenPrintingManager;

    [Header("Moving")]

    public float xBound = 3.75f; // The maximum and minimum X position of the screen
    public float speed = 10f; // The speed at which the screen moves
    private bool movingRight = true;

    [HideInInspector] public bool moveScreen = true;

    [Header("Printing")]

    public KeyCode printKey = KeyCode.Space;
    public float printStopDuration; // How long the screen stops for when the print key is pressed

    private bool onPrintCooldown; // Printing has a cooldown so you can't spam the print key
    public float printCooldownDuration;

    private GameObject center; // This is where the center of the shirt should be
    private float distanceFromCenter;
    
    [Header("Lerping Shirts")]

    public GameObject shirtPrefab;
    public GameObject designPrefab;

    private GameObject shirt;
    private LerpPosition lerpPositionScript;

    public float shirtLerpDuration; // The smaller this value is, the faster the lerp is

    private void Start()
    {
        center = transform.parent.gameObject; // The center is also where the ScreenPrintingMiniGame object is

        screenPrintingManager = transform.parent.gameObject.GetComponent<ScreenPrintingMiniGame>();
    }

    private void Update()
    {
        if (moveScreen)
        {
            // Calculate the new position based on the current direction
            float newX = movingRight ? transform.position.x + speed * Time.deltaTime : transform.position.x - speed * Time.deltaTime;

            // Check if we've reached the minimum or maximum X position
            if (newX < -xBound)
            {
                newX = -xBound;
                movingRight = true; // Change direction to move right
            }
            else if (newX > xBound)
            {
                newX = xBound;
                movingRight = false; // Change direction to move left
            }

            // Update the GameObject's position
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            // Make sure the printKey is pressed, the print is not on cooldown, and the shirt has arrived at where it should be at the center
            if (Input.GetKeyDown(printKey) && !onPrintCooldown && lerpPositionScript.finishedLerp)
            {
                StartCoroutine(Print());

                StartCoroutine(DoPrintCooldown());

                distanceFromCenter = Mathf.Abs(center.transform.position.x - transform.position.x);
                //distanceFromCenter = Mathf.Abs(shirt.transform.position.x - transform.position.x);
                screenPrintingManager.Score(distanceFromCenter);
            }
        }
    }

    private IEnumerator Print()
    {
        // Destroy the guideline on the shirt prrfab 
        Destroy(shirt.transform.GetChild(0).gameObject);

        // Print the design onto the shirt in the form of a child object to the shirt
        GameObject newDesign = Instantiate(designPrefab, transform.position + designPrefab.transform.position, Quaternion.identity, shirt.transform);
        //GameObject newDesign = Instantiate(designPrefab, transform.position, Quaternion.identity, shirt.transform);

        // Stop the screen's movement for a moment
        moveScreen = false;
        yield return new WaitForSeconds(printStopDuration);

        // Make sure the screen doesn't start moving again after the minigame has ended
        if (!screenPrintingManager.minigameComplete)
        {
            moveScreen = true;
        }

        // Lerp the shirt out of view to the left of the camera
        StartCoroutine(lerpPositionScript.Lerp(new Vector2(shirtPrefab.transform.position.x - 25f, shirtPrefab.transform.position.y), shirtLerpDuration, true));

        // Make sure new shirts don't spawn after the minigame is done
        if (!screenPrintingManager.minigameComplete)
        {
            SpawnNewShirt();
        }
    }

    private IEnumerator DoPrintCooldown()
    {
        onPrintCooldown = true;

        yield return new WaitForSeconds(printCooldownDuration);

        onPrintCooldown = false;
    }

    public void SpawnNewShirt()
    {
        // Spawn a new shirt out of view to the right of the camera
        shirt = Instantiate(shirtPrefab, new Vector2(shirtPrefab.transform.position.x + 25f, shirtPrefab.transform.position.y), Quaternion.identity, transform.parent);

        lerpPositionScript = shirt.GetComponent<LerpPosition>();

        // Lerp the shirt to where the center is
        StartCoroutine(lerpPositionScript.Lerp(shirtPrefab.transform.position, shirtLerpDuration, false));
    }
}
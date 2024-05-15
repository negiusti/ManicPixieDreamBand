using System.Collections;
using UnityEngine;
using TMPro;

public class ScreenMoverScript : MonoBehaviour
{
    private ScreenPrintingManager screenPrintingManager;

    [Header("Moving")]

    public float xBound = 6.25f; // The maximum and minimum X position of the screen
    public float speed = 10f; // The speed at which the screen moves
    private bool movingRight = true;

    [HideInInspector] public bool moveScreen = true;

    [Header("Printing")]

    public KeyCode printKey = KeyCode.Space;
    public float printStopDuration; // How long the screen stops for when the print key is pressed

    private bool onPrintCooldown; // Printing has a cooldown so you can't spam the print key
    public float printCooldownDuration;

    private GameObject gradient;
    private float distanceFromGradient;

    [Header("Thresholds")]

    public float successThreshold; // The margin of error required for a successful print
    public float misprintThreshold; // The margin of error required for a misprint

    [HideInInspector] public TextMeshPro result; // Your bandmates' reaction

    [Header("Scoring")]

    public int successScore = 20;
    public int misprintScore = 5;
    public int failScore = -5;

    [Header("Moving Shirts")]

    public GameObject shirtPrefab;
    public GameObject patternPrefab;

    private GameObject shirt;
    private MoveShirt moveShirtScript;

    public float lerpDuration; // The smaller this value is, the faster the lerp is

    [Header("Popping")]

    public GameObject successIcon;
    public GameObject misprintIcon;
    public GameObject failureIcon;

    private void Start()
    {
        gradient = transform.parent.gameObject;

        result = transform.parent.gameObject.GetComponentInChildren<TextMeshPro>();

        screenPrintingManager = transform.parent.gameObject.GetComponent<ScreenPrintingManager>();

        SpawnNewShirt();
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

            if (Input.GetKeyDown(printKey) && !onPrintCooldown)
            {
                StartCoroutine(Print());

                StartCoroutine(DoPrintCooldown());

                Score();
            }
        }
    }

    // Scores the print based on the distance between the screen and the gradient and updates the score text
    private void Score()
    {
        // Calculate how close the screen is from the gradient
        distanceFromGradient = Mathf.Abs(gradient.transform.position.x - transform.position.x);

        if (distanceFromGradient <= successThreshold)
        {
            screenPrintingManager.score += successScore;

            result.text = "Perfect!";

            successIcon.GetComponent<Animator>().SetTrigger("PopTrigger");
        }
        else if (distanceFromGradient <= misprintThreshold)
        {
            screenPrintingManager.score += misprintScore;

            result.text = "Good!";

            misprintIcon.GetComponent<Animator>().SetTrigger("PopTrigger");
        }
        else
        {
            screenPrintingManager.score += failScore;

            result.text = "Bad!";

            failureIcon.GetComponent<Animator>().SetTrigger("PopTrigger");
        }

        screenPrintingManager.UpdateScoreText();
    }

    private IEnumerator Print()
    {
        // Print the pattern onto the shirt in the form of a child object to the shirt
        GameObject newPattern = Instantiate(patternPrefab, transform.position, Quaternion.identity);
        newPattern.transform.parent = shirt.transform;

        // Stop the screen's movement for a moment
        moveScreen = false;
        yield return new WaitForSeconds(printStopDuration);
        moveScreen = true;

        result.text = "";

        // Lerp the shirt out of view to the left of the camera
        StartCoroutine(moveShirtScript.LerpPosition(new Vector2(transform.parent.position.x - 25f, transform.parent.position.y), lerpDuration, true));

        SpawnNewShirt();
    }

    private IEnumerator DoPrintCooldown()
    {
        onPrintCooldown = true;

        yield return new WaitForSeconds(printCooldownDuration);

        onPrintCooldown = false;
    }

    private void SpawnNewShirt()
    {
        // Spawn a new shirt out of view to the right of the camera
        shirt = Instantiate(shirtPrefab, new Vector2(transform.parent.position.x + 25f, transform.parent.position.y), Quaternion.identity);

        moveShirtScript = shirt.GetComponent<MoveShirt>();

        // Lerp the shirt to where the gradient is
        StartCoroutine(moveShirtScript.LerpPosition(transform.parent.position, lerpDuration, false));
    }
}
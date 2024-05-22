using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class TattooMiniGame : MonoBehaviour
{
    [Header("Drawing")]

    public GameObject linePrefab;
    private Line line; // The line being used now

    [Header("Arm Information")]

    public GameObject armPrefab;

    private GameObject arm; // The arm being used now
    private LerpPosition armLerpScript;
    public float armLerpDuration;

    [Header("Guideline Information")]

    public GameObject guideline; // The guideline being used now
    private PolygonCollider2D guidelineCollider;

    public GameObject[] guidelinePrefabs;

    // A list of all of the points connected by the guideline's collider
    private List<Vector2> guidelineColliderPoints = new List<Vector2>();

    [Header("Checking")]

    public GameObject completionCheckPrefab;
    public int completionThreshold = 5;
    public int spawnIterator = 5;

    // The number of guideline checks spawned, not counting checks spawned in the guideline that overlap with a previous line so that players can't inflate their scores
    public float checksSpawned;

    // The number of guideline checks which were not in the guideline
    public float checksOutOfGuideline;

    [Header("Scoring")]

    public float successThreshold;
    public float passThreshold;

    [Header("Incomes")]

    private float income;

    public float successIncome;
    public float passIncome;
    public float failureIncome;

    [Header("Timing")]

    public float timer = 30f; // How much time the player has at the start of the minigame
    public TextMeshPro timerText;
    private bool doTimer;
    private bool hasDoneTimerCheck;

    [Header("Speech")]

    public GameObject speechBubble;
    private Text speechText;

    public float speechBubbleActiveDuration;

    public string[] successOptions;
    public string[] passOptions;
    public string[] failureOptions;

    public string[] timerDoneOptions;

    public GameObject blackScreen;

    private void Start()
    {
        doTimer = true;

        blackScreen.SetActive(false);

        speechText = speechBubble.GetComponentInChildren<Text>();

        SpawnNewArm();
    }

    private void Update()
    {
        // Spawn a new line as a child of the arm and get its Line component
        if (Input.GetMouseButtonDown(0) && armLerpScript.finishedLerp && timer >= 0)
        {
            GameObject newLine = Instantiate(linePrefab);
            newLine.transform.parent = arm.transform;
            line = newLine.GetComponent<Line>();
        }

        // If a line is being drawn, update that line using the current mouse position
        if (line != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            line.UpdateLine(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            line = null;

            // If all of the guideline's checks have been destroyed
            if (guideline != null && guideline.transform.childCount <= completionThreshold)
            {
                Score();
            }
        }

        if (timer >= 0 && doTimer)
        {
            timer -= Time.deltaTime;
        }

        // Set the timer text to the nearest integer
        timerText.text = Mathf.RoundToInt(timer).ToString();

        if (timer <= 0 && !hasDoneTimerCheck)
        {
            // Makes sure this if statement doesn't run again
            hasDoneTimerCheck = true;

            StartCoroutine(CloseMiniGameSequence("Timer done"));
        }
    }

    private void SpawnNewArm()
    {
        // Spawn a new arm offscreen as a child of this object
        arm = Instantiate(armPrefab, new Vector2(transform.position.x - 50, transform.position.y), Quaternion.identity, transform);

        armLerpScript = arm.GetComponent<LerpPosition>();

        // Choose a random guideline from the array of guidelines
        GameObject guidelineToSpawn = guidelinePrefabs[Random.Range(0, guidelinePrefabs.Length)];

        // And spawn it as a child of the arm
        guideline = Instantiate(guidelineToSpawn, arm.transform.position, guidelineToSpawn.transform.rotation, arm.transform);

        SpawnCompletionChecks();

        // Lerp the arm to be centered on the screen
        StartCoroutine(armLerpScript.Lerp(new Vector2(transform.position.x, transform.position.y), armLerpDuration, false));
    }

    // Spawns completion check objects using the guideline's polygon collider
    private void SpawnCompletionChecks()
    {
        guidelineCollider = guideline.GetComponent<PolygonCollider2D>();

        // Clear the guidelineColliderPoints list so it doesn't use the points of the previous guidelines
        guidelineColliderPoints.Clear();

        // Looping through the paths of the guideline's collider, which contain the points
        for (int i = 0; i < guidelineCollider.pathCount; i++)
        {
            // Looping through the points in the current path and adding them to the list
            for (int j = 0; j < guidelineCollider.GetPath(i).Length; j++)
            {
                guidelineColliderPoints.Add(guidelineCollider.GetPath(i)[j]);
            }
        }

        // Iterate by spawnIterator so it doesn't spawn a bajillion completion checks
        for (int i = 0; i < guidelineColliderPoints.Count; i += spawnIterator)
        {
            // Transposing the points from local to world position by multiplying by the guideline's scale and adding the guideline's position
            Vector2 checkPosition = new Vector2(guidelineColliderPoints[i].x * guideline.transform.localScale.x + guideline.transform.position.x, guidelineColliderPoints[i].y * guideline.transform.localScale.y + guideline.transform.position.y);

            // Spawn a completion check object at the position stored in the list
            Instantiate(completionCheckPrefab, checkPosition, Quaternion.identity, guideline.transform);
        }
    }

    private void Score()
    {
        // Your successPercentage is the percentage of total guideline checks spawned that were within the guideline
        float successPercentage = Mathf.Round(100 - ((checksOutOfGuideline / checksSpawned) * 100));

        if (successPercentage >= successThreshold)
        {
            income = successIncome;
            StartCoroutine(CloseMiniGameSequence("Success"));
        }
        else if (successPercentage >= passThreshold)
        {
            income = passIncome;
            StartCoroutine(CloseMiniGameSequence("Pass"));
        }
        else
        {
            income = failureIncome;
            StartCoroutine(CloseMiniGameSequence("Failure"));
        }
    }

    private IEnumerator CloseMiniGameSequence(string result)
    {
        line = null;

        doTimer = false;

        Destroy(guideline);

        yield return new WaitForSeconds(0.5f);

        // Display the speech bubble and the appropriate text
        StartCoroutine(UpdateSpeechBubbles(result));

        // Wait until after the speech bubble disappear
        yield return new WaitForSeconds(speechBubbleActiveDuration);

        // Lerp the arm offscreen and destroy it
        StartCoroutine(armLerpScript.Lerp(new Vector2(transform.position.x - 50, transform.position.y), armLerpDuration, true));

        // Wait a moment before fading to black
        yield return new WaitForSeconds(0.25f);

        // Start fading to black
        blackScreen.SetActive(true);
    }

    private IEnumerator UpdateSpeechBubbles(string option)
    {
        string stringToDisplay;

        // Depending on how high the player's successPercentage is and whether or not the timer has finished, change the string to display to be a random string from the corresponding array

        if (option == "Success")
        {
            stringToDisplay = successOptions[Random.Range(0, successOptions.Length)];
        }
        else if (option == "Pass")
        {
            stringToDisplay = passOptions[Random.Range(0, passOptions.Length)];
        }
        else if (option == "Failure")
        {
            stringToDisplay = failureOptions[Random.Range(0, failureOptions.Length)];
        }
        else
        {
            stringToDisplay = timerDoneOptions[Random.Range(0, timerDoneOptions.Length)];
        }

        speechBubble.SetActive(true);
        speechText.text = stringToDisplay;

        yield return new WaitForSeconds(speechBubbleActiveDuration);

        speechBubble.SetActive(false);
    }
}
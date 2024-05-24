using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class TattooMiniGame : MiniGame
{
    [Header("Drawing")]

    public GameObject linePrefab;
    private Line line; // The line being used now

    private Dictionary<string, Color> lineAppearances;
    public Color currentLineAppearance;

    [Header("Arm Information")]

    public Transform armLerpPosition;
    public float armLerpDuration;

    public GameObject[] armPrefabs;

    private GameObject arm; // The arm being used now
    private LerpPosition armLerpScript;
    
    [Header("Guideline Information")]

    public GameObject[] guidelinePrefabs;

    public Sprite[] designPrefabs;

    [HideInInspector] public GameObject guideline; // The guideline being used now
    private PolygonCollider2D guidelineCollider;

    // A list of all of the points connected by the guideline's collider
    private List<Vector2> guidelineColliderPoints = new List<Vector2>();

    int guidelineIndex;

    [Header("Checking")]

    public GameObject completionCheckPrefab;
    public float completionThreshold = 0.025f; // What percent of completionChecks need to be left for the design to be done
    public int spawnIterator;

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

    [Header("Miscellaneous")]

    private GameObject mainCamera;
    public GameObject blackScreen;
    private bool isActive;

    public Texture2D tattooGun;

    private void Start()
    {
        //DisableAllChildren();

        OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        // Opening up the minigame

        mainCamera = Camera.main.transform.gameObject;

        //mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;

        // Getting and setting components of the minigame

        // Set the cursor to the tattoo gun followed by an offset that makes the tip of the gun align with where lines are spawned
        Cursor.SetCursor(tattooGun, new Vector2(0, 260), CursorMode.Auto);

        blackScreen.SetActive(false);
        speechBubble.SetActive(false);

        speechText = speechBubble.GetComponentInChildren<Text>();

        LoadColors();
        SpawnNewArm();

        // Only start the timer after the minigame has started and all its components have been set
        doTimer = true;
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        // Resets the cursor to be the heart
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();

        // Add the player's score they got into their bank account
        MainCharacterState.ModifyBankBalance(income);
    }

    private void Update()
    {
        Draw();

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

    private void Draw()
    {
        // If the arm has arrived at the screen's center, the timer has not finished, and the game is active
        if (isActive && Input.GetMouseButtonDown(0) && armLerpScript.finishedLerp && timer >= 0)
        {
            // Spawn a new line as a child of the arm and get its Line component
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
            if (guideline != null && guideline.transform.childCount <= (completionThreshold * guidelineColliderPoints.Count))
            {
                Score();
            }
        }
    }

    private void SpawnNewArm()
    {
        // Choose a random arm from the array of arms
        int armIndex = Random.Range(0, armPrefabs.Length);

        // Spawn a new arm offscreen as a child of this object
        arm = Instantiate(armPrefabs[armIndex], new Vector2(armLerpPosition.position.x + 50, armLerpPosition.position.y + 25), Quaternion.identity, transform);

        armLerpScript = arm.GetComponent<LerpPosition>();

        // Choose a random guideline from the array of guidelines
        guidelineIndex = Random.Range(0, guidelinePrefabs.Length);

        // And spawn it as a child of the arm with an offset so that it's on the arm
        guideline = Instantiate(guidelinePrefabs[guidelineIndex], new Vector2(arm.transform.position.x, arm.transform.position.y - 4.075f), guidelinePrefabs[guidelineIndex].transform.rotation, arm.transform);

        SpawnCompletionChecks();

        // Lerp the arm to be centered on the screen
        StartCoroutine(armLerpScript.Lerp(armLerpPosition.position, armLerpDuration, false));

        currentLineAppearance = lineAppearances[guidelinePrefabs[guidelineIndex].name];
    }

    private void SpawnCompletionChecks()
    {
        guidelineCollider = guideline.GetComponent<PolygonCollider2D>();

        // Clear the guidelineColliderPoints list so it doesn't use the points of the previous guidelines
        guidelineColliderPoints.Clear();

        // Looping through the paths of the guideline's polygon collider, which contain the points
        for (int i = 0; i < guidelineCollider.pathCount; i++)
        {
            // Looping through the points in the current path and adding them to the list
            // Iterate by spawnIterator so it doesn't spawn a bajillion completion checks
            for (int j = 0; j < guidelineCollider.GetPath(i).Length; j += spawnIterator)
            {
                guidelineColliderPoints.Add(guidelineCollider.GetPath(i)[j]);
            }
        }

        for (int i = 0; i < guidelineColliderPoints.Count; i++)
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

        // If the design was finished and the timer didn't run out, display the design
        if (result != "Timer done")
        {
            guideline.GetComponent<SpriteRenderer>().sprite = designPrefabs[guidelineIndex];
            guideline.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            guideline.GetComponent<Animator>().SetTrigger("DoPop");

            yield return new WaitForSeconds(0.5f);
        }

        // Display the speech bubble and the appropriate text
        StartCoroutine(UpdateSpeechBubbles(result));

        // Wait until after the speech bubble disappear
        yield return new WaitForSeconds(speechBubbleActiveDuration);

        // Lerp the arm offscreen and destroy it
        StartCoroutine(armLerpScript.Lerp(new Vector2(transform.position.x + 50, transform.position.y + 25), armLerpDuration, true));

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

    private void LoadColors()
    {
        lineAppearances = new Dictionary<string, Color>();

        lineAppearances.Add("Garf", new Color(0.6156863f, 0.3411765f, 0.02745098f));
        lineAppearances.Add("LiveLaughLove", new Color(0.3960784f, 0, 0));
        lineAppearances.Add("Mech", new Color(0.6745098f, 0.09803922f, 0.2117647f));
        lineAppearances.Add("CoolS", new Color(0.3254902f, 0.5254902f, 0.5607843f));
        lineAppearances.Add("Mom", new Color(0.9960785f, 0.6431373f, 0.6470588f));
    }
}
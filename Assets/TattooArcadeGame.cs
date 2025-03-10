using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Febucci.UI.Core;

public class TattooArcadeGame : MiniGame
{
    [Header("Drawing")]

    public GameObject linePrefab;
    private Line line; // The line being used now

    private Dictionary<string, Color> lineAppearances;
    public Color currentLineAppearance;

    [Header("Arm Information")]

    public Transform armLerpPosition;
    public float armLerpDuration;

    public GameObject armPrefab;

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
    private uint score;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI scoreModTxt;
    private Animator scoreModAnimator;
    private TypewriterCore scoreTypewriter;

    [Header("Incomes")]

    private float income;

    public float successIncome;
    public float passIncome;
    public float failureIncome;

    [Header("Speech")]

    public GameObject speechBubble;
    private TextMeshProUGUI speechText;

    public float speechBubbleActiveDuration;

    public string[] successOptions;
    public string[] passOptions;
    public string[] failureOptions;

    [Header("Miscellaneous")]

    private BlackScreen blackScreen;
    private bool isActive;
    private Timer timer;
    private HashSet<int> completedTattoos;
    private bool tattooDone;

    private void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        timer = GetComponentInChildren<Timer>(true);
        completedTattoos = new HashSet<int>();
        scoreTypewriter = scoreTxt.gameObject.GetComponent<TypewriterCore>();
        OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        isActive = true;
        score = 0;
        scoreTxt.text = "0";
        scoreModTxt.text = "";
        scoreModAnimator = scoreModTxt.gameObject.GetComponent<Animator>();
        // Getting and setting components of the minigame

        // Set the cursor to the tattoo gun followed by an offset that makes the tip of the gun align with where lines are spawned
        //Cursor.SetCursor(tattooGun, new Vector2(0, 260), CursorMode.Auto);
        Cursor.visible = false;

        blackScreen.Unfade();
        speechText = speechBubble.GetComponentInChildren<TextMeshProUGUI>();
        speechBubble.SetActive(false);
        checksOutOfGuideline = 0f;
        checksSpawned = 0f;
        LoadColors();
        SpawnNewArm();

        income = 0;
        timer.Reset();
        timer.StartTimer();
    }

    public override void CloseMiniGame()
    {
        // Resets the cursor to be the heart
        Cursor.visible = true;
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        isActive = false;

        // Add the player's score they got into their bank account
        SceneChanger.Instance.ChangeScene("SplashArcade");
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        // If the arm has arrived at the screen's center, the timer has not finished, and the game is active
        if (isActive && Input.GetMouseButtonDown(0) && armLerpScript.finishedLerp)// && timer.IsRunning())
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
            if (!tattooDone && guideline != null && guideline.transform.childCount <= (completionThreshold * guidelineColliderPoints.Count))
            {
                tattooDone = true;
                Score();
            }
        }
    }

    private void SpawnNewArm()
    {
        // Spawn a new arm offscreen as a child of this object
        arm = Instantiate(armPrefab, new Vector2(armLerpPosition.position.x + 50, armLerpPosition.position.y + 25), Quaternion.identity, transform);

        armLerpScript = arm.GetComponent<LerpPosition>();

        // Choose a random guideline from the array of guidelines
        do
        {
            guidelineIndex = Random.Range(0, guidelinePrefabs.Length);
        } while (completedTattoos.Contains(guidelineIndex));

        completedTattoos.Add(guidelineIndex);

        // And spawn it as a child of the arm with an offset so that it's on the arm
        guideline = Instantiate(guidelinePrefabs[guidelineIndex], new Vector2(arm.transform.position.x, arm.transform.position.y - 4.075f), guidelinePrefabs[guidelineIndex].transform.rotation, arm.transform);

        SpawnCompletionChecks();
        checksOutOfGuideline = 0f;
        checksSpawned = 0f;
        tattooDone = false;

        // Lerp the arm to be centered on the screen
        StartCoroutine(armLerpScript.Lerp(armLerpPosition.localPosition, armLerpDuration, false));

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
        Debug.Log("successPercentage: " + successPercentage);
        score += (uint)(successPercentage*111);
        //scoreTxt.text = "<rainb><wiggle>" + score;
        scoreModTxt.text = "<rainb><wiggle>+" + (successPercentage * 111);
        scoreModAnimator.Play("TattooScoreMod", -1, 0f);
        if (successPercentage >= successThreshold)
        {
            income += successIncome;
            StartCoroutine(FinishTattooSequence("Success"));
        }
        else if (successPercentage >= passThreshold)
        {
            income += passIncome;
            StartCoroutine(FinishTattooSequence("Pass"));
        }
        else
        {
            income += failureIncome;
            StartCoroutine(FinishTattooSequence("Failure"));
        }
    }

    private IEnumerator FinishTattooSequence(string result)
    {
        line = null;
        checksOutOfGuideline = 0f;
        checksSpawned = 0f;

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
        if (completedTattoos.Count < guidelinePrefabs.Length && timer.IsRunning())
        {
            SpawnNewArm();
        }
        else
        {
            blackScreen.Fade();
        }
        yield return new WaitForSeconds(0.5f);
        scoreTxt.text = "<wiggle>" + score;
        scoreModTxt.text = "";
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
        else // Failure
        {
            stringToDisplay = failureOptions[Random.Range(0, failureOptions.Length)];
        }

        speechText.text = stringToDisplay;
        speechBubble.SetActive(true);
        if (option != "Success" && option != "Pass")
        {
            yield return new WaitForSeconds(0.1f);
            Camera.main.gameObject.GetComponent<CameraShaker>().CameraShake();
        }

        yield return new WaitForSeconds(speechBubbleActiveDuration);

        speechBubble.SetActive(false);
        speechText.text = "";
    }

    private void LoadColors()
    {
        lineAppearances = new Dictionary<string, Color>();

        lineAppearances.Add("Garf", new Color(0.6156863f, 0.3411765f, 0.02745098f));
        lineAppearances.Add("LiveLaughLove", new Color(0.3960784f, 0, 0));
        lineAppearances.Add("Mech", new Color(0.6745098f, 0.09803922f, 0.2117647f));
        lineAppearances.Add("CoolS", new Color(0.3254902f, 0.5254902f, 0.5607843f));
        lineAppearances.Add("Mom", new Color(0.9960785f, 0.6431373f, 0.6470588f));
        lineAppearances.Add("Star", new Color(0.9882354f, 0.7058824f, 0f));
        lineAppearances.Add("Cherry", new Color(0.8078432f, 0.172549f, 0.007843138f));
        lineAppearances.Add("Cat", new Color(0.854902f, 0.3254902f, 0.007843138f));
    }
}
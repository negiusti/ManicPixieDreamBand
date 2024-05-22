using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenPrintingMiniGame : MiniGame
{
    private GameObject mainCamera;
    private bool isActive;
    private ScreenPrintingScreen screen;

    [Header("Scoring")]
    
    public int score; // The total score or income earned from the minigame

    public int successScore = 20;
    public int misprintScore = 5;
    public int failScore = -5;

    [Header("Thresholds")]

    public float successThreshold; // The margin of error required for a successful print
    public float misprintThreshold; // The margin of error required for a misprint

    [Header("Timing")]

    public float timer = 30f; // How much time the player has at the start of the minigame
    public TextMeshPro timerText;
    private bool doTimer;
    private bool hasDoneTimerCheck;

    [Header("Shirt Icons")]

    public GameObject shirtIconsParent; // The GameObject with all of the empty shirt icons as its children

    public List<GameObject> shirtIcons = new List<GameObject>(); // A list of all the empty shirt icons

    public GameObject successIcon;
    public GameObject misprintIcon;
    public GameObject failureIcon;

    [Header("Speech Bubbles")]

    public GameObject MaxSpeechBubble;
    public GameObject RickiSpeechBubble;
    private Text MaxSpeechText;
    private Text RickiSpeechText;

    private bool SpeakingTurn; // Used to alternate between who is speaking, returns true for Max and false for Ricki

    public float speechBubbleActiveDuration; // How long the speech bubble appears for

    [Header("Speech Options")]

    // To add a new line to a string in the inspector on Mac, do Option + Shift + Return

    public string[] successOptions;
    public string[] misprintOptions;
    public string[] failureOptions;
    public string[] timerDoneOptions;
    public string[] minigameCompleteOptions;

    [Header("Closing Sequence")]

    public GameObject maxArms;
    public GameObject rickiArms;

    private LerpPosition maxArmsLerpScript;
    private LerpPosition rickiArmsLerpScript;

    public float armsLerpDuration; // How long the arms take to move off the screen; the smaller this value is, the faster it goes

    public GameObject blackScreen;

    [HideInInspector] public bool minigameComplete;

    private void Start()
    {
        DisableAllChildren();

        //OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        // Opening up the minigame

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;

        // Getting components of the minigame

        screen = GetComponentInChildren<ScreenPrintingScreen>();

        // Adding all of the shirt icons to the shirtIcons list
        for (int i = 0; i < shirtIconsParent.transform.childCount; i++)
        {
            shirtIcons.Add(shirtIconsParent.transform.GetChild(i).gameObject);
        }

        MaxSpeechText = MaxSpeechBubble.GetComponentInChildren<Text>();
        RickiSpeechText = RickiSpeechBubble.GetComponentInChildren<Text>();

        MaxSpeechBubble.SetActive(false);
        RickiSpeechBubble.SetActive(false);

        maxArmsLerpScript = maxArms.GetComponent<LerpPosition>();
        rickiArmsLerpScript = rickiArms.GetComponent<LerpPosition>();

        blackScreen.SetActive(false);

        screen.SpawnNewShirt();

        // Only start the timer after the minigame has started and all its components have been set
        doTimer = true;
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();

        // Add the player's score they got into their bank account
        MainCharacterState.ModifyBankBalance(score);
    }

    private void Update()
    {
        // Count down if the timer is not at 0
        if (timer >= 0 && !minigameComplete && doTimer)
        {
            timer -= Time.deltaTime;
        }

        // Set the timer text to the nearest integer
        timerText.text = Mathf.RoundToInt(timer).ToString();

        if (timer <= 0 && !hasDoneTimerCheck)
        {
            // Makes sure this if statement doesn't run again
            hasDoneTimerCheck = true;

            StartCoroutine(UpdateSpeechBubbles("Timer Done"));

            // Start the closing sequence
            StartCoroutine(CloseMiniGameSequence());
        }
    }

    public void Score(float distance)
    {
        // Score the distance passed in based on how small it is and update the speech bubbles and shirt icons

        if (distance <= successThreshold)
        {
            score += successScore;

            StartCoroutine(UpdateSpeechBubbles("Success"));
            UpdateIcons("Success");
        }
        else if (distance <= misprintThreshold)
        {
            score += misprintScore;

            StartCoroutine(UpdateSpeechBubbles("Misprint"));
            UpdateIcons("Misprint");
        }
        else
        {
            score += failScore;

            StartCoroutine(UpdateSpeechBubbles("Failure"));
            UpdateIcons("Failure");
        }
    }

    public void UpdateIcons(string result)
    {
        // Spawns the icon prefab corresponding to the result, then destroy the empty shirt icon and remove the now empty first slot from the list

        if (result == "Success")
        {
            Instantiate(successIcon, shirtIcons[0].transform.position, successIcon.transform.rotation, shirtIconsParent.transform);
            Destroy(shirtIcons[0]);
            shirtIcons.RemoveAt(0);
        }
        else if (result == "Misprint")
        {
            Instantiate(misprintIcon, shirtIcons[0].transform.position, misprintIcon.transform.rotation, shirtIconsParent.transform);
            Destroy(shirtIcons[0]);
            shirtIcons.RemoveAt(0);
        }
        else
        {
            Instantiate(failureIcon, shirtIcons[0].transform.position, failureIcon.transform.rotation, shirtIconsParent.transform);
            Destroy(shirtIcons[0]);
            shirtIcons.RemoveAt(0);
        }

        // If the last empty icon of the list has been filled, end the minigame
        if (shirtIcons.Count == 0)
        {
            StartCoroutine(UpdateSpeechBubbles("Minigame Complete"));

            // Start the closing sequence
            StartCoroutine(CloseMiniGameSequence());
        }
    }

    private IEnumerator UpdateSpeechBubbles(string option)
    {
        // Based on what is passed in, choose a random string from the respective array and save that to a new string, then display that string in one of the speech bubbles
        string stringToDisplay;

        if (option == "Success")
        {
            stringToDisplay = successOptions[Random.Range(0, successOptions.Length)];
        }
        else if (option == "Misprint")
        {
            stringToDisplay = misprintOptions[Random.Range(0, misprintOptions.Length)];
        }
        else if (option == "Failure")
        {
            stringToDisplay = failureOptions[Random.Range(0, failureOptions.Length)];
        }
        else if (option == "Timer Done")
        {
            stringToDisplay = timerDoneOptions[Random.Range(0, timerDoneOptions.Length)];
        }
        else
        {
            stringToDisplay = minigameCompleteOptions[Random.Range(0, minigameCompleteOptions.Length)];

            // Wait a moment before displaying the minigameComplete text
            yield return new WaitForSeconds(1);
        }

        // Max speaks
        if (SpeakingTurn)
        {
            MaxSpeechBubble.SetActive(true);
            MaxSpeechText.text = stringToDisplay;

            // Swaps whose speaking turn it is
            SpeakingTurn = false;

            yield return new WaitForSeconds(speechBubbleActiveDuration);

            MaxSpeechBubble.SetActive(false);
        }
        // Ricki speaks
        else
        {
            RickiSpeechBubble.SetActive(true);
            RickiSpeechText.text = stringToDisplay;

            // Swaps whose speaking turn it is
            SpeakingTurn = true;

            yield return new WaitForSeconds(speechBubbleActiveDuration);

            RickiSpeechBubble.SetActive(false);
        }
    }

    private IEnumerator CloseMiniGameSequence()
    {
        // Stop the screen from moving
        screen.moveScreen = false;

        minigameComplete = true;

        // How long to pause for before moving the arms off the screen after the timer ends or the minigame is complete 
        yield return new WaitForSeconds(1.5f);
       
        StartCoroutine(maxArmsLerpScript.Lerp(new Vector2(maxArms.transform.position.x + 25, maxArms.transform.position.y), armsLerpDuration, false));
        StartCoroutine(rickiArmsLerpScript.Lerp(new Vector2(rickiArms.transform.position.x - 25, rickiArms.transform.position.y), armsLerpDuration, false));

        // How long to pause for before fading to black after the arms start moving
        yield return new WaitForSeconds(1);

        blackScreen.SetActive(true);
    }
}
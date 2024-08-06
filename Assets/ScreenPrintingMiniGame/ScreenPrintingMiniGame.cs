using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ScreenPrintingMiniGame : MiniGame
{
    private GameObject mainCamera;
    private bool isActive;
    private ScreenPrintingScreen screen;
    private Vector3 screenStartingPos;

    [Header("Scoring")]
    
    public int score; // The total score or income earned from the minigame

    public int successScore = 20;
    public int misprintScore = 5;
    public int failScore = -5;

    [Header("Thresholds")]

    public float successThreshold; // The margin of error required for a successful print
    public float misprintThreshold; // The margin of error required for a misprint

    [Header("Timing")]

    public float timeLimit = 30f; // How much time the player has at the start of the minigame
    public float timer = 30f; 
    public TextMeshPro timerText;
    private bool doTimer;
    private bool hasDoneTimerCheck;

    [Header("Shirt Icons")]

    public GameObject ShirtIconsParent;
    private List<ShirtIcon> shirtIcons = new List<ShirtIcon>(); // A list of all the empty shirt icons

    //public GameObject successIcon;
    //public GameObject misprintIcon;
    //public GameObject failureIcon;
    private int currentIconIdx;


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

    private Vector3 maxStartingPos;
    private Vector3 rickiStartingPos;
    private LerpPosition maxArmsLerpScript;
    private LerpPosition rickiArmsLerpScript;

    public float armsLerpDuration; // How long the arms take to move off the screen; the smaller this value is, the faster it goes

    private BlackScreen blackScreen;

    [HideInInspector] public bool minigameComplete;

    private enum PrintState {
        Success,
        Failure,
        Misprint,
        TimerDone,
        Complete
    };

    private void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        shirtIcons = ShirtIconsParent.GetComponentsInChildren<ShirtIcon>(includeInactive: true).ToList();
        screen = GetComponentInChildren<ScreenPrintingScreen>(includeInactive: true);
        screenStartingPos = screen.transform.position;
        MaxSpeechText = MaxSpeechBubble.GetComponentInChildren<Text>(includeInactive: true);
        RickiSpeechText = RickiSpeechBubble.GetComponentInChildren<Text>(includeInactive: true);
        
        maxArmsLerpScript = maxArms.GetComponent<LerpPosition>();
        maxStartingPos = maxArms.transform.position;
        rickiArmsLerpScript = rickiArms.GetComponent<LerpPosition>();
        rickiStartingPos = rickiArms.transform.position;
        DisableAllChildren();
    }

    private void OnEnable()
    {
        if (shirtIcons == null)
            Start();
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

        ResetGameState();

        screen.SpawnNewShirt();
        blackScreen.Unfade();

        // Only start the timer after the minigame has started and all its components have been set
        doTimer = true;
    }

    private void ResetGameState()
    {
        // Adding all of the shirt icons to the shirtIcons list
        foreach (ShirtIcon icon in shirtIcons)
        {
            icon.Reset();
        }
        currentIconIdx = 0;

        MaxSpeechBubble.SetActive(false);
        RickiSpeechBubble.SetActive(false);
        minigameComplete = false;
        screen.moveScreen = true;
        doTimer = false;
        timer = timeLimit;
        screen.transform.position = screenStartingPos;
        maxArms.transform.position = maxStartingPos;
        rickiArms.transform.position = rickiStartingPos;
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

            StartCoroutine(UpdateSpeechBubbles(PrintState.TimerDone));

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

            StartCoroutine(UpdateSpeechBubbles(PrintState.Success));
            UpdateIcons(PrintState.Success);
        }
        else if (distance <= misprintThreshold)
        {
            score += misprintScore;

            StartCoroutine(UpdateSpeechBubbles(PrintState.Misprint));
            UpdateIcons(PrintState.Misprint);
        }
        else
        {
            score += failScore;

            StartCoroutine(UpdateSpeechBubbles(PrintState.Failure));
            UpdateIcons(PrintState.Failure);
        }
    }

    private void UpdateIcons(PrintState result)
    {
        // Spawns the icon prefab corresponding to the result, then destroy the empty shirt icon and remove the now empty first slot from the list

        if (result == PrintState.Success)
        {
            shirtIcons[currentIconIdx++].Success(successScore);
        }
        else if (result == PrintState.Misprint)
        {
            shirtIcons[currentIconIdx++].Misprint(misprintScore);
        }
        else if (result == PrintState.Failure)
        {
            shirtIcons[currentIconIdx++].Failure(failScore);
        }

        // If the last empty icon of the list has been filled, end the minigame
        if (currentIconIdx >= shirtIcons.Count)
        {
            StartCoroutine(UpdateSpeechBubbles(PrintState.Complete));

            // Start the closing sequence
            StartCoroutine(CloseMiniGameSequence());
        }
    }

    private IEnumerator UpdateSpeechBubbles(PrintState option)
    {
        // Based on what is passed in, choose a random string from the respective array and save that to a new string, then display that string in one of the speech bubbles
        string stringToDisplay = null;

        if (option == PrintState.Success)
        {
            stringToDisplay = successOptions[Random.Range(0, successOptions.Length)];
        }
        else if (option == PrintState.Misprint)
        {
            stringToDisplay = misprintOptions[Random.Range(0, misprintOptions.Length)];
        }
        else if (option == PrintState.Failure)
        {
            stringToDisplay = failureOptions[Random.Range(0, failureOptions.Length)];
        }
        else if (option == PrintState.TimerDone)
        {
            stringToDisplay = timerDoneOptions[Random.Range(0, timerDoneOptions.Length)];
        }
        else if (option == PrintState.Complete)
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

            yield return new WaitForSeconds(speechBubbleActiveDuration);

            MaxSpeechBubble.SetActive(false);
        }
        // Ricki speaks
        else
        {
            RickiSpeechBubble.SetActive(true);
            RickiSpeechText.text = stringToDisplay;

            yield return new WaitForSeconds(speechBubbleActiveDuration);

            RickiSpeechBubble.SetActive(false);
        }
        // Swaps whose speaking turn it is
        SpeakingTurn = !SpeakingTurn;
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

        blackScreen.Fade();
    }
}
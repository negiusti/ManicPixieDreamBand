using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BandNameMiniGame : MiniGame
{
    private GameObject mainCamera;
    private bool isActive;
    public Character maxCloseup;
    public Character rickiCloseup;
    public GameObject maxSpeechBubble;
    public Text maxSpeechText;
    public GameObject rickiSpeechBubble;
    public Text rickiSpeechText;
    public BandNamePatch lbPatch;
    public BandNamePatch pjPatch;
    private BlackScreen blackScreen;
    private bool waitingForClick;
    public GameObject clickHint;
    private Camera mgCamera;
    private bool pickedPunkJuice;
    private AudioSource audioSource;

    private void Start()
    {
        mgCamera = GetComponentInChildren<Camera>(true);
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        audioSource = GetComponent<AudioSource>();
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        mainCamera = Camera.main.transform.gameObject;
        mainCamera.SetActive(false);
        EnableAllChildren();
        blackScreen.Unfade();
        MiniGameManager.PrepMiniGame();
        isActive = true;
        pickedPunkJuice = false;
        waitingForClick = true;
        clickHint.SetActive(false);
        rickiSpeechBubble.SetActive(false);
        maxSpeechBubble.SetActive(false);
        StartCoroutine(rickiBark("Go ahead. Pick one."));
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            waitingForClick = false;
        }
    }

    private IEnumerator WaitForClick(IEnumerator cr)
    {
        clickHint.SetActive(true);
        yield return new WaitForSeconds(1f);
        waitingForClick = true;
        while (waitingForClick)
        {
            yield return new WaitForSeconds(0.5f);
        }
        clickHint.SetActive(false);
        waitingForClick = true;
        StartCoroutine(cr);
        yield return null;
    }

    private IEnumerator CloseMiniGameCR()
    {
        blackScreen.Fade();
        yield return null;
    }

    public void SelectBandName(string bandName)
    {
        if (pickedPunkJuice)
            return;
        if (bandName == "LEMON BOY")
        {
            rickiSpeechBubble.SetActive(false);
            StartCoroutine(maxBark("Nah, that's a dumb name."));
            audioSource.Play();
            lbPatch.Explode();
        } else
        {
            pickedPunkJuice = true;
            maxSpeechBubble.SetActive(false);
            StartCoroutine(rickiBark("Okay, Punk Juice it is."));
            StartCoroutine(maxBark("Huh. That'd be a cool name for a video game.", true));
        }
    }

    private IEnumerator rickiBark(string speechText, bool waitFirst = false)
    {
        if (waitFirst)
            yield return new WaitForSeconds(2f);
        rickiSpeechText.text = speechText;
        rickiSpeechBubble.SetActive(true);
        yield return null;
    }

    private IEnumerator maxBark(string speechText, bool waitFirst = false)
    {
        if (waitFirst)
        {
            yield return new WaitForSeconds(.5f);
            rickiSpeechBubble.SetActive(false);
            StartCoroutine(mgCamera.GetComponent<CameraLerp>().PanCameraTo(new Vector3(8.04f, 3.37f, 0f), 6.3f, .2f));
            yield return new WaitForSeconds(.5f);
            maxCloseup.EmoteEyes("MaxStare");
            maxCloseup.FacePop();
            waitingForClick = true;
            StartCoroutine(WaitForClick(CloseMiniGameCR()));
        }
        maxSpeechText.text = speechText;
        maxSpeechBubble.SetActive(true);
        yield return null;
    }
}

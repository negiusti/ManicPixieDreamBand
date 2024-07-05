using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>();
        blackScreen.gameObject.SetActive(false);
        mgCamera = GetComponentInChildren<Camera>();
        DisableAllChildren();
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
        blackScreen.gameObject.SetActive(true);
        blackScreen.GetComponent<Animator>().Play("BlackScreenUnfade", -1, 0f);
        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
        pickedPunkJuice = false;
        waitingForClick = true;
        clickHint.SetActive(false);
        rickiSpeechBubble.SetActive(false);
        maxSpeechBubble.SetActive(false);
        StartCoroutine(rickiBark("go ahead. pick one."));
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
        if (Input.GetMouseButtonDown(0))
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
        blackScreen.gameObject.SetActive(true);
        blackScreen.GetComponent<Animator>().Play("BlackScreenFade", -1, 0f);
        yield return null;
    }

    public void SelectBandName(string bandName)
    {
        if (pickedPunkJuice)
            return;
        if (bandName == "LEMON BOY")
        {
            rickiSpeechBubble.SetActive(false);
            StartCoroutine(maxBark("nah that's a dumb name"));
            lbPatch.Explode();
        } else
        {
            pickedPunkJuice = true;
            maxSpeechBubble.SetActive(false);
            StartCoroutine(rickiBark("ok punk juice it is"));
            StartCoroutine(maxBark("huh. that'd be a cool name for a video game", true));
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
            yield return new WaitForSeconds(2f);
            rickiSpeechBubble.SetActive(false);
            StartCoroutine(mgCamera.GetComponent<CameraLerp>().PanCameraTo(new Vector3(8.22f, 3.14f, 0f), 6.3f, 1f));
            yield return new WaitForSeconds(2f);
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

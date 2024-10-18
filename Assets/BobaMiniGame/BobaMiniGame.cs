using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BobaMiniGame : MiniGame
{
    public enum Step
    {
        Milk,
        Flavor,
        Ice,
        Toppings,
        Done
    }
    private Camera cam;
    public GameObject cupTemplate;
    public BobaCup cup;
    private Step step;
    private List<Step> steps = new List<Step> { Step.Milk, Step.Flavor, Step.Ice, Step.Toppings, Step.Done };
    private int currStepIdx;
    public bool milkDone;
    public bool flavorDone;
    public bool toppingsDone;
    public bool iceDone;
    private BobaOrder order;
    public Timer timer;
    private Milk[] milks;
    private Topping[] toppings;
    private Flavor[] flavors;
    private TipJar tipJar;

    private GameObject mainCamera;
    private BlackScreen blackScreen;
    private bool isActive;
    private float tipsIncome;
    public GameObject speechBubble;
    private Text speechText;
    private int currNumMistakes;
    public string[] goodResponses; // no mistakes
    public string[] midResponses; // 1 mistake
    public string[] badResponses; // 2 or more mistakes

    public AudioClip goodJob;
    public AudioClip badJob;
    private AudioSource audioSource;
    public GameObject sweepUpMicroGame;
    public GameObject cleanSpillMicroGame;
    public GameObject bathroomCodeMicroGame;
    private Vector3 prevCamPos;
    private int microgameIdx;
    public bool TESTING_ONLY;

    // Use this for initialization
    void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        cam = GetComponentInChildren<Camera>(true);
        order = GetComponentInChildren<BobaOrder>(true);
        milks = FindObjectsOfType<Milk>(true);
        toppings = FindObjectsOfType<Topping>(true);
        flavors = FindObjectsOfType<Flavor>(true);
        speechText = speechBubble.GetComponentInChildren<Text>(true);
        tipJar = GetComponentInChildren<TipJar>(true);
        StartCoroutine(speechBubble.GetComponent<LerpPosition>().Lerp(speechBubble.transform.localPosition + Vector3.right * 35f, 1f));
        audioSource = GetComponent<AudioSource>();
        speechBubble.SetActive(false);

        DisableAllChildren();

        if(TESTING_ONLY)
            OpenMiniGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InterruptWithMicroGame()
    {
        // Save the current camera position
        prevCamPos = cam.transform.localPosition;
        // Pick a micro game
        microgameIdx++;
        if (microgameIdx % 3 == 0)
            StartCoroutine(CleanSpill());
        else if (microgameIdx % 2 == 0)
            StartCoroutine(SweepUp());
        else
            StartCoroutine(BathroomCode());
    }

    private IEnumerator BathroomCode()
    {
        bathroomCodeMicroGame.gameObject.SetActive(true);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(bathroomCodeMicroGame.transform.localPosition + Vector3.back * 10f, 0.5f));
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
        yield return new WaitForSeconds(0.5f);
        bathroomCodeMicroGame.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator CleanSpill()
    {
        cleanSpillMicroGame.gameObject.SetActive(true);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cleanSpillMicroGame.transform.localPosition + Vector3.back * 10f, 0.5f));
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
        yield return new WaitForSeconds(0.5f);
        cleanSpillMicroGame.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator SweepUp()
    {
        sweepUpMicroGame.gameObject.SetActive(true);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(sweepUpMicroGame.transform.localPosition + Vector3.back * 10f, 0.5f));
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
        yield return new WaitForSeconds(0.5f);
        sweepUpMicroGame.gameObject.SetActive(false);
        yield return null;
    }

    //private void CloseMicroGame()
    //{
    //    StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
    //}

    public void Next(string choice)
    {
        if(order.CheckOrderItem(step, choice))
        {
            //tipsIncome += 1f;
        } else
        {
            currNumMistakes++;
        }
        StartCoroutine(NextPhase());
    }

    private IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(1.5f);
        currStepIdx++;
        step = steps[currStepIdx];
        // INTERRUPT
        if (step != Step.Done)
        {
            InterruptWithMicroGame();
            yield return new WaitForSeconds(5f);
        }
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.right * 35f, 0.5f));
        StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 0.5f));
        yield return new WaitForSeconds(0.5f);

        if (step == Step.Done)
        {
            cup.GetComponent<Animator>().Play("LidAndStraw");
            speechBubble.SetActive(true);
            StartCoroutine(speechBubble.GetComponent<LerpPosition>().Lerp(speechBubble.transform.localPosition + Vector3.left * 35f, 1f));
            if (currNumMistakes == 0)
            {
                audioSource.clip = goodJob;
                audioSource.Play();
                JobSystem.GoodJob();
                speechText.text = goodResponses[Random.Range(0, goodResponses.Length)];
                tipJar.addTip(5f);
                tipsIncome += 5f;
            }
            else if (currNumMistakes == 1)
            {
                speechText.text = midResponses[Random.Range(0, midResponses.Length)];
                tipJar.addTip(1f);
                tipsIncome += 1f;
            } else
            {
                audioSource.clip = badJob;
                audioSource.Play();
                JobSystem.BadJob();
                speechText.text = badResponses[Random.Range(0, badResponses.Length)];
                tipJar.addTip(0f);
            }
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 1f, true));
            yield return new WaitForSeconds(1.75f);
            
            if (step == Step.Done && !timer.IsRunning())
            {
                StartCoroutine(CloseMiniGameSequence());
                yield return null;
            }
            else
            {
                StartCoroutine(speechBubble.GetComponent<LerpPosition>().Lerp(speechBubble.transform.localPosition + Vector3.right * 35f, 1f));
                speechBubble.SetActive(false);
                StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.left * 35f * 4f, 0.5f));
                NewOrder();
            }
        }
        yield return null;
    }

    public Step CurrentStep()
    {
        return step;
    }

    private void NewOrder()
    {
        currNumMistakes = 0;
        currStepIdx = 0;
        step = steps[currStepIdx];
        milkDone = false;
        flavorDone = false;
        toppingsDone = false;
        iceDone = false;
        order.RandomizeOrder();
        cup = Instantiate(cupTemplate, transform).GetComponent<BobaCup>();
        cup.gameObject.SetActive(true);
        tipJar.HideTipText();
        for (int i = 0; i < 3; i++)
        {
            milks[i].ResetPosition();
            flavors[i].ResetPosition();
            toppings[i].ResetPosition();
        }
    }

    public override void OpenMiniGame()
    {
        if (!TESTING_ONLY)
        {
            mainCamera = Camera.main.transform.gameObject;
            mainCamera.SetActive(false);
        }

        EnableAllChildren();
        cleanSpillMicroGame.gameObject.SetActive(false);
        sweepUpMicroGame.gameObject.SetActive(false);
        bathroomCodeMicroGame.gameObject.SetActive(false);
        if (!TESTING_ONLY)
            MiniGameManager.PrepMiniGame();
        isActive = true;

        blackScreen.Unfade();
        tipsIncome = 0;
        timer.Reset();
        timer.StartTimer();
        NewOrder();
    }

    public override void CloseMiniGame()
    {
        if (TESTING_ONLY)
        {
            return;
        }
        mainCamera.SetActive(true);
        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();

        // Add the player's score they got into their bank account
        MainCharacterState.ModifyBankBalance(tipsIncome + 100);
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    private IEnumerator CloseMiniGameSequence()
    {
        // Wait a moment before fading to black
        yield return new WaitForSeconds(1f);

        // Start fading to black
        blackScreen.Fade();
    }
}

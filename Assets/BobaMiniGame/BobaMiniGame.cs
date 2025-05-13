using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BobaMiniGame : MiniGame
{
    public enum Step
    {
        Milk,
        Flavor,
        Ice,
        Toppings,
        Done,
        Microgame
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
    public GameObject cameraUI;

    private GameObject mainCamera;
    private BlackScreen blackScreen;
    private bool isActive;
    private float tipsIncome;
    public GameObject speechBubble;
    private TextMeshProUGUI speechText;
    private int currNumMistakes;
    public List<GameObject> customers;
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
    private bool microgameDone;

    private static float microgameCameraPos = -72.1f;
    private static float milkCameraPosX = -32.7f;
    private static float flavorCameraPosX = 2.3f;
    private static float iceCameraPosX = 37.4f;
    private static float toppingsCameraPosX = 72.6f;
    private static float doneCameraPosX = 108.1f;

    private static Dictionary<Step, float> stepToPos = new Dictionary<Step, float> {
        {Step.Done, doneCameraPosX},
        {Step.Flavor, flavorCameraPosX},
        {Step.Toppings, toppingsCameraPosX},
        {Step.Milk, milkCameraPosX},
        {Step.Ice, iceCameraPosX},
        {Step.Microgame, microgameCameraPos},};

    // Use this for initialization
    void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        cam = GetComponentInChildren<Camera>(true);
        order = GetComponentInChildren<BobaOrder>(true);
        milks = FindObjectsOfType<Milk>(true);
        toppings = FindObjectsOfType<Topping>(true);
        flavors = FindObjectsOfType<Flavor>(true);
        speechText = speechBubble.GetComponentInChildren<TextMeshProUGUI>(true);
        tipJar = GetComponentInChildren<TipJar>(true);
        StartCoroutine(speechBubble.GetComponent<LerpPosition>().Lerp(speechBubble.transform.localPosition + Vector3.right * 35f, 1f));
        audioSource = GetComponent<AudioSource>();
        speechBubble.SetActive(false);
        customers.ForEach(c => c.SetActive(true));
        cam.gameObject.transform.localPosition = new Vector3(stepToPos[Step.Milk], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z);
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
        microgameDone = false;
        // Save the current camera position
        prevCamPos = cam.transform.localPosition;
        // Pick a micro game
        microgameIdx++;
        timer.Reset();
        timer.StartTimer();
        if (microgameIdx % 3 == 0)
            StartCoroutine(CleanSpill());
        else if (microgameIdx % 2 == 0)
            StartCoroutine(SweepUp());
        else
            StartCoroutine(BathroomCode());
    }

    public void Oops()
    {
        audioSource.clip = badJob;
        audioSource.Play();
        cam.GetComponent<CameraShaker>().CameraShake();
    }

    public void Yay() {
        audioSource.clip = goodJob;
        audioSource.Play();
    }

    private void DisableMicroGames() {
        bathroomCodeMicroGame.gameObject.SetActive(false);
        cleanSpillMicroGame.gameObject.SetActive(false);
        sweepUpMicroGame.gameObject.SetActive(false);
    }

    private IEnumerator BathroomCode()
    {
        bathroomCodeMicroGame.gameObject.SetActive(true);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[Step.Microgame], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        yield return new WaitForSeconds(7.5f);
        // StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[step], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        // StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
        yield return new WaitForSeconds(1.5f);
        bathroomCodeMicroGame.gameObject.SetActive(false);
        tipJar.HideTipText();
        yield return null;
    }

    private IEnumerator CleanSpill()
    {
        cleanSpillMicroGame.gameObject.SetActive(true);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[Step.Microgame], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        yield return new WaitForSeconds(7.5f);
        // StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[step], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        
        // StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
        yield return new WaitForSeconds(1.5f);
        cleanSpillMicroGame.gameObject.SetActive(false);
        tipJar.HideTipText();
        yield return null;
    }

    private IEnumerator SweepUp()
    {
        sweepUpMicroGame.gameObject.SetActive(true);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[Step.Microgame], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        yield return new WaitForSeconds(7.5f);
        // StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[step], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        // StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
        yield return new WaitForSeconds(1.5f);
        sweepUpMicroGame.gameObject.SetActive(false);
        tipJar.HideTipText();
        yield return null;
    }

    //private void CloseMicroGame()
    //{
    //    StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(prevCamPos, 0.5f));
    //}

    public void Next(string choice)
    {
        if(!order.CheckOrderItem(step, choice))
        {
            currNumMistakes++;
        }
        StartCoroutine(NextPhase());
    }

    public void MicrogameDone() {
        microgameDone = true;
    }

    private IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(1.5f);
        currStepIdx++;
        step = steps[currStepIdx];
        // INTERRUPT
        if (step != Step.Done)
        {
            cameraUI.SetActive(false);
            InterruptWithMicroGame();
            while (!microgameDone) {
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(1.5f);
            cameraUI.SetActive(true);
        }
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[step], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[step], cup.gameObject.transform.localPosition.y, cup.gameObject.transform.localPosition.z), 0.5f));
        yield return new WaitForSeconds(0.4f);
        DisableMicroGames();
        yield return new WaitForSeconds(0.1f);

        if (step == Step.Done)
        {
            cup.GetComponent<Animator>().Play("LidAndStraw");
            speechBubble.SetActive(true);
            StartCoroutine(speechBubble.GetComponent<LerpPosition>().Lerp(speechBubble.transform.localPosition + Vector3.left * 35f, 1f));
            if (currNumMistakes == 0)
            {
                Yay();
                JobSystem.GoodJob();
                speechText.text = goodResponses[Random.Range(0, goodResponses.Length)];
                tipJar.addTip(10f);
                tipsIncome += 10f;
            }
            else if (currNumMistakes == 1)
            {
                speechText.text = midResponses[Random.Range(0, midResponses.Length)];
                tipJar.addTip(5f);
                tipsIncome += 5f;
            } else
            {
                Oops();
                JobSystem.BadJob();
                speechText.text = badResponses[Random.Range(0, badResponses.Length)];
                tipJar.addTip(0f);
            }
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 1f, true));
            yield return new WaitForSeconds(1.75f);
            
            if (step == Step.Done && customers.FindAll(c => c.activeSelf).Count <= 0)
            {
                StartCoroutine(CloseMiniGameSequence());
                yield return null;
            }
            else
            {
                StartCoroutine(speechBubble.GetComponent<LerpPosition>().Lerp(speechBubble.transform.localPosition + Vector3.right * 35f, 1f));
                speechBubble.SetActive(false);
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
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(new Vector3(stepToPos[Step.Milk], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z), 0.5f));
        if (customers.Exists(c => c.activeSelf))
            customers.Find(c => c.activeSelf).SetActive(false);
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

    public void addTip(float amt) {
        tipJar.addTip(amt);
        tipsIncome += amt;
    }

    public override void OpenMiniGame()
    {
        if (!TESTING_ONLY)
        {
            mainCamera = Camera.main.transform.gameObject;
            mainCamera.SetActive(false);
        }

        EnableAllChildren();
        cam.gameObject.transform.localPosition = new Vector3(stepToPos[Step.Milk], cam.gameObject.transform.localPosition.y, cam.gameObject.transform.localPosition.z);
        cleanSpillMicroGame.gameObject.SetActive(false);
        sweepUpMicroGame.gameObject.SetActive(false);
        bathroomCodeMicroGame.gameObject.SetActive(false);
        if (!TESTING_ONLY)
            MiniGameManager.PrepMiniGame();
        isActive = true;

        blackScreen.Unfade();
        tipsIncome = 0;
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

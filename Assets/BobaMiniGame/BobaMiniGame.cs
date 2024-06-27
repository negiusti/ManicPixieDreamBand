using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Use this for initialization
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        order = GetComponentInChildren<BobaOrder>();
        milks = FindObjectsOfType<Milk>();
        toppings = FindObjectsOfType<Topping>();
        flavors = FindObjectsOfType<Flavor>();
        NewOrder();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next(string choice)
    {
        order.CheckOrderItem(step, choice);
        StartCoroutine(NextPhase());
    }

    private IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(1.5f);
        currStepIdx++;
        step = steps[currStepIdx];
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.right * 35f, 0.5f));
        StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        if (step == Step.Done)
        {
            cup.GetComponent<Animator>().Play("LidAndStraw");
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 1f, true));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.left * 35f * 4f, 0.5f));
            NewOrder();
        }
        yield return null;
    }

    private void NewOrder()
    {
        currStepIdx = 0;
        step = steps[currStepIdx];
        milkDone = false;
        flavorDone = false;
        toppingsDone = false;
        iceDone = false;
        order.RandomizeOrder();
        cup = Instantiate(cupTemplate, transform).GetComponent<BobaCup>();
        cup.gameObject.SetActive(true);
        for(int i = 0; i < 3; i++)
        {
            milks[i].ResetPosition();
            flavors[i].ResetPosition();
            toppings[i].ResetPosition();
        }
    }

    public override void OpenMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public override void CloseMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsMiniGameActive()
    {
        throw new System.NotImplementedException();
    }
}

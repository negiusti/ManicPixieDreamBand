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
    public GameObject cup;
    private Step step;
    private List<Step> steps = new List<Step> { Step.Milk, Step.Flavor, Step.Ice, Step.Toppings, Step.Done };
    private int currStepIdx;
    public bool milkDone;
    public bool flavorDone;
    public bool toppingsDone;
    // Use this for initialization
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        currStepIdx = 0;
        step = steps[currStepIdx];
        milkDone = false;
        flavorDone = false;
        toppingsDone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        currStepIdx++;
        step = steps[currStepIdx];
        StartCoroutine(NextPhase());
    }

    private IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.right * 35f, 0.5f));
        StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 0.5f));
        yield return new WaitForSeconds(2f);
        if (step == Step.Done)
        {
            cup.GetComponent<Animator>().Play("LidAndStraw");
        }
        yield return null;
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

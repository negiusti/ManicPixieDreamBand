using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BobaMiniGame : MiniGame
{
    private enum Step
    {
        Milk,
        Flavor,
        Ice,
        Toppings
    }
    private Camera cam;
    public GameObject cup;
    private Step step;
    private List<Step> steps = new List<Step> { Step.Milk, Step.Flavor, Step.Ice, Step.Toppings };
    private int currStepIdx;
    public bool milkDone;
    public bool flavorDone;
    // Use this for initialization
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        currStepIdx = 0;
        step = steps[currStepIdx];
        milkDone = false;
        flavorDone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        Debug.Log("next phase");
        currStepIdx++;
        step = steps[currStepIdx];
        StartCoroutine(NextPhase());
    }

    private IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.right * 35f, 0.5f));
        StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 35f, 0.5f));
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

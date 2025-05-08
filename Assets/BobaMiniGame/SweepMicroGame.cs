using System.Collections;
using UnityEngine;

public class SweepMicroGame : MonoBehaviour
{
    private int sweptParticles;
    private Coroutine checkRoutine;
    public GameObject nice;
    public GameObject ugh;
    private BobaMiniGame mg;
    public Timer timer;
    private bool done;


    void Start()
    {
        mg = GetComponentInParent<BobaMiniGame>();
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        if (checkRoutine != null)
        {
            StopCoroutine(checkRoutine);
            checkRoutine = null;
        }
    }

    private IEnumerator EraseCompletionChecker()
    {
        while (true)
        {
            if (sweptParticles >= 9 && !done) {
                done = true;
                mg.Yay();
                mg.addTip(3f);
                nice.SetActive(true);
            } else if (timer.TimeRemaining() <= 1 && !done) {
                done = true;
                ugh.SetActive(true);
                mg.addTip(-1f);
                mg.Oops();
            }
            yield return new WaitForSeconds(.3f);
        }
    }

    private void OnEnable()
    {
        sweptParticles = 0;
        checkRoutine = StartCoroutine(EraseCompletionChecker());
        nice.SetActive(false);
        ugh.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseCount()
    {
        sweptParticles++;
    }

    public void DecreaseCount()
    {
        sweptParticles--;
        if (sweptParticles < 0)
            sweptParticles = 0;
    }
}

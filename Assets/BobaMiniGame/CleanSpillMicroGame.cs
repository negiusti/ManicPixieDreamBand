using System.Collections;
using UnityEngine;

public class CleanSpillMicroGame : MonoBehaviour
{
    private Eraser sponge;
    private Coroutine checkEraseCoroutine;
    public GameObject nice;
    public GameObject ugh;
    private BobaMiniGame mg;
    public Timer timer;
    private bool done;


    // Start is called before the first frame update
    void Start()
    {
        sponge = GetComponentInChildren<Eraser>();
        mg = GetComponentInParent<BobaMiniGame>();
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        checkEraseCoroutine = StartCoroutine(EraseCompletionChecker());
        nice.SetActive(false);
        ugh.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        done = false;
    }

    private void OnDisable()
    {
        if (checkEraseCoroutine != null)
        {
            StopCoroutine(checkEraseCoroutine);
            checkEraseCoroutine = null;
        }
    }

    private IEnumerator EraseCompletionChecker()
    {
        if (sponge == null)
            Start();

        while (true)
        {
            if (sponge.EraseCompletionPercentage() > .97f && !done) {
                done = true;
                mg.addTip(3f);
                mg.Yay();
                nice.SetActive(true);
                mg.MicrogameDone();
            } else if (timer.TimeRemaining() <= 1 && !done) {
                done = true;
                mg.Oops();
                mg.addTip(-1f);
                ugh.SetActive(true);
                mg.MicrogameDone();
            }
            yield return new WaitForSeconds(.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

using System.Collections;
using UnityEngine;

public class SweepMicroGame : MonoBehaviour
{
    private int sweptParticles;
    private Coroutine checkRoutine;
    public GameObject nice;
    private BobaMiniGame mg;

    void Start()
    {
        mg = GetComponentInParent<BobaMiniGame>();
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
            if (sweptParticles >= 9) {
                if (!nice.activeSelf) {
                    mg.addTip(3f);
                }
                nice.SetActive(true);
            }
            yield return new WaitForSeconds(.3f);
        }
    }

    private void OnEnable()
    {
        sweptParticles = 0;
        checkRoutine = StartCoroutine(EraseCompletionChecker());
        nice.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
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

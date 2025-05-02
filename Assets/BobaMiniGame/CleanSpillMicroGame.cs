using System.Collections;
using UnityEngine;

public class CleanSpillMicroGame : MonoBehaviour
{
    private Eraser sponge;
    private Coroutine checkEraseCoroutine;
    public GameObject nice;
    private BobaMiniGame mg;

    // Start is called before the first frame update
    void Start()
    {
        sponge = GetComponentInChildren<Eraser>();
        mg = GetComponentInParent<BobaMiniGame>();
    }

    private void OnEnable()
    {
        checkEraseCoroutine = StartCoroutine(EraseCompletionChecker());
        nice.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
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
            if (sponge.EraseCompletionPercentage() > .97f) {
                if (!nice.activeSelf) {
                    mg.addTip(3f);
                }
                nice.SetActive(true);
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

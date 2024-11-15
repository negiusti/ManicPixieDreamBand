using System.Collections;
using UnityEngine;

public class CleanSpillMicroGame : MonoBehaviour
{
    private Eraser sponge;
    private Coroutine checkEraseCoroutine;
    public GameObject nice;

    // Start is called before the first frame update
    void Start()
    {
        sponge = GetComponentInChildren<Eraser>();
    }

    private void OnEnable()
    {
        checkEraseCoroutine = StartCoroutine(EraseCompletionChecker());
        nice.SetActive(false);
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
            if (sponge.EraseCompletionPercentage() > .97f)
                nice.SetActive(true);
            yield return new WaitForSeconds(.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

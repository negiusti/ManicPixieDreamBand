using UnityEngine;
using System.Collections;

public class BobaMiniGame : MonoBehaviour
{
    private Camera cam;
    public GameObject cup;
    // Use this for initialization
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        Debug.Log("next phase");
        StartCoroutine(NextPhase());
    }

    private IEnumerator NextPhase()
    {
        Debug.Log("start wait");
        yield return new WaitForSeconds(2.5f);
        Debug.Log("end wait");
        StartCoroutine(cam.gameObject.GetComponent<LerpPosition>().Lerp(cam.transform.localPosition + Vector3.right * 20f, 0.5f));
        StartCoroutine(cup.GetComponent<LerpPosition>().Lerp(cup.transform.localPosition + Vector3.right * 20f, 0.5f));
        yield return null;
    }
}

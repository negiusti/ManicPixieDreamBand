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
        cam.transform.Translate(Vector3.right * 20f);
        cup.transform.Translate(Vector3.right * 20f);
    }
}

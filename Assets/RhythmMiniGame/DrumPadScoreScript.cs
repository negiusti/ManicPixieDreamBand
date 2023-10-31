using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrumPadScoreScript : MonoBehaviour
{
    //public GameObject result;
    public TextMeshPro text;
    private Vector3 originalScale;
    //private bool isInsideTrigger = false;
    public float scaleFactor = 0.1f; // 10% scale factor change
    public float pulseSpeed = 2.0f; // Adjust the speed of the pulse
    // Start is called before the first frame update
    void Start()
    {
        //text = result.GetComponent<TextMeshPro>();
        originalScale = text.transform.localScale;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        float scaleChange = Mathf.Sin(Time.time * pulseSpeed) * scaleFactor;
        Vector3 newScale = originalScale + Vector3.one * scaleChange;
        text.transform.localScale = newScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("stick"))
            text.text = "niiiiice";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("stick"))
            text.text = "booooo u suck";
    }
}

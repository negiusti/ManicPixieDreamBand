using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHintScript : MonoBehaviour
{
    public GameObject interactHint;
    public string sceneToTrigger;
    public KeyCode keyToTrigger;
    private SceneChanger sc;

    private Vector3 originalScale;
    //private bool isInsideTrigger = false;
    public float scaleFactor = 0.1f; // 10% scale factor change
    public float pulseSpeed = 2.0f; // Adjust the speed of the pulse

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        interactHint.SetActive(false);
        sc = FindObjectOfType<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactHint.activeSelf && Input.GetKey(keyToTrigger))
        {
            if (sceneToTrigger != null && sceneToTrigger.Length > 0)
                sc.ChangeScene(sceneToTrigger);
        }
        if (interactHint.activeSelf)
        {
            // Calculate the new scale based on a sine wave
            float scaleChange = Mathf.Sin(Time.time * pulseSpeed) * scaleFactor;
            Vector3 newScale = originalScale + Vector3.one * scaleChange;
            transform.localScale = newScale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactHint.SetActive(false);
            transform.localScale = originalScale;
        }
    }
}

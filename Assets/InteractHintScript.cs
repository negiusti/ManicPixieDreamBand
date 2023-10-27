using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHintScript : MonoBehaviour
{
    public GameObject interactHint;
    public string sceneToTrigger;
    public KeyCode keyToTrigger;
    private SceneChanger sc;
    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
}

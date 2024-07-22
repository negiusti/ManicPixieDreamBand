using TMPro;
using UnityEngine;

public class OutdoorLocation : MonoBehaviour
{
    private bool inRange;
    private string location;
    private GameObject prompt;
    private Animator sign;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        location = gameObject.name;
        prompt = GetComponentInChildren<Pulsate>().gameObject;
        prompt.SetActive(false);
        sign = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inRange)
        {
            SceneChanger.Instance.ChangeScene(location);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            prompt.SetActive(true);
            if (sign != null)
                sign.SetBool("Show", true);
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            prompt.SetActive(false);
            if (sign != null)
                sign.SetBool("Show", false);
            inRange = false;
        }
    }
}
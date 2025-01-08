using PixelCrushers.DialogueSystem;
using UnityEngine;

public class OutdoorLocation : MonoBehaviour
{
    private bool inRange;
    private string location;
    private GameObject prompt;
    private Animator sign;
    public bool isBusiness;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        location = gameObject.name;
        Pulsate p = GetComponentInChildren<Pulsate>();
        if (p != null)
            prompt = p.gameObject;

        if(prompt != null)
            prompt.SetActive(false);
        sign = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && DialogueManager.IsConversationActive)
        {
            prompt.SetActive(false);
            if (sign != null)
                sign.SetBool("Show", false);
            inRange = false;
            return;
        }
        if ((Input.GetKeyDown(KeyCode.Return)  || Input.GetKeyDown(KeyCode.Space)) && inRange)
        {
            if (isBusiness)
                GameManager.miscSoundEffects.Play("businessdoor");
            else
                GameManager.miscSoundEffects.Play("Door");
            SceneChanger.Instance.ChangeScene(location);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !DialogueManager.IsConversationActive)
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
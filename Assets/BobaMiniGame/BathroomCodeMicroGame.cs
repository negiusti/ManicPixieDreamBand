using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BathroomCodeMicroGame : MonoBehaviour
{
    public TextMeshPro bathroomCode;
    public TextMeshPro userInput;
    public Text speechBubble;
    public Timer timer;
    private bool done;
    public GameObject nice;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        nice.SetActive(false);
        speechBubble.text = "What's the bathroom code??\nI'm burstin!!!!!";
        speechBubble.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(true);
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bathroomCode.text == userInput.text && !done)
        {
            done = true;
            speechBubble.text = "THANK YOU!!";
            speechBubble.gameObject.SetActive(false);
            speechBubble.gameObject.SetActive(true);
            Debug.Log(bathroomCode.text + " vs " + userInput.text);
        }
        else if (timer.TimeRemaining() <= 1 && !done)
        {
            done = true;
            speechBubble.text = "Oh no!! These are brand\nnew pants!! T_T";
            speechBubble.gameObject.SetActive(false);
            speechBubble.gameObject.SetActive(true);
            Debug.Log(bathroomCode.text + " vs " + userInput.text);
        }
    }
}

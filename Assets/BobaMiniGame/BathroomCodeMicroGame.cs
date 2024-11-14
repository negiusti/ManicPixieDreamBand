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

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        speechBubble.text = "What's the bathroom code??\nI'm burstin!!!!!";
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bathroomCode.text == userInput.text && !done)
        {
            done = true;
            speechBubble.text = "THANK YOU!!";
            Debug.Log(bathroomCode.text + " vs " + userInput.text);
        }
        else if (timer.TimeRemaining() <= 0 && !done)
        {
            done = true;
            speechBubble.text = "Oh no!! These are new pants!! T_T";
            Debug.Log(bathroomCode.text + " vs " + userInput.text);
        }
    }
}

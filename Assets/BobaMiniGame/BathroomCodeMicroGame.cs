using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BathroomCodeMicroGame : MonoBehaviour
{
    public TextMeshPro bathroomCode;
    public TextMeshPro userInput;
    public TextMeshProUGUI speechBubble;
    public Timer timer;
    private bool done;
    public GameObject nice;
    public GameObject ugh;
    private BobaMiniGame mg;

    // Start is called before the first frame update
    void Start()
    {
        mg = GetComponentInParent<BobaMiniGame>();
    }

    private void OnEnable()
    {
        nice.SetActive(false);
        ugh.SetActive(false);
        speechBubble.text = "What's the\nbathroom\ncode??\nI'm burstin'!!";
        speechBubble.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(true);
        done = false;
        Cursor.lockState = CursorLockMode.None;
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
            mg.addTip(5f);
            mg.Yay();
            mg.MicrogameDone();
        }
        else if (timer.TimeRemaining() <= 1 && !done)
        {
            done = true;
            speechBubble.text = "Oh no!!\nThese are\nbrand new\npants!!";
            speechBubble.gameObject.SetActive(false);
            speechBubble.gameObject.SetActive(true);
            Debug.Log(bathroomCode.text + " vs " + userInput.text);
            mg.addTip(-1f);
            mg.Oops();
            mg.MicrogameDone();
        }
    }
}

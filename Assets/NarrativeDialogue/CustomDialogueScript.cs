using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CustomDialogueScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool isCoolDown;
    private int coolDown = 1;

    // Start is called before the first frame update
    void Start()
    {
        isCoolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode) && !isCoolDown) {
            DialogueManager.standardDialogueUI.OnContinue();
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        isCoolDown = false;
    }

    public void OnConversationLine(Subtitle subtitle)
    {
        if (subtitle.dialogueEntry.DialogueText.Length == 0 && subtitle.dialogueEntry.Title != "START") {
            Debug.Log("Continuing after empty line of dialogue!!");
            DialogueManager.standardDialogueUI.OnContinue();
        }
    }
}

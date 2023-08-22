using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CustomDialogueScript : MonoBehaviour
{
    //AbstractDialogueUI dialogueUI;

    // Start is called before the first frame update
    void Start()
    {
        //dialogueUI = GetComponentInChildren<AbstractDialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConversationLine(Subtitle subtitle)
    {
        //Debug.Log(subtitle.dialogueEntry.DialogueText);
        if (subtitle.dialogueEntry.DialogueText.Length == 0 && subtitle.dialogueEntry.Title != "START") {
            Debug.Log("Continuing after empty line of dialogue!!");
            DialogueManager.standardDialogueUI.OnContinue();
            //dialogueUI.OnContinue();
        }
    }

    //public void Test()
    //{
    //    Debug.Log("HELLO!!");
    //    DialogueManager.standardDialogueUI.OnContinue();
    //    //dialogueUI.OnContinue();
    //    //dialogueUI.OnContinueConversation();
    //    //DialogueManager.PlaySequence("Continue()");
    //}
}

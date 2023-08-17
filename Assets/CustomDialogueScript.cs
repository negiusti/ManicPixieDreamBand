using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CustomDialogueScript : MonoBehaviour
{
    AbstractDialogueUI dialogueUI;

    // Start is called before the first frame update
    void Start()
    {
        dialogueUI = GetComponentInChildren<AbstractDialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConversationLine(Subtitle subtitle)
    {        
        if (subtitle.dialogueEntry.DialogueText.Length == 0) {
            Debug.Log("HELLOo!!");
            dialogueUI.OnContinue();
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

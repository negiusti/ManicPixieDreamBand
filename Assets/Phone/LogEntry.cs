using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class is the UI template for a single subtitle in the back log.
/// </summary>
public class LogEntry : MonoBehaviour
{
    public Text dialogueText;
    public string text;
    public bool isFirstTxt;
    public bool isGroupChat;
    public bool isPlayer;
    public string speakerName;
    public int entryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT/Band" };

    public void Assign(Subtitle subtitle)
    {
        if(!subtitle.speakerInfo.IsPlayer)
            dialogueText.transform.localRotation = new Quaternion(dialogueText.transform.localRotation.x, 180f, dialogueText.transform.localRotation.z, dialogueText.transform.localRotation.w);
        //speakerName.text = subtitle.speakerInfo.Name;
        //dialogueText.resizeTextForBestFit = true;
        dialogueText.text = subtitle.formattedText.text;
        //speakerName.rectTransform.sizeDelta = new Vector2(speakerName.preferredWidth, speakerName.preferredHeight);
        dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, dialogueText.preferredHeight);

        text = dialogueText.text;
        isFirstTxt = subtitle.dialogueEntry.Title.Equals("FIRST");
        isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
        isPlayer = subtitle.speakerInfo.IsPlayer;
        speakerName = subtitle.speakerInfo.Name;
        entryID = subtitle.dialogueEntry.id;
    }

    public void Assign(LogEntryData entry)
    {
        if (!entry.isPlayer)
            dialogueText.transform.localRotation = new Quaternion(dialogueText.transform.localRotation.x, 180f, dialogueText.transform.localRotation.z, dialogueText.transform.localRotation.w);
        dialogueText.text = entry.text;
        dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, dialogueText.preferredHeight);


        isFirstTxt = entry.isFirstTxt;
        isGroupChat = entry.isGroupChat;
        isPlayer = entry.isPlayer;
        speakerName = entry.speakerName;
        entryID = entry.entryID;
        text = entry.text;
    }
}

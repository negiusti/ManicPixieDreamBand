using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.U2D.Animation;

/// <summary>
/// This class is the UI template for a single subtitle in the back log.
/// </summary>
public class LogEntry : MonoBehaviour
{
    public Text dialogueText;
    public string text;

    private SpriteRenderer sr;
    private SpriteResolver resolver;
    public Image image;
    
    public bool isFirstTxt;
    public bool isGroupChat;
    public bool isPlayer;
    public string speakerName;
    public int entryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT/Band" };


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(true);
        resolver = GetComponentInChildren<SpriteResolver>(true);
    }

    //public void Assign(Subtitle subtitle)
    //{
    //    if (dialogueText == null)
    //        Debug.LogError("dialogueText is null");

    //    if (!subtitle.speakerInfo.IsPlayer)
    //        dialogueText.transform.localRotation = new Quaternion(dialogueText.transform.localRotation.x, 180f, dialogueText.transform.localRotation.z, dialogueText.transform.localRotation.w);
    //    //speakerName.text = subtitle.speakerInfo.Name;
    //    //dialogueText.resizeTextForBestFit = true;
    //    dialogueText.text = subtitle.formattedText.text;
    //    //speakerName.rectTransform.sizeDelta = new Vector2(speakerName.preferredWidth, speakerName.preferredHeight);
    //    dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, dialogueText.preferredHeight);

    //    text = dialogueText.text;
    //    isFirstTxt = subtitle.dialogueEntry.Title.Equals("FIRST");
    //    isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
    //    isPlayer = subtitle.speakerInfo.IsPlayer;
    //    speakerName = subtitle.speakerInfo.Name;
    //    entryID = subtitle.dialogueEntry.id;
    //}

    public void Assign(Subtitle subtitle)
    {
        if (subtitle.formattedText.text.StartsWith("_"))
        {
            if (resolver == null)
                Start();
            if (isPlayer)
                image.transform.localRotation = new Quaternion(image.transform.localRotation.x, 180f, image.transform.localRotation.z, image.transform.localRotation.w);
            resolver.SetCategoryAndLabel("Pics", subtitle.formattedText.text);
            resolver.ResolveSpriteToSpriteRenderer();
            image.sprite = sr.sprite;
        }
        else
        {
            if (!subtitle.speakerInfo.IsPlayer)
                dialogueText.transform.localRotation = new Quaternion(dialogueText.transform.localRotation.x, 180f, dialogueText.transform.localRotation.z, dialogueText.transform.localRotation.w);
            dialogueText.text = subtitle.formattedText.text;
            dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, dialogueText.preferredHeight);
        }
        text = subtitle.formattedText.text;
        isFirstTxt = subtitle.dialogueEntry.Title.Equals("FIRST");
        isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
        isPlayer = subtitle.speakerInfo.IsPlayer;
        speakerName = subtitle.speakerInfo.Name;
        entryID = subtitle.dialogueEntry.id;
    }

    //public void Assign(string img, string speaker, bool isMC, bool isFirst)
    //{
    //    if (image == null)
    //        Debug.LogError("Image is null");

    //    if(isPlayer)
    //        image.transform.localRotation = new Quaternion(image.transform.localRotation.x, 180f, image.transform.localRotation.z, image.transform.localRotation.w);

    //    resolver.SetCategoryAndLabel("Pics", imgName);
    //    resolver.ResolveSpriteToSpriteRenderer();
    //    image.sprite = sr.sprite;
    //    imgName = img;

    //    isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
    //    speakerName = speaker;
    //    isFirstTxt = isFirst;
    //    isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
    //    isPlayer = isMC;
    //    entryID = subtitle.dialogueEntry.id;
    //}

    public void Assign(LogEntryData entry)
    {
        if (entry.text.StartsWith("_"))
        {
            if (resolver == null)
                Start();
            if (isPlayer)
                image.transform.localRotation = new Quaternion(image.transform.localRotation.x, 180f, image.transform.localRotation.z, image.transform.localRotation.w);

            resolver.SetCategoryAndLabel("Pics", entry.text);
            resolver.ResolveSpriteToSpriteRenderer();
            image.sprite = sr.sprite;
        } else 
        {
            if (!entry.isPlayer)
                dialogueText.transform.localRotation = new Quaternion(dialogueText.transform.localRotation.x, 180f, dialogueText.transform.localRotation.z, dialogueText.transform.localRotation.w);
            dialogueText.text = entry.text;
            dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, dialogueText.preferredHeight);
        }
        text = entry.text;
        isFirstTxt = entry.isFirstTxt;
        isGroupChat = entry.isGroupChat;
        isPlayer = entry.isPlayer;
        speakerName = entry.speakerName;
        entryID = entry.entryID;
    }
}

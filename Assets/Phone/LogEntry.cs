using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// This class is the UI template for a single subtitle in the back log.
/// </summary>
public class LogEntry : MonoBehaviour
{
    //public Text speakerName;
    public Text dialogueText;

    public void Assign(Subtitle subtitle)
    {
        if(!subtitle.speakerInfo.IsPlayer)
            dialogueText.transform.localRotation = new Quaternion(dialogueText.transform.localRotation.x, 180f, dialogueText.transform.localRotation.z, dialogueText.transform.localRotation.w);
        //speakerName.text = subtitle.speakerInfo.Name;
        //dialogueText.resizeTextForBestFit = true;
        dialogueText.text = subtitle.formattedText.text;
        //speakerName.rectTransform.sizeDelta = new Vector2(speakerName.preferredWidth, speakerName.preferredHeight);
        dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, dialogueText.preferredHeight);
    }
}

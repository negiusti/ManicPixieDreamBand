using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// This class is the UI template for a single subtitle in the back log.
/// </summary>
public class LogEntry : MonoBehaviour
{
    public Text speakerName;
    public Text dialogueText;

    public void Assign(Subtitle subtitle)
    {
        speakerName.text = subtitle.speakerInfo.Name;
        dialogueText.text = subtitle.formattedText.text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SpeechBubble : MonoBehaviour
{
    private LogEntry logEntry;

    // Start is called before the first frame update
    void Start()
    {
        logEntry = this.GetComponentInChildren<LogEntry>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeechBubble(Subtitle subtitle)
    {
        logEntry.Assign(subtitle);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class is the UI template for a single subtitle in the back log.
/// </summary>
public class LogEntryData
{
    public string text;
    public bool isFirstTxt;
    public bool isGroupChat;
    public bool isPlayer;
    public string speakerName;
    public int entryID;
    public string imgName;

    public LogEntryData (LogEntry entry)
    {
        Assign(entry);
    }

    public void Assign(LogEntry entry)
    {
        isFirstTxt = entry.isFirstTxt;
        isGroupChat = entry.isGroupChat;
        isPlayer = entry.isPlayer;
        speakerName = entry.speakerName;
        entryID = entry.entryID;
        text = entry.text;
        imgName = entry.imgName;
    }
}

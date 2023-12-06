using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CustomDialogueScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool isCoolDown;
    private int coolDown = 1;
    public BackLog backLogTemplate;
    public ConvoHeader convoHeaderTemplate;
    private Dictionary<string, BackLog> backLogs;
    private Dictionary<string, ConvoHeader> convoHeaders;

    private void Awake()
    {
        backLogTemplate.gameObject.SetActive(false);
        convoHeaderTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        isCoolDown = false;
        backLogs = new Dictionary<string, BackLog>();
        convoHeaders = new Dictionary<string, ConvoHeader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode) && !isCoolDown) {
            DialogueManager.standardDialogueUI.OnContinue();
            StartCoroutine(CoolDown());
        }
    }

    public void AddBackLog(string contactName)
    {
        BackLog instance = Instantiate(backLogTemplate, backLogTemplate.transform.parent.transform);
        instance.gameObject.SetActive(true);
        backLogs.Add(contactName, instance);
        ConvoHeader instance2 = Instantiate(convoHeaderTemplate, convoHeaderTemplate.transform.parent.transform);
        instance2.gameObject.SetActive(true);
        instance2.SetConvoHeader(contactName);
        convoHeaders.Add(contactName, instance2);
    }

    IEnumerator CoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        isCoolDown = false;
    }

    void OnConversationLine(Subtitle subtitle)
    {
        Debug.Log("OnConversationLine: " + subtitle.dialogueEntry.DialogueText);
        string convoName = DialogueManager.LastConversationStarted;
        if (convoName.Contains("TXT_")) {
            string contactName = convoName.Substring(4);
            backLogs[contactName].AddToBacklog(subtitle);
            return;
        }
        if (subtitle.dialogueEntry.DialogueText.Length == 0 && subtitle.dialogueEntry.Title != "START")
        {
            Debug.Log("Continuing after empty line of dialogue!!");
            DialogueManager.standardDialogueUI.OnContinue();
        }
    }

    public void StopCurrentConvo()
    {
        DialogueManager.StopAllConversations();
    }

    public void ResumeTxtConvo(string contactName)
    {
        DialogueManager.StartConversation("TXT_" + contactName, null, null, backLogs[contactName].GetCurrEntryID());
    }

    public void FocusBackLog(string contactName)
    {
        foreach (string c in backLogs.Keys)
        {
            backLogs[c].gameObject.SetActive(c.Equals(contactName));
            convoHeaders[c].gameObject.SetActive(c.Equals(contactName));
        }
    }

    public void CloseBacklogs()
    {
        foreach (string c in backLogs.Keys)
        {
            backLogs[c].gameObject.SetActive(false);
            convoHeaders[c].gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(DebugLog), this, SymbolExtensions.GetMethodInfo(() => DebugLog(string.Empty)));
        Lua.RegisterFunction(nameof(StopCurrentConvo), this, SymbolExtensions.GetMethodInfo(() => StopCurrentConvo()));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(DebugLog));
        Lua.UnregisterFunction(nameof(StopCurrentConvo));
        //}
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }
}

using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;

public class CustomDialogueScript : MonoBehaviour
{
    public Action<string> ConvoCompleted;
    public KeyCode keyCode;
    private bool isCoolDown;
    private float coolDown = 1f;
    public BackLog backLogTemplate;
    public ConvoHeader convoHeaderTemplate;
    private Dictionary<string, BackLog> backLogs;
    private Dictionary<string, ConvoHeader> convoHeaders;
    public StandardUIMenuPanel phoneResponsePanel;
    private StandardUIMenuPanel responsePanel;
    private Dictionary<string, Conversation> conversations;
    private ConversationData[] plotData;
    private Canvas phoneResponsePanelCanvas;
    public int currentConvoIdx;
    private string currentLocation;
    private Phone phone;

    private void Awake()
    {
        backLogTemplate.gameObject.SetActive(false);
        convoHeaderTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        phone = FindFirstObjectByType<Phone>();
        currentConvoIdx = 0;
        SubscribeToEvents();
        isCoolDown = false;
        backLogs = new Dictionary<string, BackLog>();
        convoHeaders = new Dictionary<string, ConvoHeader>();
        currentLocation = SceneManager.GetActiveScene().name;
        GetAllConversations();
        phoneResponsePanelCanvas = phoneResponsePanel.gameObject.GetComponent<Canvas>();
        CheckForPlotConvo();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        SceneManager.sceneLoaded -= NewActiveScene;
        SceneManager.sceneUnloaded -= EndingActiveScene;
    }

    private void SubscribeToEvents()
    {
        SceneManager.sceneLoaded += NewActiveScene;
        SceneManager.sceneUnloaded += EndingActiveScene;
    }

    private void NewActiveScene(Scene current, LoadSceneMode mode)
    {
        currentLocation = SceneManager.GetActiveScene().name;
    }

    private void EndingActiveScene(Scene current)
    {
        StopCurrentConvo();
    }

    private bool CheckForPlotConvo()
    {
        if (currentConvoIdx > plotData.Length -1)
        {
            return false;
        }
        if (!phone.IsLocked())
            return false;
        if (DialogueManager.IsConversationActive)
            return false;
        if (plotData[currentConvoIdx].locations.Contains(currentLocation))
        {
            //if (!conversations.ContainsKey(plotData.conversationsData[currentConvoIdx].conversation))
            //{
            //    Debug.Log("Could not find conversation in database: " + plotData.conversationsData[currentConvoIdx].conversation);
            //    return;
            //}
            StartConversation(plotData[currentConvoIdx].conversation);
            return true;
        }
        return false;
    }

    private void StartConversation(string conversation)
    {
        // TXT_ContactName_ConversationName
        if (IsTxtConvo(conversation))
        {
            // Send notification to phone
            phone.ReceiveMsg(conversation, true);
        }
        else
        {
            if (currentConvoIdx < plotData.Count() && plotData[currentConvoIdx].conversation.Equals(conversation))
                SpawnCharacters.SpawnParticipants(plotData[currentConvoIdx].participants);
            DialogueManager.StartConversation(conversation);
        }
    }

    public void StartConversation(ConversationData convoData)
    {
        // TXT_ContactName_ConversationName
        if (IsTxtConvo(convoData.conversation))
        {
            // Send notification to phone
            phone.ReceiveMsg(convoData.conversation, true);
        }
        else
        {
            //if (currentConvoIdx < plotData.Count() && plotData[currentConvoIdx].conversation.Equals(convoData.conversation))
            SpawnCharacters.SpawnParticipants(convoData.participants);
            DialogueManager.StartConversation(convoData.conversation);
        }
    }

    private void GetAllConversations()
    {
        // TODO sort conversations into trivial, plot, text, etc
        //conversations = DialogueManager.masterDatabase.conversations.ToDictionary(c => c.Name, c => c);
        plotData = ConversationJson.GetPlotData().conversationsData;
    }

    public bool IsPCResponseMenuOpen()
    {
        responsePanel = GameObject.FindFirstObjectByType<MainCharacter>().gameObject.GetComponentInChildren<PixelCrushers.DialogueSystem.Wrappers.StandardUIMenuPanel>();
        return responsePanel != null && responsePanel.gameObject.activeSelf;// && responsePanel.isOpen;
    }

    // Update is called once per frame
    void Update()
    {
        // the reason I do this is so that the space button selects the dialogue option without also continuing
        if (Input.GetKeyDown(keyCode) && DialogueManager.IsConversationActive && !isCoolDown &&
            !IsPCResponseMenuOpen() && !IsTxtConvoActive() && !DialogueTime.IsPaused)
        {
            StartCoroutine(CoolDown());
            Debug.Log("continuing");
            DialogueManager.standardDialogueUI.OnContinueConversation();
            //DialogueManager.standardDialogueUI.OnContinue();
        } else if (Input.GetKeyDown(keyCode) && DialogueManager.IsConversationActive && isCoolDown)
        {
            Debug.Log("cooling down");
            //isCoolDown = false;
        }
        if (!DialogueManager.IsConversationActive)
        {
            Debug.Log("no active conversations");
            if (!CheckForPlotConvo())
            {
                Debug.Log("no plot convos found, checking for quest convo...");
                // Check for QuestConvo
                QuestManager.CheckForQuestConvo();
            }
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

    void OnConversationResponseMenu(Response[] responses)
    {
        
    }
    private void PrepTxtConvo()
    {
        DialogueManager.displaySettings.subtitleSettings.skipPCSubtitleAfterResponseMenu = true;
        DialogueManager.displaySettings.subtitleSettings.showPCSubtitlesDuringLine = false;
        DialogueManager.displaySettings.subtitleSettings.showNPCSubtitlesDuringLine = false;
        DialogueManager.SetDialoguePanel(true, true);
        phoneResponsePanel.gameObject.SetActive(true);
        phoneResponsePanelCanvas.enabled = true;
        DialogueManager.standardDialogueUI.ForceOverrideMenuPanel(phoneResponsePanel);
        DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;
    }

    void OnConversationStart(Transform actor)
    {
        string convoName = DialogueManager.LastConversationStarted;
        if (IsTxtConvo(convoName))
        {
            PrepTxtConvo();
        }
        else
        {
            PrepSpokenConvo();
        }
    }

    private void PrepSpokenConvo()
    {
        DialogueManager.displaySettings.subtitleSettings.skipPCSubtitleAfterResponseMenu = false;
        DialogueManager.displaySettings.subtitleSettings.showPCSubtitlesDuringLine = true;
        DialogueManager.displaySettings.subtitleSettings.showNPCSubtitlesDuringLine = true;
        DialogueManager.SetDialoguePanel(true, true);
        responsePanel = GameObject.FindObjectOfType<MainCharacter>().gameObject.GetComponentInChildren<StandardUIMenuPanel>();
        DialogueManager.standardDialogueUI.ForceOverrideMenuPanel(responsePanel);
        DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;
    }

    //void OnConversationEnd(Transform actor)
    //{
    //}

    public bool IsCurrentConvoTxt()
    {
        string convoName = DialogueManager.LastConversationStarted;
        return IsTxtConvo(convoName);
    }

    public bool IsTxtConvoActive()
    {
        return DialogueManager.IsConversationActive && IsCurrentConvoTxt();
    }

    public bool IsTxtConvo(string convoName)
    {
        return convoName.StartsWith("TXT/");
    }

    void OnConversationLine(Subtitle subtitle)
    {
        Debug.Log("OnConversationLine: " + subtitle.dialogueEntry.DialogueText);
        string convoName = DialogueManager.LastConversationStarted;

        // if you reached the end of the conversation
        if (subtitle.dialogueEntry.outgoingLinks.Count == 0)
        {
            ConversationComplete(convoName);
            if (subtitle.dialogueEntry.DialogueText.Length == 0)// && !subtitle.dialogueEntry.Title.Equals("START"))
            {
                Debug.Log("Continuing after empty line of dialogue!!");
                DialogueManager.standardDialogueUI.OnContinue();
            }
        }

        if (subtitle.dialogueEntry.subtitleText.StartsWith('[') && subtitle.dialogueEntry.subtitleText.EndsWith(']'))
        {
            Debug.Log("meta dialogue text: " + subtitle.dialogueEntry.DialogueText);
            DialogueManager.standardDialogueUI.HideSubtitle(subtitle);
            DialogueManager.standardDialogueUI.OnContinue();
            return;
        }

        if (subtitle.dialogueEntry.DialogueText.Length > 0 && IsTxtConvo(convoName)) {
            string contactName = phone.GetContactNameFromConvoName(convoName);
            FocusBackLog(contactName);
            backLogs[contactName].AddToBacklog(subtitle);
            return;
        }
    }

    private void ConversationComplete(string convoName)
    {
        Debug.Log("convo complete: " + convoName);
        if (currentConvoIdx < plotData.Count() && plotData[currentConvoIdx].conversation.Equals(convoName))
            currentConvoIdx++;
        if (IsTxtConvo(convoName))
        {
            phone.CompleteConvo(convoName);
            backLogs[phone.GetContactNameFromConvoName(convoName)].ResetCurrentEntryID();
        }
        ConvoCompleted?.Invoke(convoName);
    }

    public void StopCurrentConvo()
    {
        Debug.Log("STJOP CURRENTJ JCONVJOj");
        DialogueManager.StopAllConversations();
    }

    public void ResumeTxtConvo(string contactName, string conversation)
    {
        // TODO check if conversation was completed or else resume it
        //conversations[0].dialogueEntries.LastOrDefault().id;
        DialogueManager.displaySettings.subtitleSettings.skipPCSubtitleAfterResponseMenu = true;
        DialogueManager.displaySettings.subtitleSettings.showPCSubtitlesDuringLine = false;
        DialogueManager.displaySettings.subtitleSettings.showNPCSubtitlesDuringLine = false;
        FocusBackLog(contactName);
        DialogueManager.StartConversation(conversation, null, null, backLogs[contactName].GetCurrEntryID());
    }

    public void FocusBackLog(string contactName)
    {
        if (!backLogs.ContainsKey(contactName))
        {
            AddBackLog(contactName);
        }
        if (backLogs[contactName].gameObject.activeSelf)
        {
            return;
        }
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

    public void StartShopkeeperConvo(string convoName)
    {
        if (DialogueManager.IsConversationActive)
            return;
        StartConversation(convoName);
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(DebugLog), this, SymbolExtensions.GetMethodInfo(() => DebugLog(string.Empty)));
        //Lua.RegisterFunction(nameof(StopCurrentConvo), this, SymbolExtensions.GetMethodInfo(() => StopCurrentConvo()));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(DebugLog));
        //Lua.UnregisterFunction(nameof(StopCurrentConvo));
        //}
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }
}

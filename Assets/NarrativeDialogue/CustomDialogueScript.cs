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
    private float coolDown = 0.75f;
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
    private readonly float orthoCamSizeOutside = 12f;
    private readonly float orthoCamSizeOutsideConvo = 8f;
    private bool waitForK;

    // Start is called before the first frame update
    void Start()
    {
        backLogTemplate.gameObject.SetActive(false);
        convoHeaderTemplate.gameObject.SetActive(false);
        currentConvoIdx = ES3.Load("PlotConvoIdx", 0);
        SubscribeToEvents();
        isCoolDown = false;
        backLogs = new Dictionary<string, BackLog>();
        convoHeaders = new Dictionary<string, ConvoHeader>();
        currentLocation = SceneManager.GetActiveScene().name;
        GetAllConversations();
        phoneResponsePanelCanvas = phoneResponsePanel.gameObject.GetComponent<Canvas>();
        StartCoroutine(CheckForConvos());
        CheckForConvo();
    }

    public void Reset()
    {
        currentConvoIdx = 0;
        convoHeaders = new Dictionary<string, ConvoHeader>();
        backLogs = new Dictionary<string, BackLog>();
        for (int i = 0; i < backLogTemplate.transform.parent.childCount; i++)
        {
            if (backLogTemplate.transform.parent.GetChild(i).gameObject.name.Contains("Clone"))
                Destroy(backLogTemplate.transform.parent.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        ES3.Save("PlotConvoIdx", currentConvoIdx);
        UnsubscribeFromEvents();
        StopAllCoroutines();
    }

    private void UnsubscribeFromEvents()
    {
        SceneManager.sceneLoaded -= NewActiveScene;
        SceneManager.sceneUnloaded -= EndingActiveScene;
        ConvoCompleted -= Calendar.OnConversationComplete;
        ConvoCompleted -= RomanceManager.OnConversationComplete;
    }

    private void SubscribeToEvents()
    {
        SceneManager.sceneLoaded += NewActiveScene;
        SceneManager.sceneUnloaded += EndingActiveScene;
        ConvoCompleted += Calendar.OnConversationComplete;
        ConvoCompleted += RomanceManager.OnConversationComplete;
    }

    private void NewActiveScene(Scene current, LoadSceneMode mode)
    {
        currentLocation = SceneManager.GetActiveScene().name;
        CheckForConvo();
    }

    private void EndingActiveScene(Scene current)
    {
        StopCurrentConvo();
        ES3.Save("PlotConvoIdx", currentConvoIdx);
    }

    private bool CheckForPlotConvo()
    {
        if (currentConvoIdx > plotData.Length - 1)
        {
            return false;
        }

        //if (!plotData[currentConvoIdx].locations.Contains(currentLocation))
        //{
        //    Debug.Log("plotData[currentConvoIdx]" + plotData[currentConvoIdx].conversation + " " + plotData[currentConvoIdx].locations + " " + currentLocation);
        //}

        if (plotData[currentConvoIdx].locations.Contains(currentLocation) && ConvoRequirements.RequirementsMet(plotData[currentConvoIdx].requirements))
        {
            //if (!conversations.ContainsKey(plotData.conversationsData[currentConvoIdx].conversation))
            //{
            //    Debug.Log("Could not find conversation in database: " + plotData.conversationsData[currentConvoIdx].conversation);
            //    return;
            //}

            // THIS IS INSANITY:
            // I WANT spoken convos to start during loading screen to ensure characters are spawned before loading screen completes.
            // I DON'T WANT the loading screen to hide the unlock animation for the phone, so it shouldn't start a text convo until loading screen completes
            // UNLESS the text convo is happening in the background without unlocking the phone. Then I want it to start immediately without delaying further convos.
            if (SceneChanger.Instance.IsLoadingScreenOpen() && (IsTxtConvo(plotData[currentConvoIdx].conversation) && !plotData[currentConvoIdx].conversation.Contains("Opt")))
                return false;
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
            // Send notification to Phone.Instance
            Phone.Instance.ReceiveMsg(conversation, !conversation.Contains("Opt"));
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
            // Send notification to Phone.Instance
            Phone.Instance.ReceiveMsg(convoData.conversation, !convoData.conversation.Contains("Opt"));
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
        if (waitForK)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                waitForK = false;
                DialogueManager.standardDialogueUI.OnContinueConversation();
            }
            return;
        }
        // the reason I do this is so that the space button selects the dialogue option without also continuing
        if ((Input.GetKeyDown(keyCode) || Input.GetKeyDown(KeyCode.Return)) && DialogueManager.IsConversationActive && !isCoolDown &&
            !IsPCResponseMenuOpen() && !IsTxtConvoActive() && !DialogueTime.IsPaused && SceneChanger.Instance != null && !SceneChanger.Instance.IsLoadingScreenOpen())
        {
            StartCoroutine(CoolDown());
            Debug.Log("continuing");
            UnityUITypewriterEffect[] fucks = FindObjectsOfType<UnityUITypewriterEffect>().Where(t => t.IsPlaying).ToArray();
            foreach (UnityUITypewriterEffect t in fucks)
            {
                t.Stop();
            }
            if (fucks.Length == 0 && DialogueManager.currentConversationState.subtitle.dialogueEntry.currentDialogueText.Length > 0)
                DialogueManager.standardDialogueUI.OnContinueConversation();
            //DialogueManager.standardDialogueUI.OnContinue();
        } else if ((Input.GetKeyDown(keyCode) || Input.GetKeyDown(KeyCode.Return)) && DialogueManager.IsConversationActive && isCoolDown)
        {
            //FindObjectsOfType<UnityUITypewriterEffect>().Select(t => t.enabled = false);
            //Debug.Log("cooling down");
            UnityUITypewriterEffect[] fucks = FindObjectsOfType<UnityUITypewriterEffect>().Where(t => t.IsPlaying).ToArray();
            foreach (UnityUITypewriterEffect t in fucks)
            {
                t.Stop();
            }
        }
    }

    private void CheckForConvo()
    {
        if (!DialogueManager.IsConversationActive &&
                !MiniGameManager.AnyActiveMiniGames() &&
                Phone.Instance != null &&
                Phone.Instance.IsLocked())
        {
            if (!CheckForPlotConvo())
            {
                RomanceManager.CheckForRomanceConvo();
                QuestManager.CheckForQuestConvo();
            }
        }
    }

    private IEnumerator CheckForConvos()
    {
        while(true)
        {
            CheckForConvo();
            yield return new WaitForSeconds(1f);
        }
    }

    public void AddBackLog(string contactName)
    {
        BackLog instance = Instantiate(backLogTemplate, backLogTemplate.transform.parent);
        instance.gameObject.SetActive(true);
        instance.AssignContact(contactName);
        backLogs.Add(contactName, instance);
        ConvoHeader instance2 = Instantiate(convoHeaderTemplate, convoHeaderTemplate.transform.parent);
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
        DialogueManager.displaySettings.subtitleSettings.skipPCSubtitleAfterResponseMenu = true;
        DialogueManager.displaySettings.subtitleSettings.showPCSubtitlesDuringLine = true;
        DialogueManager.displaySettings.subtitleSettings.showNPCSubtitlesDuringLine = true;
        DialogueManager.SetDialoguePanel(true, true);
        responsePanel = GameObject.FindObjectOfType<MainCharacter>().gameObject.GetComponentInChildren<StandardUIMenuPanel>();
        DialogueManager.standardDialogueUI.ForceOverrideMenuPanel(responsePanel);
        DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;
        bool isOutside = FindObjectsOfType<OutdoorLocation>().Length > 0;
        if (isOutside)
        {
            if (Camera.main.gameObject.GetComponent<CameraLerp>() == null)
                Camera.main.gameObject.AddComponent<CameraLerp>();
            StartCoroutine(Camera.main.gameObject.GetComponent<CameraLerp>().PanCameraTo(orthoCamSizeOutsideConvo, 0.5f));
        }
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

        if (subtitle.dialogueEntry.DialogueText.Length > 0 && IsTxtConvo(convoName)) {
            string contactName = Phone.Instance.GetContactNameFromConvoName(convoName);
            FocusBackLog(contactName);
            backLogs[contactName].AddToBacklog(subtitle);
            return;
        }
    }

    private void ConversationComplete(string convoName)
    {
        bool isOutside = FindObjectsOfType<OutdoorLocation>().Length > 0;
        if (isOutside)
        { 
            if (Camera.main.gameObject.GetComponent<CameraLerp>() == null)
                Camera.main.gameObject.AddComponent<CameraLerp>();
            StartCoroutine(Camera.main.gameObject.GetComponent<CameraLerp>().PanCameraTo(orthoCamSizeOutside, 0.5f));
        }
        Debug.Log("convo complete: " + convoName);
        if (currentConvoIdx < plotData.Count() && plotData[currentConvoIdx].conversation.Equals(convoName))
            currentConvoIdx++;
        if (IsTxtConvo(convoName))
        {
            Phone.Instance.CompleteConvo(convoName);
            backLogs[Phone.Instance.GetContactNameFromConvoName(convoName)].ResetCurrentEntryID();
        }
        ConvoCompleted?.Invoke(convoName);
        CheckForConvo();
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

    public void WaitForK()
    {
        waitForK = true;
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(DebugLog), this, SymbolExtensions.GetMethodInfo(() => DebugLog(string.Empty)));
        Lua.RegisterFunction(nameof(WaitForK), this, SymbolExtensions.GetMethodInfo(() => WaitForK()));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(DebugLog));
        Lua.UnregisterFunction(nameof(WaitForK));
        //}
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }
}

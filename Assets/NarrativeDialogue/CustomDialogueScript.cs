using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class CustomDialogueScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool isCoolDown;
    private float coolDown = 0.5f;
    public BackLog backLogTemplate;
    public ConvoHeader convoHeaderTemplate;
    private Dictionary<string, BackLog> backLogs;
    private Dictionary<string, ConvoHeader> convoHeaders;
    public StandardUIMenuPanel phoneResponsePanel;
    private StandardUIMenuPanel responsePanel;
    private List<Conversation> conversations;
    private PlotData plotData;
    private Canvas phoneResponsePanelCanvas;
    private int currentConvoIdx;

    private void Awake()
    {
        backLogTemplate.gameObject.SetActive(false);
        convoHeaderTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentConvoIdx = 0;
        //SceneManager.activeSceneChanged += ChangedActiveScene;
        SceneManager.sceneLoaded += ChangedActiveScene;
        isCoolDown = false;
        backLogs = new Dictionary<string, BackLog>();
        convoHeaders = new Dictionary<string, ConvoHeader>();
        GetAllConversations();
        phoneResponsePanelCanvas = phoneResponsePanel.gameObject.GetComponent<Canvas>();
        CheckForConvo();
    }

    private void ChangedActiveScene(Scene current, LoadSceneMode mode)
    {
        CheckForConvo();
    }

    private void CheckForConvo()
    {
        string currentLocation = SceneManager.GetActiveScene().name;
        if (plotData.conversationsData[currentConvoIdx].locations.Contains(currentLocation))
        {
            DialogueManager.StartConversation(plotData.conversationsData[currentConvoIdx].conversation);
            currentConvoIdx++;
        }
    }

    private void GetAllConversations()
    {
        // TODO sort conversations into trivial, plot, text, etc
        conversations = DialogueManager.masterDatabase.conversations;
        ConversationJson.LoadFromJson().WaitForCompletion();
        plotData = ConversationJson.GetPlotData();
    }

    public bool IsPCResponseMenuOpen()
    {
        responsePanel = GameObject.FindFirstObjectByType<MainCharacter>().gameObject.GetComponentInChildren<PixelCrushers.DialogueSystem.Wrappers.StandardUIMenuPanel>();
        return responsePanel != null && responsePanel.gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        // the reason I do this is so that the space button selects the dialogue option without also continuing
        if (Input.GetKeyDown(keyCode) && DialogueManager.IsConversationActive && !isCoolDown &&
            !IsPCResponseMenuOpen())
        {
            DialogueManager.standardDialogueUI.OnContinue();
            StartCoroutine(CoolDown());
            Debug.Log("continuing");
        } else if (Input.GetKeyDown(keyCode) && DialogueManager.IsConversationActive && isCoolDown)
        {
            Debug.Log("cooling down");
            isCoolDown = false;
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

    void OnConversationStart(Transform actor)
    {
        string convoName = DialogueManager.LastConversationStarted;
        if (IsTxtConvo(convoName))
        {
            DialogueManager.displaySettings.subtitleSettings.skipPCSubtitleAfterResponseMenu = true;
            DialogueManager.displaySettings.subtitleSettings.showPCSubtitlesDuringLine = false;
            DialogueManager.displaySettings.subtitleSettings.showNPCSubtitlesDuringLine = false;
            DialogueManager.SetDialoguePanel(false, true);
            //phoneResponsePanel.gameObject.SetActive(true);            
            //phoneResponsePanelCanvas.enabled = true;
            DialogueManager.standardDialogueUI.ForceOverrideMenuPanel(phoneResponsePanel);
            DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Never;
        }
        else
        {
            DialogueManager.displaySettings.subtitleSettings.skipPCSubtitleAfterResponseMenu = false;
            DialogueManager.displaySettings.subtitleSettings.showPCSubtitlesDuringLine = true;
            DialogueManager.displaySettings.subtitleSettings.showNPCSubtitlesDuringLine = true;
            DialogueManager.SetDialoguePanel(true, true);
            responsePanel = GameObject.FindObjectOfType<MainCharacter>().gameObject.GetComponentInChildren<StandardUIMenuPanel>();
            DialogueManager.standardDialogueUI.ForceOverrideMenuPanel(responsePanel);
            DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;
        }
    }

    void OnConversationEnd(Transform actor)
    {
        //phoneResponsePanelCanvas.enabled = false;
        //phoneResponsePanel.gameObject.SetActive(false);
    }

    public bool IsCurrentConvoTxt()
    {
        string convoName = DialogueManager.LastConversationStarted;
        return IsTxtConvo(convoName);
    }

    public bool IsTxtConvoActive()
    {
        return DialogueManager.IsConversationActive && IsCurrentConvoTxt();
    }

    private bool IsTxtConvo(string convoName)
    {
        return convoName.Contains("TXT");
    }

    void OnConversationLine(Subtitle subtitle)
    {
        Debug.Log("OnConversationLine: " + subtitle.dialogueEntry.DialogueText);
        string convoName = DialogueManager.LastConversationStarted;
        if (IsTxtConvo(convoName)) {
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
        // TODO check if conversation was completed or else resume it
        //conversations[0].dialogueEntries.LastOrDefault().id;
        // check if convo name is group or singular
        DialogueManager.StartConversation("TXT_" + contactName, null, null, backLogs[contactName].GetCurrEntryID());
        DialogueManager.SetDialoguePanel(false, true);
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

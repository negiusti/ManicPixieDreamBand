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
    private Dictionary<string, BackLog> backLogs;

    private void Awake()
    {
        backLogTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        isCoolDown = false;
        backLogs = new Dictionary<string, BackLog>();
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
    }

    IEnumerator CoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        isCoolDown = false;
    }

    public void OnConversationLine(Subtitle subtitle)
    {
        if (subtitle.dialogueEntry.DialogueText.Length == 0 && subtitle.dialogueEntry.Title != "START") {
            Debug.Log("Continuing after empty line of dialogue!!");
            DialogueManager.standardDialogueUI.OnContinue();
        }
        string convoName = DialogueManager.LastConversationStarted;
        if (convoName.Contains("TXT_")) {
            string contactName = convoName.Substring(4);
            backLogs[contactName].AddToBacklog(subtitle);
        }
    }

    public void FocusBackLog(string contactName)
    {
        foreach (string c in backLogs.Keys)
        {
            backLogs[c].gameObject.SetActive(c.Equals(contactName));
        }
    }

    public void CloseBacklogs()
    {
        foreach (string c in backLogs.Keys)
        {
            backLogs[c].gameObject.SetActive(false);
        }
    }
}

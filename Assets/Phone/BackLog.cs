using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System;

/// <summary>
/// This script runs the back log. It records dialogue lines as they're played.
/// The ShowBackLog() method fills a back log window with the recorded lines.
/// </summary>
public class BackLog : MonoBehaviour
{
    private string contactName; // used for saving the backlog
    public Transform logEntryContainer;
    public LogEntry logEntryTemplate;
    public LogEntry photoEntryTemplate;
    public Text speakerNameTemplate;
    public ScrollRect scrollView;
    public GameObject typingBubbleTemplate;
    public StandardUIMenuPanel responseMenu;
    private int currentEntryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT/Band" };

    //private List<Subtitle> log = new List<Subtitle>();
    private List<LogEntry> instances = new List<LogEntry>();
    private List<LogEntryData> savedData = new List<LogEntryData>();
    private RectTransform rectTransform;
    private Scrollbar scrollbar;
    private static float longScrollViewHeight = 310.6017f;
    private static float shortScrollViewHeight = 203.4449f;
    private GameObject typingBubble;

    private void Start()
    {
        logEntryTemplate.gameObject.SetActive(false);
        photoEntryTemplate.gameObject.SetActive(false);
        speakerNameTemplate.gameObject.SetActive(false);
        typingBubbleTemplate.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
        scrollbar = GetComponentInChildren<Scrollbar>(true);
        ResetCurrentEntryID();
    }

    public void ResetCurrentEntryID()
    {
        currentEntryID = 0;
    }

    public void AssignContact(string name)
    {
        contactName = name;
        // Load previous messages
        savedData = ES3.Load("Msgs/"+contactName, new List<LogEntryData>());
        foreach (LogEntryData l in savedData)
        {
            LoadBacklogEntry(l);
        }
    }

    public void SaveContact()
    {
        Debug.Log("Saving contact: " + contactName);
        ES3.Save("Msgs/" + contactName, savedData);
    }

    private void OnDisable()
    {
        DialogueManager.Unpause();
        if (typingBubble != null)
        {
            typingBubble.SetActive(false);
            Destroy(typingBubble);
            typingBubble = null;
        }
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        bool rView = Math.Abs(sizeDelta.y - shortScrollViewHeight) < 0.1;
        if (!rView && responseMenu.gameObject.activeSelf && responseMenu.isOpen)
        {
            Debug.Log("shorten view");
            sizeDelta.y = shortScrollViewHeight;
            rectTransform.sizeDelta = sizeDelta;

            RectTransform r = scrollbar.gameObject.GetComponent<RectTransform>();
            sizeDelta = r.sizeDelta;
            sizeDelta.y = shortScrollViewHeight;
            r.sizeDelta = sizeDelta;
        } else if (rView && (!responseMenu.gameObject.activeSelf || !responseMenu.isOpen))
        {
            Debug.Log("extend view");
            sizeDelta.y = longScrollViewHeight;
            rectTransform.sizeDelta = sizeDelta;

            RectTransform r = scrollbar.gameObject.GetComponent<RectTransform>();
            sizeDelta = r.sizeDelta;
            sizeDelta.y = longScrollViewHeight;
            r.sizeDelta = sizeDelta;
        }
    }

    public int GetCurrEntryID()
    {
        return currentEntryID;
    }

    public void AddToBacklog(Subtitle subtitle)
    {
        if (subtitle.dialogueEntry.id == currentEntryID)
        {
            Debug.Log("oncontinuing");
            DialogueManager.standardDialogueUI.OnContinueConversation();
        } else if (subtitle.activeConversationRecord.conversationTitle.Contains("Opt"))
        {
            AddToBacklogWitouthDelay(subtitle);
        }
        else
        {
            DialogueManager.Pause();
            StartCoroutine(AddToBacklogWithDelay(subtitle));
        }
    }

    private void LoadBacklogEntry(LogEntryData entry)
    {
        Debug.Log("Load entry: " + entry.text);
        if (!string.IsNullOrEmpty(entry.text))
        {
            if (entry.isGroupChat && !entry.isPlayer)
            {
                // add a name header
                Text speakerName = Instantiate(speakerNameTemplate, logEntryContainer);
                speakerName.text = entry.speakerName;
                speakerName.gameObject.SetActive(true);
            }

            LogEntry instance = Instantiate(logEntryTemplate, logEntryContainer);
            instances.Add(instance);
            instance.gameObject.SetActive(true);
            instance.Assign(entry);

            if (!entry.isPlayer)
            {
                instance.transform.rotation = new Quaternion(instance.transform.rotation.x, 180f, instance.transform.rotation.z, instance.transform.rotation.w);
            }
            else
            {
                instance.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperRight;
            }

            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            Text[] texts = instance.GetComponentsInChildren<Text>();
            float preferredHeight = 30f;
            foreach (Text t in texts)
            {
                preferredHeight += t.preferredHeight;
            }
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

            Image image = instance.GetComponent<Image>();
            if (entry.isPlayer)
            {
                image.color = new Color(0.6f, 0.94f, 1f);
            }
            else
            {
                image.color = new Color(0.98f, 0.89f, 1f);
            }
        }
        DialogueManager.standardDialogueUI.OnContinue();
        ScrollToBottomOfScrollView();
    }

    private void AddToBacklogWitouthDelay(Subtitle subtitle)
    {
        bool isFirstTxt = subtitle.dialogueEntry.Title.Equals("FIRST");
        bool isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));

        if (!string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            if (isGroupChat && !subtitle.speakerInfo.IsPlayer)
            {
                // add a name header
                Text speakerName = Instantiate(speakerNameTemplate, logEntryContainer);
                speakerName.text = subtitle.speakerInfo.Name;
                speakerName.gameObject.SetActive(true);
            }

            //log.Add(subtitle);
            LogEntry instance;
            if (subtitle.formattedText.text.StartsWith("_"))
            {
                instance = Instantiate(photoEntryTemplate, logEntryContainer);
            }
            else
            {
                instance = Instantiate(logEntryTemplate, logEntryContainer);
            }
            instances.Add(instance);
            instance.gameObject.SetActive(true);
            instance.Assign(subtitle);
            savedData.Add(new LogEntryData(instance));

            if (!subtitle.speakerInfo.IsPlayer)
            {
                instance.transform.rotation = new Quaternion(instance.transform.rotation.x, 180f, instance.transform.rotation.z, instance.transform.rotation.w);
            }
            else
            {
                instance.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperRight;
            }

            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            Text[] texts = instance.GetComponentsInChildren<Text>();
            float preferredHeight = 30f;
            foreach (Text t in texts)
            {
                preferredHeight += t.preferredHeight;
            }
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

            Image image = instance.GetComponent<Image>();
            if (subtitle.speakerInfo.IsPlayer)
            {
                image.color = new Color(0.6f, 0.94f, 1f);
            }
            else
            {
                image.color = new Color(0.98f, 0.89f, 1f);
            }
        }
        currentEntryID = subtitle.dialogueEntry.id;
        DialogueManager.standardDialogueUI.OnContinue();
        ScrollToBottomOfScrollView();
    }

    private IEnumerator AddToBacklogWithDelay(Subtitle subtitle)
    {
        bool isFirstTxt = subtitle.dialogueEntry.Title.Equals("FIRST");
        bool isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
        if (!isFirstTxt && !subtitle.speakerInfo.IsPlayer && currentEntryID > 0 && !string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            // add typing bubble here
            yield return new WaitForSeconds(1f);
            typingBubble = Instantiate(typingBubbleTemplate, logEntryContainer);
            typingBubble.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            typingBubble.SetActive(false);
            Destroy(typingBubble);
            typingBubble = null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
        }
        
        if (!string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            if (!subtitle.speakerInfo.IsPlayer)
                Phone.Instance.PlayTxtRcvSound();
            else
                Phone.Instance.PlayTxtSntSound();

            if (isGroupChat && !subtitle.speakerInfo.IsPlayer)
            {
                // add a name header
                Text speakerName = Instantiate(speakerNameTemplate, logEntryContainer);
                speakerName.text = subtitle.speakerInfo.Name;
                speakerName.gameObject.SetActive(true);
            }

            //log.Add(subtitle);
            LogEntry instance;
            if (subtitle.formattedText.text.StartsWith("_"))
            {
                instance = Instantiate(photoEntryTemplate, logEntryContainer);
            } else
            {
                instance = Instantiate(logEntryTemplate, logEntryContainer);
            }
            instances.Add(instance);
            instance.gameObject.SetActive(true);
            instance.Assign(subtitle);
            savedData.Add(new LogEntryData(instance));

            if (!subtitle.speakerInfo.IsPlayer)
            {
                instance.transform.rotation = new Quaternion(instance.transform.rotation.x, 180f, instance.transform.rotation.z, instance.transform.rotation.w);
            } else
            {
                instance.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperRight;
            }

            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            Text[] texts = instance.GetComponentsInChildren<Text>();
            float preferredHeight = 30f;
            foreach (Text t in texts)
            {
                preferredHeight += t.preferredHeight;
            }
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

            Image image = instance.GetComponent<Image>();
            if (subtitle.speakerInfo.IsPlayer)
            {
                image.color = new Color(0.6f, 0.94f, 1f);
            }
            else
            {
                image.color = new Color(0.98f, 0.89f, 1f);
            }
        }
        currentEntryID = subtitle.dialogueEntry.id;
        DialogueManager.Unpause();
        Debug.Log("continuing");
        DialogueManager.standardDialogueUI.OnContinue();
        ScrollToBottomOfScrollView();
        yield return null;
    }

    void ScrollToBottomOfScrollView()
    {
        if (scrollView != null && scrollView.verticalScrollbar != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
            scrollView.verticalNormalizedPosition = Mathf.Clamp(scrollView.verticalNormalizedPosition, 0f, 1f);
            scrollView.verticalScrollbar.value = 0f;
            scrollView.verticalNormalizedPosition = 0f;
        }
        else
        {
            Debug.LogWarning("ScrollView or its verticalScrollbar is not assigned.");
        }
    }

    //public void ShowBackLog()
    //{
    //    instances.ForEach(instance => Destroy(instance));
    //    instances.Clear();
    //    foreach (Subtitle subtitle in log)
    //    {
    //        var instance = Instantiate(logEntryTemplate, logEntryContainer);
    //        instances.Add(instance.gameObject);
    //        instance.gameObject.SetActive(true);
    //        instance.Assign(subtitle);
    //    }
    //}
}

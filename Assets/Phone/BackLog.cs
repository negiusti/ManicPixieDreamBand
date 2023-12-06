using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using TMPro;

/// <summary>
/// This script runs the back log. It records dialogue lines as they're played.
/// The ShowBackLog() method fills a back log window with the recorded lines.
/// </summary>
public class BackLog : MonoBehaviour
{
    public Transform logEntryContainer;
    public LogEntry logEntryTemplate;
    public Text speakerNameTemplate;
    public ScrollRect scrollView;
    private int currentEntryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT_Band" };

    private List<Subtitle> log = new List<Subtitle>();

    private List<GameObject> instances = new List<GameObject>();

    private void Start()
    {
        currentEntryID = 0;
    }

    private void Awake()
    {
        logEntryTemplate.gameObject.SetActive(false);
        speakerNameTemplate.gameObject.SetActive(false);
    }

    public int GetCurrEntryID()
    {
        return currentEntryID;
    }
    
    public void AddToBacklog(Subtitle subtitle)
    {
        bool isGroupChat = groupChats.Contains(DialogueManager.LastConversationStarted);
        Debug.Log("AddToBacklog: " + subtitle.dialogueEntry.DialogueText);
        ScrollToBottomOfScrollView();
        if (!string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            if (isGroupChat && !subtitle.speakerInfo.IsPlayer)
            {
                // add a name header
                Text speakerName = Instantiate(speakerNameTemplate, logEntryContainer);
                speakerName.text = subtitle.speakerInfo.Name;
                speakerName.gameObject.SetActive(true);
            }

            log.Add(subtitle);
            LogEntry instance = Instantiate(logEntryTemplate, logEntryContainer);
            instances.Add(instance.gameObject);
            instance.gameObject.SetActive(true);
            instance.Assign(subtitle);
            Image image = instance.GetComponent<Image>();
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            Text[] texts = instance.GetComponentsInChildren<Text>();
            float preferredWidth = 0f;
            float preferredHeight = 100f;
            foreach (Text t in texts)
            {
                //if (preferredHeight < t.preferredHeight)
                preferredHeight += t.preferredHeight;
                if (preferredWidth < t.preferredWidth)
                    preferredWidth = t.preferredWidth;
            }
            //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
            //rectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);

            // TODO use different bubble image for PLAYER
            if (subtitle.speakerInfo.IsPlayer)
            {
                image.color = Color.yellow;
            } else
            {
                image.color = Color.cyan;
            }

            // Move scroll to bottom
        }
        currentEntryID = subtitle.dialogueEntry.id;
        ScrollToBottomOfScrollView();
    }

    void ScrollToBottomOfScrollView()
    {
        if (scrollView != null && scrollView.verticalScrollbar != null)
        {
            scrollView.verticalScrollbar.value = 0f;
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

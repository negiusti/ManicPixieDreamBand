using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;

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
    public GameObject typingBubbleTemplate;
    private int currentEntryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT_Band" };

    private List<Subtitle> log = new List<Subtitle>();
    private Stack<GameObject> typingBubbles = new Stack<GameObject>();
    private List<GameObject> instances = new List<GameObject>();

    private void Start()
    {
        currentEntryID = 0;
    }

    private void Awake()
    {
        logEntryTemplate.gameObject.SetActive(false);
        speakerNameTemplate.gameObject.SetActive(false);
        typingBubbleTemplate.SetActive(false);
    }

    public int GetCurrEntryID()
    {
        return currentEntryID;
    }

    public void AddToBacklog(Subtitle subtitle)
    {
        StartCoroutine(AddToBacklogWithDelay(subtitle));
    }
    
    private IEnumerator AddToBacklogWithDelay(Subtitle subtitle)
    {
        bool isGroupChat = groupChats.Contains(DialogueManager.LastConversationStarted);
        if (subtitle.dialogueEntry.id == currentEntryID)
            yield return null;

        if (!subtitle.speakerInfo.IsPlayer && !subtitle.dialogueEntry.isRoot)
        {
            // add typing bubble here
            GameObject typingBubble = Instantiate(typingBubbleTemplate, logEntryContainer);
            typingBubble.SetActive(true);
            typingBubbles.Push(typingBubble);
            yield return new WaitForSeconds(2);
            // remove typing bubble here
            typingBubble.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
        }

        if (!string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            GameObject b;
            while (typingBubbles.TryPeek(out _))
            {
                b = typingBubbles.Pop();
                b.SetActive(false);
                Destroy(b);
                LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
            }
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
            
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            Text[] texts = instance.GetComponentsInChildren<Text>();
            float preferredHeight = 100f;
            foreach (Text t in texts)
            {
                preferredHeight += t.preferredHeight;
            }
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

            Image image = instance.GetComponent<Image>();
            if (subtitle.speakerInfo.IsPlayer)
            {
                image.color = Color.yellow;
            } else
            {
                image.color = Color.cyan;
            }
        }
        currentEntryID = subtitle.dialogueEntry.id;
        ScrollToBottomOfScrollView();
        yield return null;
    }

    void ScrollToBottomOfScrollView()
    {
        if (scrollView != null && scrollView.verticalScrollbar != null)
        {
            //Canvas.ForceUpdateCanvases();
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

using System.Collections.Generic;
using System.Linq;
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
    public StandardUIMenuPanel responseMenu;
    private int currentEntryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT/Band" };

    private List<Subtitle> log = new List<Subtitle>();
    private List<GameObject> instances = new List<GameObject>();
    private RectTransform rectTransform;
    private Scrollbar scrollbar;
    private bool responseMenuView;
    private static float longScrollViewHeight = 310.6017f;
    private static float shortScrollViewHeight = 203.4449f;
    //private Queue<IEnumerator> coroutines;
    //private bool inprogress;
    private GameObject typingBubble;
    //private object coroutineLock = new object();

    private void Start()
    {
        logEntryTemplate.gameObject.SetActive(false);
        speakerNameTemplate.gameObject.SetActive(false);
        typingBubbleTemplate.SetActive(false);
        responseMenuView = false;
        rectTransform = GetComponent<RectTransform>();
        scrollbar = GetComponentInChildren<Scrollbar>();
        ResetCurrentEntryID();
    }

    public void ResetCurrentEntryID()
    {
        currentEntryID = 0;
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

    private void Update()
    {
        if (!responseMenuView && responseMenu.gameObject.activeSelf && responseMenu.isOpen)
        {
            // Shorten scroll view
            responseMenuView = true;

            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = shortScrollViewHeight;
            rectTransform.sizeDelta = sizeDelta;

            RectTransform r = scrollbar.gameObject.GetComponent<RectTransform>();
            sizeDelta = r.sizeDelta;
            sizeDelta.y = shortScrollViewHeight;
            r.sizeDelta = sizeDelta;
        } else if (responseMenuView && (!responseMenu.gameObject.activeSelf || !responseMenu.isOpen))
        {
            // Extend scroll view
            responseMenuView = false;

            Vector2 sizeDelta = rectTransform.sizeDelta;
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
        }
        else
        {
            DialogueManager.Pause();
            StartCoroutine(AddToBacklogWithDelay(subtitle));
        }
    }

    private IEnumerator AddToBacklogWithDelay(Subtitle subtitle)
    {
        bool isFirstTxt = subtitle.dialogueEntry.Title.Equals("FIRST");
        bool isGroupChat = groupChats.Any(gc => DialogueManager.LastConversationStarted.Contains(gc));
        if (!isFirstTxt && !subtitle.speakerInfo.IsPlayer && currentEntryID > 0 && !string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            // add typing bubble here
            typingBubble = Instantiate(typingBubbleTemplate, logEntryContainer);
            typingBubble.SetActive(true);
            yield return new WaitForSeconds(2);
            typingBubble.SetActive(false);
            Destroy(typingBubble);
            typingBubble = null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
        }

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

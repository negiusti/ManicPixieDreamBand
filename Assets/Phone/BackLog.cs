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
    public StandardUIMenuPanel responseMenu;
    private int currentEntryID;
    private static HashSet<string> groupChats = new HashSet<string> { "TXT_Band" };

    private List<Subtitle> log = new List<Subtitle>();
    private Stack<GameObject> typingBubbles = new Stack<GameObject>();
    private List<GameObject> instances = new List<GameObject>();
    private RectTransform rectTransform;
    private Scrollbar scrollbar;
    private bool responseMenuView;
    private static float longScrollViewHeight = 310.6017f;
    private static float shortScrollViewHeight = 203.4449f;
    private Queue<IEnumerator> coroutines;
    private bool waitingToRespond;

    private void Start()
    {
        currentEntryID = 0;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Awake()
    {
        logEntryTemplate.gameObject.SetActive(false);
        speakerNameTemplate.gameObject.SetActive(false);
        typingBubbleTemplate.SetActive(false);
        responseMenuView = false;
        rectTransform = GetComponent<RectTransform>();
        scrollbar = GetComponentInChildren<Scrollbar>();
        coroutines = new Queue<IEnumerator>();
    }

    private void Update()
    {
        if (!responseMenuView && responseMenu.gameObject.activeSelf)
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
        } else if (responseMenuView && !responseMenu.gameObject.activeSelf)
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
        //if (responseMenu.gameObject.activeSelf && typingBubbles.Count > 0)
        //{
        //    responseMenu.gameObject.SetActive(false);
        //    //responseMenu.Close();
        //    waitingToRespond = true;
        //}
        //if (waitingToRespond && typingBubbles.Count == 0)
        //{
        //    responseMenu.gameObject.SetActive(true);
        //    responseMenu.Open();
        //    waitingToRespond = false;
        //}
    }

    public int GetCurrEntryID()
    {
        return currentEntryID;
    }

    public void AddToBacklog(Subtitle subtitle)
    {
        if (subtitle.dialogueEntry.id == currentEntryID)
            return;
        else
        {
            coroutines.Enqueue(AddToBacklogWithDelay(subtitle));
            if (coroutines.Count == 1)
                StartCoroutine(RunCoroutineQueue());
        }
    }

    IEnumerator RunCoroutineQueue()
    {
        while (true)
        {
            if (coroutines.Count == 0)
            {
                yield break;
            }
            else
            {
                // Dequeue the next coroutine and start it
                IEnumerator currentCoroutine = coroutines.Dequeue();
                yield return StartCoroutine(currentCoroutine);
            }
        }
    }

    private IEnumerator AddToBacklogWithDelay(Subtitle subtitle)
    {
        currentEntryID = subtitle.dialogueEntry.id;
        bool isGroupChat = groupChats.Contains(DialogueManager.LastConversationStarted);
        if (!subtitle.speakerInfo.IsPlayer && instances.Count > 0 && !string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            // add typing bubble here
            yield return new WaitForSeconds(2);
            GameObject typingBubble = Instantiate(typingBubbleTemplate, logEntryContainer);
            typingBubble.SetActive(true);
            typingBubbles.Push(typingBubble);
            yield return new WaitForSeconds(2);
        }

        if (!string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            while (typingBubbles.TryPeek(out _))
            {
                GameObject b = typingBubbles.Pop();
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

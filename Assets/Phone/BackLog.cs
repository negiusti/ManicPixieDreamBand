using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// This script runs the back log. It records dialogue lines as they're played.
/// The ShowBackLog() method fills a back log window with the recorded lines.
/// </summary>
public class BackLog : MonoBehaviour
{

    public Transform logEntryContainer;
    public LogEntry logEntryTemplate;
    public ScrollRect scrollView;
    public Sprite pinkSprite;
    public Sprite greenSprite;
    private int currentEntryID;

    private List<Subtitle> log = new List<Subtitle>();
    private List<GameObject> instances = new List<GameObject>();

    private void Start()
    {
        currentEntryID = 0;
    }

    private void Awake()
    {
        logEntryTemplate.gameObject.SetActive(false);
    }

    public int GetCurrEntryID()
    {
        return currentEntryID;
    }
    
    public void AddToBacklog(Subtitle subtitle)
    {
        ScrollToBottomOfScrollView();
        if (!string.IsNullOrEmpty(subtitle.formattedText.text))
        {
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
                image.sprite = greenSprite;
                instance.speakerName.enabled = false;
            } else
            {
                image.sprite = pinkSprite;
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

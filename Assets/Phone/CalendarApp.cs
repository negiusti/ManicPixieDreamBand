using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CalendarApp : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    private TextMeshPro tmp;
    public PhoneCalendarEvent[] eventBubbles;
    public GameObject line;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = this.GetComponentInChildren<SpriteResolver>();
        tmp = this.GetComponentInChildren<TextMeshPro>();
        eventBubbles = this.GetComponentsInChildren<PhoneCalendarEvent>().ToArray();
        UpdateImage();
        ShowTodaysEvents();
    }

    private void ShowTodaysEvents()
    {
        List<ICalendarEvent> events = Calendar.GetTodaysEvents();
        int i = 0;
        foreach (PhoneCalendarEvent e in eventBubbles)
        {
            if (events.Count <= i)
            {
                e.gameObject.SetActive(false);
            } else
            {
                e.AssignEvent(events[i]);
                if (i < Calendar.GetCurrentEventIdx())
                {
                    e.Complete();
                }
                else
                {
                    e.NotComplete();
                }
            }
            i++;
        }
        line.transform.SetSiblingIndex(Calendar.GetCurrentEventIdx());
    }

    void UpdateImage()
    {
        if (Calendar.IsNight())
        {
            spriteResolver.SetCategoryAndLabel("MoonSun", "moon");
        }
        else
        {
            spriteResolver.SetCategoryAndLabel("MoonSun", "sun");
        }
        tmp.text = Calendar.Date().ToString();
    }

    public void Sleep()
    {
        Calendar.Sleep();
        UpdateImage();
        ShowTodaysEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDayNight()
    {
        Calendar.ToggleDayNight();
        UpdateImage();
        ShowTodaysEvents();
    }

    public void SetIsNight(bool value)
    {
        Calendar.SetIsNight(value);
        UpdateImage();
        ShowTodaysEvents();
    }

    // FOR DEBUG BUTTON ONLY
    public void CompleteCurrentEvent() 
    {
        eventBubbles[Calendar.GetCurrentEventIdx()].Complete();
        Calendar.CompleteCurrentEvent();
        UpdateImage();
        ShowTodaysEvents();
    }
}

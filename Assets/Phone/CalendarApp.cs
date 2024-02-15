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
        GetTodaysEvents();
    }

    private void GetTodaysEvents()
    {
        List<CalendarEvent> events = Calendar.GetTodaysEvents();
        int i = 0;
        foreach (PhoneCalendarEvent e in eventBubbles)
        {
            if (events.Count <= i)
            {
                e.gameObject.SetActive(false);
            } else
            {
                e.AssignEvent(events[i]);
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
        GetTodaysEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDayNight()
    {
        Calendar.ToggleDayNight();
        UpdateImage();
        GetTodaysEvents();
    }

    public void SetIsNight(bool value)
    {
        Calendar.SetIsNight(value);
        UpdateImage();
        GetTodaysEvents();
    }

    public void CompleteCurrentEvent() 
    {
        Calendar.CompleteCurrentEvent();
        UpdateImage();
        GetTodaysEvents();
    }
}

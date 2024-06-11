using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CalendarApp : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    public TextMeshPro dayNum;
    public TextMeshPro weatherInfo;
    public PhoneCalendarEvent[] eventBubbles;
    public GameObject line;
    public PhoneIcon phoneIcon;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponentInChildren<SpriteResolver>();
        eventBubbles = GetComponentsInChildren<PhoneCalendarEvent>().ToArray();
        UpdateImage();
        ShowTodaysEvents();
    }

    private void OnEnable()
    {
        phoneIcon.HideNotificationIndicator();
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
            if (Weather.Current().Equals(Weather.WeatherState.Rainy))
                spriteResolver.SetCategoryAndLabel("MoonSun", "moon_rain");
            else
                spriteResolver.SetCategoryAndLabel("MoonSun", "moon");
        }
        else
        {
            if (Weather.Current().Equals(Weather.WeatherState.Rainy))
                spriteResolver.SetCategoryAndLabel("MoonSun", "sun_rain");
            else
                spriteResolver.SetCategoryAndLabel("MoonSun", "sun");
        }
        dayNum.text = Calendar.Date().ToString();
        if (Weather.Current().Equals(Weather.WeatherState.Rainy))
            weatherInfo.text = "rainy\n69º";
        else
            weatherInfo.text = "partly cloudy\n54º";
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

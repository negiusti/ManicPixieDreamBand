using UnityEngine;

public interface ICalendarEvent
{
    string Location();
    string Name();    
    bool IsNight();
}
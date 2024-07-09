using UnityEngine;

public interface ICalendarEvent
{
    string Location();
    string Name();    
    bool IsNight();
    void OnConversationComplete(string convoName);
}
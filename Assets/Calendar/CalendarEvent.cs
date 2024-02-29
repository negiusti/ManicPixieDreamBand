using UnityEngine;

public interface ICalendarEvent
{
    string Location();
    string Name();
    GameObject Minigame();
    bool IsNight();
    void OnConversationComplete(string convoName);
}
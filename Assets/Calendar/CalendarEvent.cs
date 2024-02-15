using UnityEngine;

public interface CalendarEvent
{
    string Location();
    string Name();
    string BeforeConversation();
    GameObject Minigame();
    string AfterConversation();
    bool IsNight();
}
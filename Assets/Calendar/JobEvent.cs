using UnityEngine;

public class JobEvent : CalendarEvent
{
    private string name;
    private string beforeConversation;
    private string afterConversation;
    private GameObject minigame;
    private string location;
    private bool isNight;

    public JobEvent(string name, string beforeConversation, GameObject minigame, string afterConversation, bool isNight, string location)
    {
        this.name = name;
        this.beforeConversation = beforeConversation;
        this.minigame = minigame;
        this.afterConversation = afterConversation;
        this.location = location;
        this.isNight = isNight;
    }

    public string Name()
    {
        return "work at " + Location();
    }

    public string BeforeConversation()
    {
        return beforeConversation;
    }

    public GameObject Minigame()
    {
        return minigame;
    }

    public string Location()
    {
        return location;
    }

    public string AfterConversation()
    {
        return afterConversation;
    }

    public bool IsNight()
    {
        return isNight;
    }
}
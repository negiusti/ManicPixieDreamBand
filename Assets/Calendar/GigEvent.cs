using UnityEngine;

public class GigEvent : CalendarEvent
{
    [SerializeField] private string name;
    [SerializeField] private string beforeConversation;
    [SerializeField] private string afterConversation;
    [SerializeField] private GameObject minigame;
    [SerializeField] private string location;
    [SerializeField] private bool isNight;

    public GigEvent(string name, string beforeConversation, GameObject minigame, string afterConversation, bool isNight = true, string location = "SmallBar")
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
        return "play show at " + Location();
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
using UnityEngine;

public class BandPracticeEvent : CalendarEvent
{
    [SerializeField] private string beforeConversation;
    [SerializeField] private string afterConversation;
    [SerializeField] private GameObject minigame;
    [SerializeField] private string location;
    [SerializeField] private bool isNight;

    public BandPracticeEvent(string beforeConversation, GameObject minigame, string afterConversation, bool isNight, string location = "Basement")
    {
        this.beforeConversation = beforeConversation;
        this.minigame = minigame;
        this.afterConversation = afterConversation;
        this.location = location;
        this.isNight = isNight;
    }

    public string Name()
    {
        return "band practice at " + Location();
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
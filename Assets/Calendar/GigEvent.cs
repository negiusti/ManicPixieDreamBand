using UnityEngine;

public class GigEvent : ICalendarEvent
{
    [SerializeField] private string name;
    [SerializeField] private GameObject minigame;
    [SerializeField] private string location;
    [SerializeField] private bool isNight;

    public GigEvent(string name, GameObject minigame, bool isNight = true, string location = "SmallBar")
    {
        this.name = name;
        this.minigame = minigame;
        this.location = location;
        this.isNight = isNight;
    }

    public string Name()
    {
        return "play show at " + Location();
    }

    public GameObject Minigame()
    {
        return minigame;
    }

    public string Location()
    {
        return location;
    }

    public bool IsNight()
    {
        return isNight;
    }

    public void OnConversationComplete(string convoName)
    {
        throw new System.NotImplementedException();
    }
}
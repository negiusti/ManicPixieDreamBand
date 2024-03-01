using UnityEngine;

public class JobEvent : ICalendarEvent
{
    [SerializeField] private string name;
    [SerializeField] private GameObject minigame;
    [SerializeField] private string location;
    [SerializeField] private bool isNight;

    public JobEvent(string name, GameObject minigame, bool isNight, string location)
    {
        this.name = name;
        this.minigame = minigame;
        this.location = location;
        this.isNight = isNight;
    }

    public string Name()
    {
        return "work at " + Location();
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
        if (SceneChanger.Instance.GetCurrentScene() == location)
        {
            Calendar.CompleteCurrentEvent();
        }
    }
}
using UnityEngine;

public class BandPracticeEvent : ICalendarEvent
{
    [SerializeField] private GameObject minigame;
    [SerializeField] private string location;
    [SerializeField] private bool isNight;

    public BandPracticeEvent(GameObject minigame, bool isNight, string location = "Warehouse")
    {
        this.minigame = minigame;
        this.location = location;
        this.isNight = isNight;
    }

    public string Name()
    {
        return "band practice @ warehouse";
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
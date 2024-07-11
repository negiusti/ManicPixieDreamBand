using UnityEngine;

public class QuestEvent : ICalendarEvent
{
    [SerializeField] private string name;
    [SerializeField] private string conversation;
    [SerializeField] private string location;
    [SerializeField] private bool isNight;

    public QuestEvent(string name, string conversation, bool isNight, string location)
    {
        this.name = name;
        this.location = location;
        this.isNight = isNight;
        this.conversation = conversation;
    }

    public string Name()
    {
        return name;
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

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestManager", menuName = "Custom/QuestManager")]
public class QuestManager : ScriptableObject
{
    private ConversationsData quests = ConversationJson.GetQuestsData();
    private ConversationData currentQuest;

    private static void StartNewQuest()
    {
        
    }
}

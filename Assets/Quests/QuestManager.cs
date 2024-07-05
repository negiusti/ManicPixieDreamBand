using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestManager", menuName = "Custom/QuestManager")]
public class QuestManager : ScriptableObject
{
    private ConversationsData quests = ConversationJson.GetQuestsData();
    private Quest currentQuest;
    private HashSet<string> completedQuests;
    private HashSet<string> availableQuests;

    private static void StartNewQuest()
    {
        
    }

    private static void CompleteCurrentQuest()
    {

    }

    public static void Save()
    {

    }

    public static void Load()
    {

    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestManager", menuName = "Custom/QuestManager")]
public class QuestManager : ScriptableObject
{
    private static Dictionary<string, Quest> quests;
    private static Quest currentQuest;
    private static HashSet<string> completedQuests;
    private static HashSet<string> availableQuests;

    private static void StartNewQuest()
    {
        
    }

    private static void CompleteCurrentQuest()
    {
        availableQuests.Remove(currentQuest.questName);
        completedQuests.Add(currentQuest.questName);
        currentQuest = null;
    }

    public static bool QuestCurrentlyActive()
    {
        return currentQuest != null;
    }

    public static void Save()
    {
        //ES3.Save("CurrentQuest", currentQuest.questName);
    }

    public static void Load()
    {
        //quests = ConversationJson.GetQuestsData().quests.ToDictionary(q => q.questName, q => q);
        //string currentQuestName = ES3.Load<string>("CurrentQuest", defaultValue: null);
        //currentQuest = currentQuestName == null ? null : quests[currentQuestName];
    }
}

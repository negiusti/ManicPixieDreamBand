using System.Linq;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestManager", menuName = "Custom/QuestManager")]
public class QuestManager : ScriptableObject
{
    private static Dictionary<string, Quest> quests;
    private static Quest currentQuest;
    private static int currQuestConvoIdx;
    private static HashSet<string> completedQuests;
    private static HashSet<string> availableQuests;

    public static bool CheckForQuestConvo()
    {
        if (DialogueManager.IsConversationActive)
            return false;
        if (currentQuest != null)
        {
            //Debug.Log("current quest is: " + currentQuest.questName);
            if (currQuestConvoIdx >= currentQuest.conversationsData.Length)
            {
                CompleteCurrentQuest();
                return false;
            }
                
            bool requirementsMet = ConvoRequirements.RequirementsMet(currentQuest.conversationsData[currQuestConvoIdx].requirements) &&
                (!SceneChanger.Instance.IsLoadingScreenOpen() || !DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().IsTxtConvo(currentQuest.conversationsData[currQuestConvoIdx].conversation));
            if (requirementsMet)
                DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().StartConversation(currentQuest.conversationsData[currQuestConvoIdx++]);
            return requirementsMet;
        } else
        {
            foreach (string questName in availableQuests)
            {
                //Debug.Log("searching for new quest: " + questName);
                //Debug.Log("checking requirements for: " + quests[questName].conversationsData[0].conversation);
                bool requirementsMet = ConvoRequirements.RequirementsMet(quests[questName].conversationsData[0].requirements) &&
                     (!SceneChanger.Instance.IsLoadingScreenOpen() || !DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().IsTxtConvo(quests[questName].conversationsData[0].conversation));
                if (requirementsMet)
                {
                    currQuestConvoIdx = 0;
                    currentQuest = quests[questName];
                    availableQuests.Remove(questName);
                    DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().StartConversation(quests[questName].conversationsData[currQuestConvoIdx++]);
                    return true;
                }
            }
        }
        return false;
    }

    public static HashSet<string> CompletedQuests()
    {
        return completedQuests;
    }

    public static void CompleteCurrentQuest()
    {
        availableQuests.Remove(currentQuest.questName);
        completedQuests.Add(currentQuest.questName);
        currentQuest = null;
        currQuestConvoIdx = 0;
    }

    public static bool QuestCurrentlyActive()
    {
        return currentQuest != null;
    }

    public static void Save()
    {
        ES3.Save("CurrentQuest", currentQuest != null ? currentQuest.questName : null);
        ES3.Save("CurrentQuestIdx", currQuestConvoIdx);
        ES3.Save("CompletedQuests", completedQuests);
    }

    public static void Load()
    {
        quests = ConversationJson.GetQuestsData().quests.ToDictionary(q => q.questName, q => q);
        string currentQuestName = ES3.Load<string>("CurrentQuest", defaultValue: null);
        currentQuest = currentQuestName == null ? null : quests[currentQuestName];
        currQuestConvoIdx = ES3.Load("CurrentQuestIdx", 0);
        completedQuests = ES3.Load("CompletedQuests", new HashSet<string>());
        availableQuests = quests.Keys.Where(q => !completedQuests.Contains(q)).ToHashSet();
    }
}

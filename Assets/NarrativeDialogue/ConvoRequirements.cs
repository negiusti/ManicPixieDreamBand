//public class Requirements
//{
//    public string currentJob;
//    public RelationshipScore relationshipScore;

//    public float minMoney;
//    public float maxMoney;

//    public int dayNum;
//    public int maxDay;
//    public int minDay;
//    public string[] pocketsItems;
//    public string[] completedQuests;
//    public string currentEventType;
//    string location;
//}
using System.Linq;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ConvoRequirements", menuName = "Custom/ConvoRequirements")]
public class ConvoRequirements : ScriptableObject
{
    public static string CurrentJob()
    {
        return JobSystem.CurrentJob().ToString();
    }

    public static bool ContainsItems(string[] items)
    {
        return items.All(i => InventoryManager.GetPocketItems().Keys.Select(i => i.ToString()).Contains(i));
    }

    public static float CurrentMoney()
    {
        return (float)MainCharacterState.CurrentBankBalance();
    }

    public static bool CompletedQuests(string[] quests)
    {
        return quests.All(q => QuestManager.CompletedQuests().Contains(q));
    }

    public static string CurrentEventType()
    {
        return Calendar.GetCurrentEvent().GetType().ToString();
    }

    public static int CurrentDay()
    {
        return Calendar.Date();
    }

    public static string CurrentLocation()
    {
        return SceneChanger.Instance.GetCurrentScene();
    }

    public static bool RequirementsMet(Requirements requirements)
    {
        //    public string currentJob;
        //    public RelationshipScore relationshipScore;

        //    public float minMoney;
        //    public float maxMoney;

        //    public int dayNum;
        //    public int maxDay;
        //    public int minDay;
        //    public string[] pocketsItems;
        //    public string[] completedQuests;
        //    public string currentEventType;
        if (requirements == null)
        {
            Debug.Log("requirements are null");
            return true;
        }
        if (requirements.currentJob != null && requirements.currentJob != CurrentJob())
        {
            Debug.Log("job req not met");
            return false;
        }
        if (requirements.minMoney > 0 && CurrentMoney() < requirements.minMoney)
        {
            Debug.Log("money req not met");
            return false;
        }
        if (requirements.minMoney > 0 && CurrentMoney() > requirements.maxMoney)
        {
            Debug.Log("money req not met");
            return false;
        }
        if (requirements.dayNum > 0 && CurrentDay() != requirements.dayNum)
        {
            Debug.Log("dayNum req not met");
            return false;
        }
        if (requirements.maxDay > 0 && CurrentDay() > requirements.maxDay)
        {
            Debug.Log("dayNum req not met");
            return false;
        }
        if (requirements.minDay > 0 && CurrentDay() < requirements.minDay)
        {
            Debug.Log("dayNum req not met");
            return false;
        }
        if (requirements.pocketsItems != null && !ContainsItems(requirements.pocketsItems))
        {
            Debug.Log("pockets req not met");
            return false;
        }
        if (requirements.completedQuests != null && !CompletedQuests(requirements.completedQuests))
        {
            Debug.Log("quests req not met");
            return false;
        }
        if (requirements.currentEventType != null && requirements.currentEventType != CurrentEventType())
        {
            Debug.Log("event type req not met");
            return false;
        }
        if (requirements.locations != null && !requirements.locations.Contains(CurrentLocation()))
        {
            Debug.Log("location req not met: " + requirements.locations[0] + " " + CurrentLocation());
            return false;
        }
        if (requirements.relationshipScore != null && requirements.relationshipScore.npc != null && RomanceManager.GetRelationshipScore(requirements.relationshipScore.npc) < requirements.relationshipScore.score)
        {
            Debug.Log("relationshipScore req not met: " + requirements.relationshipScore.npc);
            return false;
        }
        if (requirements.changedOutfitToday && !MainCharacterState.HasChangedOutfitToday())
        {
            Debug.Log("HasChangedOutfitToday req not met");
            return false;
        }

        if (requirements.falseFlags != null)
        {
            Debug.Log("falseFlags req not met");
            if (requirements.falseFlags.Any(f => MainCharacterState.CheckFlag(f)))
            {
                return false;
            }
        }

        if (requirements.trueFlags != null)
        {
            Debug.Log("trueFlags req not met");
            if (requirements.trueFlags.Any(f => !MainCharacterState.CheckFlag(f)))
            {
                return false;
            }
        }
        return true;
    }
}

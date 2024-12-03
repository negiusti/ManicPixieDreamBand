using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.IO;

[System.Serializable]
public class Participant
{
    public string name;
    public Vector3 position;
    public string faceLeftOrRight;
    public string layer;
    public bool isTrigger; // does interacting with this participant trigger the conversation
    public bool existAtStart;
}

[System.Serializable]
public class ConversationData
{
    public string[] locations; // possible locations to trigger conversation, this should almost ALWAYS be one location
    public Participant[] participants;
    public string trigger; // either Start or the name of the participant to interact with
    public string conversation;
    public Requirements requirements; // Requirements must be met to trigger the conversation
}

[System.Serializable] 
public class Requirements
{
    public string currentJob;
    public RelationshipScore relationshipScore;

    public float minMoney = -1f;
    public float maxMoney = -1f;

    public int dayNum = -1;
    public int maxDay = -1;
    public int minDay = -1;
    public string[] pocketsItems;
    public string[] completedQuests;
    public string currentEventType;
    public string eventName;
    public string[] locations;
    public bool changedOutfitToday;
    public string[] trueFlags;
    public string[] falseFlags;
}

[System.Serializable]
public class RelationshipScore
{
    public string npc;
    public int score;
}

[System.Serializable]
public class PlotConversationsData
{
    public ConversationData[] conversationsData;
}

[System.Serializable]
public class Quest
{
    public ConversationData[] conversationsData;
    public string questName;
}

[System.Serializable]
public class QuestsData
{
    public Quest[] quests;
}

[System.Serializable]
public class Romance
{
    public string npcName;
    public ConversationData[] conversationsData;
}

[System.Serializable]
public class RomancesData
{
    public Romance[] romances;
}

[CreateAssetMenu(fileName = "ConversationJson", menuName = "Custom/ConversationJson")]
public class ConversationJson : ScriptableObject
{
    private static string plotConvoJsonPath = "Assets/Conversations/plot.json";
    private static string questsConvoJsonPath = "Assets/Conversations/quests.json";
    private static string romancesConvoJsonPath = "Assets/Conversations/romances.json";
    private static PlotConversationsData plotData;
    private static QuestsData questsData;
    private static RomancesData romancesData;

    public static AsyncOperationHandle<TextAsset> LoadFromJson()
    {
        AsyncOperationHandle<TextAsset> p = Addressables.LoadAssetAsync<TextAsset>(plotConvoJsonPath);
        p.Completed += OnPlotDataLoaded;
        return p;
    }

    public static AsyncOperationHandle<TextAsset> LoadQuestsFromJson()
    {
        AsyncOperationHandle<TextAsset> q = Addressables.LoadAssetAsync<TextAsset>(questsConvoJsonPath);
        q.Completed += OnQuestsDataLoaded;
        return q;
    }

    public static AsyncOperationHandle<TextAsset> LoadRomancesFromJson()
    {
        AsyncOperationHandle<TextAsset> q = Addressables.LoadAssetAsync<TextAsset>(romancesConvoJsonPath);
        q.Completed += OnRomancesDataLoaded;
        return q;
    }

    private static void OnPlotDataLoaded(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonData = obj.Result.text;
            plotData = JsonUtility.FromJson<PlotConversationsData>(jsonData);
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    private static void OnQuestsDataLoaded(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonData = obj.Result.text;
            questsData = JsonUtility.FromJson<QuestsData>(jsonData);
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    private static void OnRomancesDataLoaded(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonData = obj.Result.text;
            romancesData = JsonUtility.FromJson<RomancesData>(jsonData);
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    public static PlotConversationsData GetPlotData()
    {
        return plotData;
    }

    public static QuestsData GetQuestsData()
    {
        return questsData;
    }

    public static RomancesData GetRomancesData()
    {
        return romancesData;
    }

}

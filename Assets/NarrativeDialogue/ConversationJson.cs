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

[CreateAssetMenu(fileName = "ConversationJson", menuName = "Custom/ConversationJson")]
public class ConversationJson : ScriptableObject
{
    private static string plotConvoJsonPath = "Assets/Conversations/plot.json";
    private static string questsConvoJsonPath = "Assets/Conversations/quests.json";
    private static PlotConversationsData plotData;
    private static QuestsData questsData;

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

    public static PlotConversationsData GetPlotData()
    {
        return plotData;
    }

    public static QuestsData GetQuestsData()
    {
        return questsData;
    }

}

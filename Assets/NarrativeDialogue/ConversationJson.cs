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
    public string[] locations;
    public Participant[] participants;
    public string trigger; // either Start or the name of the participant to interact with
    public string conversation;
}

[System.Serializable]
public class ConversationsData
{
    public ConversationData[] conversationsData;
}

[CreateAssetMenu(fileName = "ConversationJson", menuName = "Custom/ConversationJson")]
public class ConversationJson : ScriptableObject
{
    private static string plotConvoJsonPath = "Assets/Conversations/plot.json";
    private static string questsConvoJsonPath = "Assets/Conversations/quests.json";
    //private static string randomQuestsConvoJsonPath = "Assets/Conversations/plot.json";
    //private static string npcQuestsConvoJsonPath = "Assets/Conversations/plot.json";
    private static ConversationsData plotData;
    private static ConversationsData questsData;

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
            plotData = JsonUtility.FromJson<ConversationsData>(jsonData);
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
            questsData = JsonUtility.FromJson<ConversationsData>(jsonData);
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    public static ConversationsData GetPlotData()
    {
        return plotData;
    }

    public static ConversationsData GetQuestsData()
    {
        return questsData;
    }

}

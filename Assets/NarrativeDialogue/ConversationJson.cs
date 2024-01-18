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
    public bool existAtStart;
}

[System.Serializable]
public class ConversationData
{
    public string[] locations;
    public Participant[] participants;
    public string trigger;
    public string conversation;
}

[System.Serializable]
public class PlotData
{
    public ConversationData[] conversationsData;
}

[CreateAssetMenu(fileName = "ConversationJson", menuName = "Custom/ConversationJson", order = 1)]
public class ConversationJson : ScriptableObject
{
    private static string plotConvoJsonPath = "Assets/Conversations/plot.json";
    private static PlotData plotData;

    public static void LoadFromJson()
    {
        Addressables.LoadAssetAsync<TextAsset>(plotConvoJsonPath).Completed += OnPlotDataLoaded;
    }

    private static void OnPlotDataLoaded(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonData = obj.Result.text;
            plotData = JsonUtility.FromJson<PlotData>(jsonData);
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    public static PlotData GetPlotData()
    {
        return plotData;
    }

}

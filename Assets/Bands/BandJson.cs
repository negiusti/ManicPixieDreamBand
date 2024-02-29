using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class BandsData
{
    public List<Band> bandsData;
}

[CreateAssetMenu(fileName = "BandJson", menuName = "Custom/BandJson")]
public class BandJson : ScriptableObject
{
    private static string bandsJsonPath = "Assets/Bands/bands.json";
    private static BandsData bandsData;

    public static AsyncOperationHandle<TextAsset> LoadFromJson()
    {
        AsyncOperationHandle<TextAsset> x = Addressables.LoadAssetAsync<TextAsset>(bandsJsonPath);
        x.Completed += OnBandDataLoaded;
        return x;
    }

    private static void OnBandDataLoaded(AsyncOperationHandle<TextAsset> obj)
    {
        var bd = new BandsData();
        bd.bandsData = new List<Band>();
        var b = new Band();
        b.AvgTixSales = 2;
        b.leftAmp = "sldkfj";
        b.rightAmp = "sdlkf";
        b.Name = "LB";
        var bm = new BandMember();
        bm.band = "LB";
        bm.instrument = "gtr";
        bm.name = "toots";
        bm.position = "left";
        b.members = new BandMember[] {bm,bm };
        bd.bandsData.Add(b);

        Debug.Log("LOOK HERE IDIOT: " + JsonUtility.ToJson(bd));
        Debug.Log("LOOK HERE IDIOT: " + JsonUtility.ToJson(b));
        Debug.Log("LOOK HERE IDIOT: " + JsonUtility.ToJson(bm));

        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonData = obj.Result.text;
            bandsData = JsonUtility.FromJson<BandsData>(jsonData);
        }
        else
        {
            Debug.LogError("Failed to load text from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    public static List<Band> GetBandsData()
    {
        return bandsData.bandsData;
    }

}

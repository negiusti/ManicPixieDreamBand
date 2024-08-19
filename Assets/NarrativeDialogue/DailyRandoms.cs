using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "DailyRandoms", menuName = "Custom/DailyRandoms")]
public class DailyRandoms : ScriptableObject
{
    [System.Serializable]
    public class DailyRandomsData
    {
        public string[] drinkSpecials;
    }
    private static DailyRandomsData data;
    private static string jsonPath = "Assets/Conversations/dailyrandoms.json";
    public static string[] DrinkSpecials;
    private static string currentDrinkSpecial;

    public static string DrinkSpecial()
    {
        if (currentDrinkSpecial == null)
            Refresh();
        return currentDrinkSpecial;
    }

    public static void Refresh()
    {
        if (data == null)
            LoadFromJson().WaitForCompletion();
        string prevDrinkSpecial = currentDrinkSpecial;
        do
        {
            currentDrinkSpecial = DrinkSpecials[Random.Range(0, DrinkSpecials.Length)];
        } while (currentDrinkSpecial == prevDrinkSpecial);
    }

    public static AsyncOperationHandle<TextAsset> LoadFromJson()
    {
        AsyncOperationHandle<TextAsset> p = Addressables.LoadAssetAsync<TextAsset>(jsonPath);
        p.Completed += OnDataLoaded;
        return p;
    }

    private static void OnDataLoaded(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string jsonData = obj.Result.text;
            Debug.Log("DRINK JSON: " + jsonData);
            data = JsonUtility.FromJson<DailyRandomsData>(jsonData);
            Debug.Log("DRINK DATA: " + data);
            DrinkSpecials = data.drinkSpecials;
            Debug.Log("DRINK SPECIALS: " + DrinkSpecials);
            if (data == null)
            {
                Debug.LogError("Deserialization returned null.");
            }
            else if (data.drinkSpecials == null || data.drinkSpecials.Length == 0)
            {
                Debug.LogWarning("Deserialized object, but DrinkSpecials is null or empty.");
            }
            else
            {
                Debug.Log("Deserialization successful. Number of items: " + data.drinkSpecials.Length);
                foreach (var special in data.drinkSpecials)
                {
                    Debug.Log(special);
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }
}

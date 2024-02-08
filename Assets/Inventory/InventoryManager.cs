using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventoryManager", menuName = "Custom/InventoryManager")]
public class InventoryManager : ScriptableObject
{
    private static Dictionary<string, Dictionary<string, HashSet<string>>> characterInventories;
    private static string invSaveKey = "CharacterInventories";
    private static Dictionary<string, HashSet<string>> cateogoryToPurchased;
    private static string purchasedSaveKey = "PurchasedInventory";

    public static void SaveInventories()
    {
        ES3.Save(invSaveKey, characterInventories);
        ES3.Save(purchasedSaveKey, cateogoryToPurchased);
    }

    public static void AddToInventory(string character, string category, string item)
    {
        GetCharacterInventory(character, category).Add(item);
    }

    public static void RemoveFromInventory(string character, string category, string item)
    {
        GetCharacterInventory(character, category).Remove(item);
    }

    public static void TransferBetweenInventories(string rcvCharacter, string sndCharacter, string category, string item)
    {
        RemoveFromInventory(sndCharacter, category, item);
        AddToInventory(rcvCharacter, category, item);
    }

    public static HashSet<string> GetPurchasedItems(string category)
    {
        return cateogoryToPurchased.GetValueOrDefault(category, defaultValue: new HashSet<string>());
    }

    public static void LoadInventories()
    {
        if (ES3.KeyExists(invSaveKey))
        {
            characterInventories = (Dictionary<string, Dictionary<string, HashSet<string>>>)ES3.Load(invSaveKey);
        } else
        {
            Debug.LogError("Could not find characterInventories in easy save system");
        }
        if (ES3.KeyExists(purchasedSaveKey))
        {
            cateogoryToPurchased = ES3.Load<Dictionary<string, HashSet<string>>>(purchasedSaveKey);
        }
        else
        {
            Debug.LogError("Could not find cateogoryToPurchased in easy save system");
        }
    }

    public static HashSet<string> GetCharacterInventory(string character, string category)
    {
        if (characterInventories == null)
            LoadInventories();
        if (!characterInventories.ContainsKey(character))
            return new HashSet<string>();
        if (!characterInventories[character].ContainsKey(category))
            return new HashSet<string>();
        return characterInventories[character][category];
    }

    private void OnDestroy()
    {
        SaveInventories();
    }
}
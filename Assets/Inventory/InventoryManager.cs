using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventoryManager", menuName = "Custom/InventoryManager")]
public class InventoryManager : ScriptableObject
{
    private static Dictionary<string, Dictionary<string, HashSet<string>>> characterInventories;
    private static string invSaveKey = "CharacterInventories";
    private static Dictionary<string, HashSet<string>> categoryToPurchased;
    private static string purchasedSaveKey = "PurchasedInventory";
    private static string MAIN_CHARACTER = "MainCharacter";

    public static void SaveInventories()
    {
        ES3.Save(invSaveKey, characterInventories);
        ES3.Save(purchasedSaveKey, categoryToPurchased);
    }

    public static void AddToInventory(string character, string category, string item)
    {
        GetCharacterInventory(character, category).Add(item);
    }

    public static void AddToMCInventory(string category, string item)
    {
        AddToInventory(MAIN_CHARACTER, category, item);
    }

    public static void RemoveFromInventory(string character, string category, string item)
    {
        GetCharacterInventory(character, category).Remove(item);
    }

    public static void RemoveFromMCInventory(string category, string item)
    {
        RemoveFromInventory(MAIN_CHARACTER, category, item);
    }

    public static void TransferBetweenInventories(string rcvCharacter, string sndCharacter, string category, string item)
    {
        RemoveFromInventory(sndCharacter, category, item);
        AddToInventory(rcvCharacter, category, item);
    }

    public static void MCGivesTo(string npc, string category, string item)
    {
        TransferBetweenInventories(npc, MAIN_CHARACTER, category, item);
    }

    public static void MCReceivesFrom(string npc, string category, string item)
    {
        TransferBetweenInventories(MAIN_CHARACTER, npc, category, item);
    }

    public static HashSet<string> GetPurchasedItems(string category)
    {       
        return categoryToPurchased.GetValueOrDefault(category, defaultValue: new HashSet<string>());
    }

    public static void RecordPurchase(string category, string item)
    {
        if (!categoryToPurchased.ContainsKey(category))
            categoryToPurchased[category] = new HashSet<string>();
        categoryToPurchased[category].Add(item);
    }

    public static void LoadInventories()
    {
        if (ES3.KeyExists(invSaveKey))
        {
            characterInventories = (Dictionary<string, Dictionary<string, HashSet<string>>>)ES3.Load(invSaveKey);
        }
        if (characterInventories == null)
        {
            characterInventories = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            Debug.LogError("Could not find characterInventories in easy save system");
        }
        if (ES3.KeyExists(purchasedSaveKey))
        {
            categoryToPurchased = ES3.Load<Dictionary<string, HashSet<string>>>(purchasedSaveKey);
        }
        if (categoryToPurchased == null)
        {
            categoryToPurchased = new Dictionary<string, HashSet<string>>();
            Debug.LogError("Could not find cateogoryToPurchased in easy save system");
        }
    }

    public static HashSet<string> GetCharacterInventory(string character, string category)
    {
        if (characterInventories == null)
            LoadInventories();
        if (!characterInventories.ContainsKey(character))
        {
            characterInventories.Add(character, new Dictionary<string, HashSet<string>>());
        }
        category = GetInventoryCategory(category);
        if (!characterInventories[character].ContainsKey(category))
        {
            characterInventories[character].Add(category, new HashSet<string>());
        }
        return characterInventories[character][category];
    }

    public static HashSet<string> GetMCInventory(string category)
    {
        return GetCharacterInventory(MAIN_CHARACTER, category);
    }

    //private void OnDestroy()
    //{
    //    SaveInventories();
    //}

    private static string GetInventoryCategory(string category)
    {
        if (category.Contains("Shoe"))
            return "Shoe_Icons";
        else if (category.Contains("Sock"))
            return "Sock_Icons";
        else if (category.Contains("FB_"))
            return "FB_Icons";
        else if (category.Contains("Top"))
            return "Top_Icons";
        else if (category.Contains("Pant") || category.Contains("Crotch"))
            return "Bottom_Icons";
        else
            return category;
    }
}
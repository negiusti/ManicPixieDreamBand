using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventoryManager", menuName = "Custom/InventoryManager")]
public class InventoryManager : ScriptableObject
{
    private static Dictionary<string, Dictionary<string, HashSet<string>>> characterInventories;
    private static string SaveKey = "CharacterInventories";

    public static void SaveInventories()
    {
        ES3.Save(SaveKey, characterInventories);
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

    public static void LoadInventories()
    {
        if (ES3.KeyExists(SaveKey))
        {
            characterInventories = (Dictionary<string, Dictionary<string, HashSet<string>>>)ES3.Load(SaveKey);
        } else
        {
            Debug.LogError("Could not find characterInventories in easy save system");
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
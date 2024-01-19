using PixelCrushers.DialogueSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "MainCharacterState", menuName = "Custom/MainCharacterState")]
public class MainCharacterState : ScriptableObject
{
    // We store MC-specific info here like: 
    // money, unlocked clothes/furniture/instruments, inventory, etc
    private static string characterName;
    private static float bankBalance;
    private static bool hasChangedOutfitToday;

    public static bool HasChangedOutfitToday()
    {
        Debug.Log("HasChangedOutfitToday: " + hasChangedOutfitToday);
        return hasChangedOutfitToday;
    }

    public static void SetOutfitChangedFlag(bool changed)
    {
        Debug.Log("SetOutfitChangedFlag: " + changed);
        hasChangedOutfitToday = changed;
    }

    public static void SetCharacterName(string name)
    {
        characterName = name;
    }

    public static void ModifyBankBalance(float delta)
    {
        bankBalance += delta;
    }

    public static float GetBankBalance()
    {
        return (float)System.Math.Round(bankBalance, 2);
    }
}

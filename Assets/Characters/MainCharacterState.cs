using UnityEngine;

[CreateAssetMenu(fileName = "MainCharacterState", menuName = "Custom/MainCharacterState")]
public class MainCharacterState : ScriptableObject
{
    // We store MC-specific info here like: 
    // money, unlocked clothes/furniture/instruments, inventory, etc
    private static string characterName;
    private static double bankBalance;
    private static bool hasChangedOutfitToday;

    public static void Save()
    {
        ES3.Save("MC_Name", characterName);
        ES3.Save("MC_Money", bankBalance);
        ES3.Save("MC_Flag_HasChangedOutfitToday", hasChangedOutfitToday);
    }

    public static void Load()
    {
        characterName = ES3.Load<string>("MC_Name");
        bankBalance = ES3.Load<double>("MC_Money", 100);
        hasChangedOutfitToday = ES3.Load<bool>("MC_Flag_HasChangedOutfitToday", false);
    }

    public static string GetCharacterName()
    {
        return characterName;
    }

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

    public static void ModifyBankBalance(double delta)
    {
        bankBalance += delta;
    }

    public static double GetBankBalance()
    {
        return System.Math.Round(bankBalance, 2);
    }
}
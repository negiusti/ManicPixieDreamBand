using System;
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
        characterName = ES3.Load<string>("MC_Name", defaultValue:"");
        bankBalance = ES3.Load<double>("MC_Money", 100d);
        hasChangedOutfitToday = ES3.Load<bool>("MC_Flag_HasChangedOutfitToday", false);
    }

    public static string GetCharacterName()
    {
        return characterName;
    }

    public static bool HasChangedOutfitToday()
    {
        //Debug.Log("HasChangedOutfitToday: " + hasChangedOutfitToday);
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
        Phone.Instance.NotificationMessage((delta >= 0 ? "+" : "-") + "$" + Math.Abs(delta) + " in bank account");
    }

    public static void SetBankBalance(double value)
    {
        bankBalance = value;
    }

    public static double CurrentBankBalance()
    {
        return Math.Round(bankBalance, 2);
    }
}

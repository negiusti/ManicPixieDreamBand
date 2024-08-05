using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainCharacterState", menuName = "Custom/MainCharacterState")]
public class MainCharacterState : ScriptableObject
{
    // We store MC-specific info here like: 
    // money, unlocked clothes/furniture/instruments, inventory, etc
    private static string characterName;
    private static double bankBalance;
    private static bool hasChangedOutfitToday;
    private static List<string> unlockedPhotos;

    public static void Save()
    {
        ES3.Save("MC_Name", characterName);
        ES3.Save("MC_Money", bankBalance);
        ES3.Save("MC_Flag_HasChangedOutfitToday", hasChangedOutfitToday);
        ES3.Save("Photos", unlockedPhotos);
    }

    public static void UnlockPhoto(string photoName)
    {
        unlockedPhotos.Insert(0, photoName);
        Phone.Instance.SendNotificationTo("Photos");
    }

    public static List<string> UnlockedPhotos()
    {
        if (unlockedPhotos == null)
            unlockedPhotos =  ES3.Load("Photos", new List<string>() { "Boxes", "PizzaRat" });
        return unlockedPhotos;
    }

    public static void Load()
    {
        characterName = ES3.Load<string>("MC_Name", defaultValue:"");
        bankBalance = ES3.Load<double>("MC_Money", 100d);
        hasChangedOutfitToday = ES3.Load<bool>("MC_Flag_HasChangedOutfitToday", false);
        unlockedPhotos = ES3.Load("Photos", new List<string>() { "Boxes", "PizzaRat" });
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

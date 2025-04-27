using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MainCharacterState", menuName = "Custom/MainCharacterState")]
public class MainCharacterState : ScriptableObject
{
    private static string characterName;
    private static double bankBalance;
    private static List<string> unlockedPhotos;
    private static Dictionary<string, bool> flags;
    public static readonly string RollerskateCategory = "Rollerskate_Icons";
    public static readonly string SkateboardCategory = "Skateboard";

    public static void Save()
    {
        ES3.Save("MC_Name", characterName);
        ES3.Save("MC_Money", bankBalance);
        ES3.Save("Flags", flags);
        ES3.Save("Photos", unlockedPhotos);
    }
    
    public static void SetFlag(string flag, bool val)
    {
        Debug.Log("Set flag: " + flag + val);
        if (flags == null)
            flags = ES3.Load("Flags", new Dictionary<string, bool>());
        if (flags == null)
            flags = new Dictionary<string, bool>();
        flags[flag] = val;
    }

    public static void SetRollerskates(string label)
    {
        ES3.Save(RollerskateCategory, label);
    }

    public static void SetSkateboard(string label)
    {
        ES3.Save(SkateboardCategory, label);
    }

    public static string GetRollerSkateLabel()
    {
        return ES3.Load(RollerskateCategory, defaultValue: InventoryManager.defaultPurchaseables.data.First(p => p.category == "Rollerskate_Icons").items.First());
    }

    public static string GetSkateboardLabel()
    {
        return ES3.Load(SkateboardCategory, defaultValue: InventoryManager.defaultPurchaseables.data.First(p => p.category == "Skateboard").items.First());
    }

    public static void SetFlagPrefix(string flagPrefix, bool val)
    {
        foreach(string key in flags.Keys.ToList())
        {
            if (key.StartsWith(flagPrefix))
                flags[key] = val;
        }
    }

    public static bool CheckFlag(string flag)
    {
        if (flags == null)
            flags = ES3.Load("Flags", new Dictionary<string, bool>());
        if (flags == null)
            flags = new Dictionary<string, bool>();
        Debug.Log("Check flag: " + flag + (flags.ContainsKey(flag) ? flags[flag] : "DNE!!"));
        return flags.GetValueOrDefault(flag, false);
    }

    public static void UnlockPhoto(string photoName)
    {
        if (unlockedPhotos.Contains(photoName))
            return;
        unlockedPhotos.Insert(0, photoName);
        Phone.Instance.SendNotificationTo("Photos");

        if (photoName == "_Band")
            Tutorial.joinedTheBand = true;
    }

    public static List<string> UnlockedPhotos()
    {
        if (unlockedPhotos == null)
            unlockedPhotos =  ES3.Load("Photos", new List<string>() { "_Party1", "_Party2", "_Boxes" });
        if (unlockedPhotos == null)
            unlockedPhotos = new List<string>() { "_Party1", "_Party2", "_Boxes" };
        return unlockedPhotos;
    }

    public static void Load()
    {
        characterName = ES3.Load<string>("MC_Name", defaultValue:"");
        bankBalance = ES3.Load<double>("MC_Money", 100d);
        unlockedPhotos = ES3.Load("Photos", new List<string>() { "_Party1", "_Party2", "_Boxes" });

        if (unlockedPhotos == null)
            unlockedPhotos = new List<string>() { "_Party1", "_Party2", "_Boxes" };

        flags = ES3.Load("Flags", new Dictionary<string, bool>());
        if (flags == null)
            flags = new Dictionary<string, bool>();
    }

    public static string GetCharacterName()
    {
        return characterName;
    }

    public static bool HasChangedOutfitToday()
    {
        Debug.Log("HasChangedOutfitToday: " + CheckFlag("ChangedOutfitToday"));
        return CheckFlag("ChangedOutfitToday");
    }

    public static void SetOutfitChangedFlag(bool changed)
    {
        Debug.Log("SetOutfitChangedFlag: " + changed);
        SetFlag("ChangedOutfitToday", changed);
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

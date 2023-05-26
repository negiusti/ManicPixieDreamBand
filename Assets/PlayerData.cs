using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    private Dictionary<string, string> categoryToLabelMap;
    private Dictionary<string, int> tagToColorIndexMap;
    private bool isWearingPants;
    private string name;

    public PlayerData(Player player)
    {
        this.categoryToLabelMap = player.CategoryToLabelMap();
        this.tagToColorIndexMap = player.TagToColorIndexMap();
        this.isWearingPants = player.IsWearingPants();
        this.name = player.PlayerName();
    }

    public Dictionary<string, string> CategoryToLabelMap()
    {
        return categoryToLabelMap;
    }

    public Dictionary<string, int> TagToColorIndexMap()
    {
        return tagToColorIndexMap;
    }

    public bool IsWearingPants()
    {
        return this.isWearingPants;
    }

    public String GetName()
    {
        return name;
    }

}

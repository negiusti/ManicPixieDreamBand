using System;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    private Dictionary<string, string> categoryToLabelMap;
    private Dictionary<string, int> tagToColorIndexMap;
    private Dictionary<string, bool> categoryToEnabled;
    private bool isWearingFullFit;
    private string name;

    public CharacterData(Character character)
    {
        this.categoryToLabelMap = character.CategoryToLabelMap();
        this.tagToColorIndexMap = character.CategoryToColorIndexMap();
        this.isWearingFullFit = character.IsWearingFullFit();
        this.categoryToEnabled = character.CategoryToEnabled();
        this.name = character.CharacterName();
    }

    public Dictionary<string, string> CategoryToLabelMap()
    {
        return categoryToLabelMap;
    }

    public Dictionary<string, int> TagToColorIndexMap()
    {
        return tagToColorIndexMap;
    }

    public Dictionary<string, bool> CategoryToEnabled()
    {
        return categoryToEnabled;
    }

    public bool IsWearingFullFit()
    {
        return this.isWearingFullFit;
    }


    public String GetName()
    {
        return name;
    }

}

using System;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    private Dictionary<string, string> categoryToLabelMap;
    private Dictionary<string, int> tagToColorIndexMap;
    private bool isWearingPants;
    private string name;

    public CharacterData(Character character)
    {
        this.categoryToLabelMap = character.CategoryToLabelMap();
        this.tagToColorIndexMap = character.CategoryToColorIndexMap();
        this.isWearingPants = character.IsWearingPants();
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

    public bool IsWearingPants()
    {
        return this.isWearingPants;
    }

    public String GetName()
    {
        return name;
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    [SerializeField] private Dictionary<string, string> categoryToLabelMap;
    [SerializeField] private Dictionary<string, float[]> categoryToColorMap;
    [SerializeField] private Dictionary<string, float> hueShifts;
    [SerializeField] private Dictionary<string, bool> categoryToEnabled;
    [SerializeField] private bool isWearingFullFit;
    [SerializeField] private string name;
    [SerializeField] private bool isMusician;

    public CharacterData(Character character)
    {
        this.categoryToLabelMap = character.CategoryToLabelMap();
        this.categoryToColorMap = character.CategoryToColorMap();
        this.isWearingFullFit = character.IsWearingFullFit();
        this.categoryToEnabled = character.CategoryToEnabled();
        this.hueShifts = character.HueShifts();
        this.name = character.CharacterName();
    }

    public Dictionary<string, string> CategoryToLabelMap()
    {
        return categoryToLabelMap;
    }

    public Dictionary<string, float> HueShifts()
    {
        return hueShifts;
    }

    public Dictionary<string, float[]> CategoryToColorMap()
    {
        return categoryToColorMap;
    }

    public Dictionary<string, bool> CategoryToEnabled()
    {
        return categoryToEnabled;
    }

    public bool IsWearingFullFit()
    {
        return isWearingFullFit;
    }

    public bool IsMusician()
    {
        return isMusician;
    }

    public String GetName()
    {
        return name;
    }

}

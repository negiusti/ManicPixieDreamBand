using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Character : MonoBehaviour
{
    private Dictionary<string, string> categoryToLabelMap;
    private SpriteResolver[] spriteResolvers;
    private SpriteRenderer[] spriteRenderers;
    private Dictionary<string, int> categoryToColorIndexMap;
    private Dictionary<string, bool> categoryToEnabled;
    //private bool isWearingPants; // Crotch is always enabled, determines whether L_Pant and R_Pant are enabled
    //private bool hasSleeves; // Whether this outfit has sleeves
    private bool isWearingFullFit; // Set matching Top, Crotch, and (optional) sleeves, (optional) L_Pant and R_Pant

    //private ColorPicker[] colorPickers;
    private SpriteLibraryAsset libraryAsset;
    private System.Random random;
    private string characterName;

    public bool IsWearingFullFit()
    {
        return this.isWearingFullFit;
    }

    public void SetCharacterName(string name)
    {
        this.characterName = name;
    }

    public string CharacterName()
    {
        return characterName;
    }

    public void SetIsWearingFullFit(bool isWearingFullFit)
    {
        this.isWearingFullFit = isWearingFullFit;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        spriteResolvers = this.GetComponentsInChildren<SpriteResolver>();
        categoryToEnabled = new Dictionary<string, bool>();
        categoryToLabelMap = new Dictionary<string, string>();
        categoryToColorIndexMap = new Dictionary<string, int>();
        //colorPickers = this.GetComponentsInChildren<ColorPicker>();
        
        if (libraryAsset == null)
        {
            libraryAsset = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        }
        this.random = new System.Random();
        this.characterName = gameObject.name;
        LoadCharacter();
        //RandomizeAppearance();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void updateSpriteResolverMap()
    {
        foreach (var targetResolver in spriteResolvers)
        {
            if (targetResolver.GetCategory() != null)
            {
                categoryToLabelMap[targetResolver.GetCategory()] = targetResolver.GetLabel();
                Debug.Log("category: " + targetResolver.GetCategory() + " label: " + targetResolver.GetLabel());
                Debug.Log("category: " + targetResolver.GetCategory() + " label: " + categoryToLabelMap[targetResolver.GetCategory()]);
            }
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            categoryToEnabled[spriteRenderer.gameObject.name] = spriteRenderer.enabled;
        }
    }

    //public void updateSpriteColorMap()
    //{
    //    foreach (var colorPicker in colorPickers)
    //    {
    //        categoryToColorIndexMap[colorPicker.gameObject.name] = colorPicker.GetColor();
    //    }
    //}

    public void SaveCharacter()
    {
        Debug.Log("Saving... " + characterName);
        updateSpriteResolverMap();
        //updateSpriteColorMap();
        SaveSystem.SaveCharacter(this);
    }

    public void LoadCharacter()
    {
        Debug.Log("Loading... " + characterName);
        CharacterData characterData = SaveSystem.LoadCharacter(characterName);
        if (characterData == null)
        {
            // Character does not exist yet...
            return;
        }
        isWearingFullFit = characterData.IsWearingFullFit();
        categoryToLabelMap = characterData.CategoryToLabelMap();
        categoryToColorIndexMap = characterData.TagToColorIndexMap();
        characterName = characterData.GetName();
        categoryToEnabled = characterData.CategoryToEnabled();
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        foreach (var targetResolver in spriteResolvers)
        {
            if (targetResolver.GetCategory() != null && categoryToLabelMap.ContainsKey(targetResolver.GetCategory())) {
                Debug.Log("category: " + targetResolver.GetCategory() + " label: " + targetResolver.GetLabel());
                Debug.Log("category: " + targetResolver.GetCategory() + " label: " + categoryToLabelMap[targetResolver.GetCategory()]);
                //if (!categoryToLabelMap.ContainsKey(targetResolver.GetCategory()))
                //{
                //    categoryToLabelMap[targetResolver.GetCategory()] = targetResolver.GetLabel();
                //}
                targetResolver.SetCategoryAndLabel(targetResolver.GetCategory(), categoryToLabelMap[targetResolver.GetCategory()]);
                targetResolver.ResolveSpriteToSpriteRenderer();
            }
        }

        //foreach (var colorPicker in colorPickers)
        //{
        //    if (categoryToColorIndexMap.ContainsKey(colorPicker.gameObject.name))
        //        colorPicker.SetColor(categoryToColorIndexMap[colorPicker.gameObject.name]);
        //}

        foreach (var spriteRenderer in spriteRenderers)
        {
           spriteRenderer.enabled = categoryToEnabled[spriteRenderer.gameObject.name];
        }
    }

    //public void RandomizeAppearance()
    //{
    //    updateSpriteResolverMap();
    //    updateSpriteColorMap();
    //    RandomizeAppearance(true, false);
    //}

    // Bangs
    // Hair
    // Brows
    // Eyes
    // Nose
    // Lips
    // L_Sleeve
    // R_Sleeve
    // Shirt
    // Bottom
    // L_Shoe
    // R_Shoe
    // L_Bottom
    // R_Bottom
    // Face_Details
    // L_Bottom_U
    // R_Bottom_U
    // Mustache
    // Beard
    // Glasses
    // R_Arm
    // L_Arm
    // R_Leg
    // L_Leg

    //public void RandomizeAppearance(bool femmeOnly, bool mascOnly)
    //{
    //    var keys = categoryToLabelMap.Keys.ToArray<String>();
    //    foreach (var category in keys)
    //    {
    //        var options = libraryAsset.GetCategoryLabelNames(category).ToArray();
    //        if (options.Length == 0)
    //            continue;

    //        var label = options[random.Next(options.Length)];
    //        if (femmeOnly)
    //        {
    //            while (label.EndsWith("M"))
    //            {
    //                label = options[random.Next(options.Length)];
    //            }
    //        }
    //        if (mascOnly)
    //        {
    //            while (label.EndsWith("F"))
    //            {
    //                label = options[random.Next(options.Length)];
    //            }
    //        }
    //        if (category.StartsWith("L_") || category.StartsWith("R_"))
    //        {
    //            var subCategory = category.Split("_")[1];
    //            var leftRight = category.Split("_")[0] == "R" ? "L" : "R";
    //            categoryToLabelMap[leftRight + "_" + subCategory] = label;
    //        }
    //        categoryToLabelMap[category] = label;
    //        if ((category == "Mustache" || category == "Beard") && femmeOnly)
    //        {
    //            categoryToLabelMap[category] = options[0];
    //        }
    //    }
    //}

    //private void RandomizeColors()
    //{
    //    var color1 = random.Next(colorPickers[1].NumColors());
    //    var color2 = random.Next(colorPickers[1].NumColors());
    //    var hairCategories = new HashSet<String> { "Hair", "Bangs", "Brow", "Mustache", "Beard"};

    //    foreach (var colorPicker in colorPickers)
    //    {
    //        if (colorPicker.tag == "BodyPart")
    //            continue;
    //        categoryToColorIndexMap[colorPicker.gameObject.name] = random.Next(2) == 1 ? color2 : color1;
    //        if (colorPicker.gameObject.name.StartsWith("L_") || colorPicker.gameObject.name.StartsWith("R_"))
    //        {
    //            var subCategory = colorPicker.gameObject.name.Split("_")[1];
    //            var leftRight = colorPicker.gameObject.name.Split("_")[0] == "R" ? "L" : "R";
    //            categoryToColorIndexMap[leftRight + "_" + subCategory] = categoryToColorIndexMap[colorPicker.gameObject.name];
    //        }
    //        if (hairCategories.Contains(colorPicker.gameObject.name))
    //        {
    //            //var hairColor = random.Next(colorPicker.NumColors());
    //            categoryToColorIndexMap["Hair"] = categoryToColorIndexMap[colorPicker.gameObject.name];
    //            categoryToColorIndexMap["Bangs"] = categoryToColorIndexMap["Hair"] = categoryToColorIndexMap[colorPicker.gameObject.name];
    //            categoryToColorIndexMap["Brow"] = categoryToColorIndexMap["Hair"] = categoryToColorIndexMap[colorPicker.gameObject.name];
    //            categoryToColorIndexMap["Mustache"] = categoryToColorIndexMap[colorPicker.gameObject.name];
    //            categoryToColorIndexMap["Beard"] = categoryToColorIndexMap[colorPicker.gameObject.name];
    //        }
    //    }
    //}

    //public void RandomizeCharacter()
    //{
    //    RandomizeAppearance();
    //    RandomizeColors();
    //    UpdateAppearance();
    //}

    public Dictionary<string, string> CategoryToLabelMap()
    {
        return categoryToLabelMap;
    }

    public Dictionary<string, int> CategoryToColorIndexMap()
    {
        return categoryToColorIndexMap;
    }

    public Dictionary<string, bool> CategoryToEnabled()
    {
        return categoryToEnabled;
    }

    public SpriteLibraryAsset LibraryAsset()
    {
        return libraryAsset;
    }
}


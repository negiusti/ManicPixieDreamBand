using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour
{
    private Dictionary<string, string> categoryToLabelMap;
    private SpriteResolver[] spriteResolvers;
    private SpriteRenderer[] spriteRenderers;
    private Dictionary<string, int> tagToColorIndexMap;
    private bool isWearingPants;
    private ColorPicker[] colorPickers;
    private SpriteLibraryAsset libraryAsset;
    private System.Random random;
    private string playerName;

    public bool IsWearingPants()
    {
        return this.isWearingPants;
    }

    public void SetPlayerName(string name)
    {
        this.playerName = name;
    }

    public string PlayerName()
    {
        return playerName;
    }

    public void SetIsWearingPants(bool isWearingPants)
    {
        this.isWearingPants = isWearingPants;
    }

    // Start is called before the first frame update
    void Start()
    {
        isWearingPants = true;
        spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        spriteResolvers = this.GetComponentsInChildren<SpriteResolver>();
        categoryToLabelMap = new Dictionary<string, string>();
        tagToColorIndexMap = new Dictionary<string, int>();
        colorPickers = this.GetComponentsInChildren<ColorPicker>();
        
        if (libraryAsset == null)
        {
            libraryAsset = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        }
        this.random = new System.Random();
        this.playerName = transform.parent.name;
        //LoadPlayer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void updateSpriteResolverMap()
    {
        foreach (var targetResolver in spriteResolvers)
        {
            categoryToLabelMap[targetResolver.GetCategory()] = targetResolver.GetLabel();
        }
    }

    public void updateSpriteColorMap()
    {
        foreach (var colorPicker in colorPickers)
        {
            tagToColorIndexMap[colorPicker.tag] = colorPicker.GetColor();
        }
    }

    public void SavePlayer()
    {
        updateSpriteColorMap();
        updateSpriteResolverMap();
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        Debug.Log("Loading... " + playerName);
        PlayerData playerData = SaveSystem.LoadPlayer(playerName);
        isWearingPants = playerData.IsWearingPants();
        categoryToLabelMap = playerData.CategoryToLabelMap();
        tagToColorIndexMap = playerData.TagToColorIndexMap();
        playerName = playerData.GetName();
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        foreach (var targetResolver in spriteResolvers)
        {
            if (targetResolver.GetCategory() != null && categoryToLabelMap.ContainsKey(targetResolver.GetCategory()) &&
                libraryAsset.GetCategoryLabelNames(targetResolver.GetCategory()).Contains(categoryToLabelMap[targetResolver.GetCategory()]))
            {
                Debug.Log(targetResolver.GetCategory() + " " + categoryToLabelMap[targetResolver.GetCategory()]);
                targetResolver.SetCategoryAndLabel(targetResolver.GetCategory(), categoryToLabelMap[targetResolver.GetCategory()]);
                targetResolver.ResolveSpriteToSpriteRenderer();
            }
        }

        foreach (var colorPicker in colorPickers)
        {
            if (tagToColorIndexMap.ContainsKey(colorPicker.tag))
                colorPicker.SetColor(tagToColorIndexMap[colorPicker.tag]);
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.tag == "L_Pants" || spriteRenderer.tag == "R_Pants")
                spriteRenderer.enabled = isWearingPants;
            if (spriteRenderer.tag == "Skirt")
                spriteRenderer.enabled = !isWearingPants;
        }
    }

    public void RandomizeAppearance()
    {
        updateSpriteResolverMap();
        updateSpriteColorMap();
        RandomizeAppearance(true, false);
    }

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

    public void RandomizeAppearance(bool femmeOnly, bool mascOnly)
    {
        var keys = categoryToLabelMap.Keys.ToArray<String>();
        foreach (var category in keys)
        {
            var options = libraryAsset.GetCategoryLabelNames(category).ToArray();
            if (options.Length == 0)
                continue;
            var label = options[random.Next(options.Length)];
            if (femmeOnly)
            {
                while (label.EndsWith("M"))
                {
                    label = options[random.Next(options.Length)];
                }
            }
            if (mascOnly)
            {
                while (label.EndsWith("F"))
                {
                    label = options[random.Next(options.Length)];
                }
            }
            categoryToLabelMap[category] = label;
            if (category.StartsWith("L_") || category.StartsWith("R_"))
            {
                var subCategory = category.Split("_")[1];
                var leftRight = category.Split("_")[0] == "R" ? "L" : "R";
                categoryToLabelMap[leftRight + "_" + subCategory] = label;
            }
            if ((category == "Mustache" || category == "Beard") && femmeOnly)
            {
                categoryToLabelMap[category] = options[0];
            }
        }
    }

    private void RandomizeColors()
    {
        var color1 = random.Next(colorPickers[1].NumColors());
        var color2 = random.Next(colorPickers[1].NumColors());
        var hairCategories = new HashSet<String> { "Hair", "Bangs", "Brow", "Mustache", "Beard"};

        foreach (var colorPicker in colorPickers)
        {
            if (colorPicker.tag == "BodyPart")
                continue;
            tagToColorIndexMap[colorPicker.tag] = random.Next(2) == 1 ? color2 : color1;
            if (colorPicker.tag.StartsWith("L_") || colorPicker.tag.StartsWith("R_"))
            {
                var subCategory = colorPicker.tag.Split("_")[1];
                var leftRight = colorPicker.tag.Split("_")[0] == "R" ? "L" : "R";
                tagToColorIndexMap[leftRight + "_" + subCategory] = tagToColorIndexMap[colorPicker.tag];
            }
            if (hairCategories.Contains(colorPicker.tag))
            {
                //var hairColor = random.Next(colorPicker.NumColors());
                tagToColorIndexMap["Hair"] = tagToColorIndexMap[colorPicker.tag];
                tagToColorIndexMap["Bangs"] = tagToColorIndexMap["Hair"] = tagToColorIndexMap[colorPicker.tag];
                tagToColorIndexMap["Brow"] = tagToColorIndexMap["Hair"] = tagToColorIndexMap[colorPicker.tag];
                tagToColorIndexMap["Mustache"] = tagToColorIndexMap[colorPicker.tag];
                tagToColorIndexMap["Beard"] = tagToColorIndexMap[colorPicker.tag];
            }
        }
    }

    public void RandomizePlayer()
    {
        RandomizeAppearance();
        RandomizeColors();
        UpdateAppearance();
    }

    public Dictionary<string, string> CategoryToLabelMap()
    {
        return categoryToLabelMap;
    }

    public Dictionary<string, int> TagToColorIndexMap()
    {
        return tagToColorIndexMap;
    }

    public SpriteLibraryAsset LibraryAsset()
    {
        return libraryAsset;
    }
}


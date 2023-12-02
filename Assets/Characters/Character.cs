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
    private Dictionary<string, Color> categoryToColorMap;
    private Dictionary<string, bool> categoryToEnabled;
    //private bool isWearingPants; // Crotch is always enabled, determines whether L_Pant and R_Pant are enabled
    //private bool hasSleeves; // Whether this outfit has sleeves
    private bool isWearingFullFit; // Set matching Top, Crotch, and (optional) sleeves, (optional) L_Pant and R_Pant

    //private ColorPicker[] colorPickers;
    private SpriteLibraryAsset libraryAsset;
    private System.Random random;
    private string characterName;
    private Animator animator;

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
        categoryToColorMap = new Dictionary<string, Color>();
        //colorPickers = this.GetComponentsInChildren<ColorPicker>();
        
        if (libraryAsset == null)
        {
            libraryAsset = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        }
        this.random = new System.Random();
        this.characterName = gameObject.name;
        LoadCharacter();
        animator = this.GetComponent<Animator>();
        animator.Play("BaseCharacter_Idle");
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

    private void updateSpriteColorMap()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            categoryToColorMap[spriteRenderer.gameObject.name] = spriteRenderer.color;
        }
    }

    public void SaveCharacter()
    {
        Debug.Log("Saving... " + characterName);
        updateSpriteResolverMap();
        updateSpriteColorMap();
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
        categoryToColorMap = characterData.CategoryToColorMap().ToDictionary(pair => pair.Key, pair => new Color(pair.Value[0], pair.Value[1], pair.Value[2], pair.Value[3]));
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
                targetResolver.SetCategoryAndLabel(targetResolver.GetCategory(), categoryToLabelMap[targetResolver.GetCategory()]);
                targetResolver.ResolveSpriteToSpriteRenderer();
            }
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = categoryToColorMap[spriteRenderer.gameObject.name];
            spriteRenderer.enabled = categoryToEnabled[spriteRenderer.gameObject.name];
        }
    }

    public Dictionary<string, string> CategoryToLabelMap()
    {
        return categoryToLabelMap;
    }

    public Dictionary<string, float[]> CategoryToColorMap()
    {
        return categoryToColorMap.ToDictionary(pair => pair.Key, pair => new float[] { pair.Value.r, pair.Value.g, pair.Value.b, pair.Value.a });
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


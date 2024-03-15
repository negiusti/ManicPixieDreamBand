using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.Rendering;

public class Character : MonoBehaviour
{
    private Dictionary<string, string> categoryToLabelMap;
    private Dictionary<string, SpriteResolver> categoryToResolver = new Dictionary<string, SpriteResolver>();
    private Dictionary<string, SpriteRenderer> categoryToRenderer = new Dictionary<string, SpriteRenderer>();
    private SpriteResolver[] spriteResolvers;
    private SpriteRenderer[] spriteRenderers;
    private Dictionary<string, Color> categoryToColorMap;
    private Dictionary<string, bool> categoryToEnabled;
    //private SpriteResolver instResolver;
    //private SpriteRenderer instRenderer;
    private SortingGroup sortingGroup;
    private bool isWearingFullFit; // Set matching Top, Crotch, and (optional) sleeves, (optional) L_Pant and R_Pant
    private bool isMC;
    //private SpriteResolver mouthResolver;
    //private SpriteRenderer mouthRenderer;
    //private SpriteResolver eyesResolver;
    //private SpriteRenderer eyesRenderer;
    //private SpriteRenderer eyeshadowRenderer;
    //private SpriteRenderer eyebrowsRenderer;
    private Animator animator;
    private SpriteLibraryAsset libraryAsset;
    private string characterName;

    public SortingGroup GetCurrentLayer()
    {
        return sortingGroup;
    }

    public bool IsWearingFullFit()
    {
        return isWearingFullFit;
    }

    public void SetCharacterName(string name)
    {
        characterName = name;
    }

    public string CharacterName()
    {
        return characterName;
    }

    public void SetIsWearingFullFit(bool isWearingFullFit)
    {
        this.isWearingFullFit = isWearingFullFit;
    }

    public bool isMainCharacter()
    {
        return isMC;
    }

    // Start is called before the first frame update
    void Start()
    {
        isMC = gameObject.name == "MainCharacter";
        spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        spriteResolvers = this.GetComponentsInChildren<SpriteResolver>();
        categoryToEnabled = new Dictionary<string, bool>();
        categoryToLabelMap = new Dictionary<string, string>();
        categoryToColorMap = new Dictionary<string, Color>();
        sortingGroup = this.GetComponent<SortingGroup>();
        animator = this.GetComponent<Animator>();

        if (libraryAsset == null)
        {
            libraryAsset = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        }
        string originalName = gameObject.name;

        // Check if the name ends with "(Clone)"
        if (originalName.EndsWith("(Clone)"))
        {
            // Remove "(Clone)" from the end of the name
            string newName = originalName.Substring(0, originalName.Length - "(Clone)".Length);

            // Set the new name to the game object
            gameObject.name = newName;

            // Optionally, you can print the new name
            Debug.Log($"Changed name from '{originalName}' to '{newName}'");
        }

        this.characterName = gameObject.name;

        foreach (SpriteResolver resolver in spriteResolvers)
        {
            categoryToResolver[resolver.gameObject.name] = resolver;
        }
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            categoryToRenderer[renderer.gameObject.name] = renderer;
        }
        LoadCharacter();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetInstrumentSprite(string s)
    {
        categoryToRenderer["Instrument"].enabled = true;
        categoryToResolver["Instrument"].SetCategoryAndLabel("Instrument", s);
        categoryToResolver["Instrument"].ResolveSpriteToSpriteRenderer();
    }

    public void HideInstrumentSprite()
    {
        categoryToResolver["Instrument"].SetCategoryAndLabel("Instrument", "None");
        categoryToResolver["Instrument"].ResolveSpriteToSpriteRenderer();
    }

    private void updateSpriteResolverMap()
    {
        foreach (var targetResolver in spriteResolvers)
        {
            //if (targetResolver.GetLabel() == null)
            //{
            //    targetResolver.SetCategoryAndLabel
            //}
            if (targetResolver.GetLabel() == null)
            {
                Debug.LogError(gameObject.name + "'s targetresolver for " + targetResolver.gameObject.name + " is null");
                continue;
            }
            if (targetResolver.GetLabel().StartsWith("E_")) // don't save emotes
                continue;
            categoryToLabelMap[GetSRCategory(targetResolver)] = targetResolver.GetLabel();
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            categoryToEnabled[spriteRenderer.gameObject.name] = spriteRenderer.enabled || spriteRenderer.gameObject.name == "Eyeshadow" || spriteRenderer.gameObject.name == "Eyebrows";
        }
    }

    private void updateSpriteColorMap()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            // don't save emotes
            if (spriteRenderer.gameObject.name == "Mouth" && categoryToResolver["Mouth"] != null && categoryToResolver["Mouth"].GetLabel().StartsWith("E_"))
                continue;
            if (spriteRenderer.gameObject.name == "Eyes" && categoryToResolver["Eyes"] != null && categoryToResolver["Eyes"].GetLabel().StartsWith("E_"))
                continue;

            categoryToColorMap[spriteRenderer.gameObject.name] = spriteRenderer.color;
        }
    }

    private string GetSRCategory(SpriteResolver sr)
    {
        return sr.GetCategory() ?? sr.gameObject.name;
    }

    public void SaveCharacter()
    {
        Debug.Log("Saving... " + characterName);
        updateSpriteResolverMap();
        updateSpriteColorMap();
        ES3.Save(characterName, new CharacterData(this));
    }

    public void LoadCharacter()
    {
        Debug.Log("Loading... " + characterName);
        if (!ES3.KeyExists(characterName))
        {
            // Character does not exist yet...
            updateSpriteResolverMap();
            updateSpriteColorMap();
            return;
        }
        CharacterData characterData = (CharacterData)ES3.Load(characterName);
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
            string category = GetSRCategory(targetResolver);
            if (categoryToLabelMap.ContainsKey(category))
            {
                if (categoryToLabelMap[category] == null && targetResolver.GetLabel() != null)
                {
                    categoryToLabelMap[category] = targetResolver.GetLabel();
                }
                targetResolver.SetCategoryAndLabel(category, categoryToLabelMap[category]);
                targetResolver.ResolveSpriteToSpriteRenderer();
            }
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = categoryToColorMap.TryGetValue(spriteRenderer.gameObject.name, out Color value) ? value : Color.white;
            spriteRenderer.enabled = categoryToEnabled.GetValueOrDefault(spriteRenderer.gameObject.name);
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

    public void MoveToRenderLayer(string layer, int idx)
    {
        if (sortingGroup == null)
        {
            sortingGroup = this.GetComponent<SortingGroup>();
        }
        sortingGroup.sortingLayerName = layer;
        sortingGroup.sortingOrder = idx;
    }

    public void EmoteMouth(string emotion)
    {
        string label = (emotion == "default") ? categoryToLabelMap["Mouth"] : "E_" + emotion;
        categoryToResolver["Mouth"].SetCategoryAndLabel("Mouth", label);
        Debug.Log("Setting Mouth resolver to: " + categoryToResolver["Mouth"].GetLabel());
        categoryToResolver["Mouth"].ResolveSpriteToSpriteRenderer();
        Color color = (emotion == "default") ? categoryToColorMap["Mouth"] : Color.white;
        categoryToRenderer["Mouth"].color = color;
    }

    public void FacePop()
    {
        animator.Play("BaseCharacter_FacePop", 1, 0f);
    }

    private void SetShoes(string label)
    {
        categoryToResolver["R_Shoe"].SetCategoryAndLabel("R_Shoe", label);
        categoryToResolver["R_Shoe"].ResolveSpriteToSpriteRenderer();
        categoryToResolver["L_Shoe"].SetCategoryAndLabel("L_Shoe", label);
        categoryToResolver["L_Shoe"].ResolveSpriteToSpriteRenderer();
    }

    public void EmoteEyes(string emotion)
    {
        string label = (emotion == "default") ? categoryToLabelMap["Eyes"] : "E_" + emotion;
        categoryToResolver["Eyes"].SetCategoryAndLabel("Eyes", label);
        Debug.Log("Setting Eyes resolver to: " + categoryToResolver["Eyes"].GetLabel());
        categoryToResolver["Eyes"].ResolveSpriteToSpriteRenderer();
        Color color = (emotion == "default") ? categoryToColorMap["Eyes"] : Color.white;
        categoryToRenderer["Eyes"].color = color;
        categoryToRenderer["Eyeshadow"].enabled = (emotion == "default");
        categoryToRenderer["Eyebrows"].enabled = (emotion == "default");
    }

    public void RollerskatesOnOff(bool isRollerskating)
    {
        string label = isRollerskating ? "E_YellowSkate" : categoryToLabelMap["L_Shoe"];
        SetShoes(label);
    }
}


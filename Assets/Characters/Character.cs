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
    private Dictionary<string, float> hueShifts;
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
    private bool isRollerskating;
    private static HashSet<string> shirtsToFlip = new HashSet<string> { "Daisy Dukes Shirt", "Punk Juice Shirt"};
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

    public void FaceRight()
    {
        Quaternion currentRotation = transform.rotation;
        currentRotation.eulerAngles = new Vector3(0f, 180f, 0f);
        transform.rotation = currentRotation;
    }

    private bool isFacingRight()
    {
        return transform.rotation.eulerAngles.y > 1;
    }

    private bool isFacingLeft()
    {
        return transform.rotation.eulerAngles.y < 179;
    }

    public void FaceLeft()
    {
        Quaternion currentRotation = transform.rotation;
        currentRotation.eulerAngles = new Vector3(0f, 0f, 0f);
        transform.rotation = currentRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        isMC = gameObject.name == "MainCharacter";
        spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        spriteResolvers = this.GetComponentsInChildren<SpriteResolver>(includeInactive: true);
        categoryToEnabled = new Dictionary<string, bool>();
        categoryToLabelMap = new Dictionary<string, string>();
        categoryToColorMap = new Dictionary<string, Color>();
        hueShifts = new Dictionary<string, float>();
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
            Debug.Log("renaming: " + originalName);
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

    public void Teleport(float x, float y, string layer, int idx)
    {
        gameObject.transform.position = new Vector3(x, y, 3);
        MoveToRenderLayer(layer, idx);
    }

    private void OnDisable()
    {
        if (gameObject.layer == LayerMask.NameToLayer("LoadingScreen") || gameObject.layer == LayerMask.NameToLayer("MiniGame"))
            return;
        SaveCharacter();
    }

    private void OnEnable()
    {
        if (categoryToColorMap == null)
            Start();
        LoadCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        //// if top is one of the band shirts, then flip it if necessary
        //if (shirtsToFlip.Contains(categoryToResolver["Top"].GetLabel()))
        //{
        //    if (isFacingLeft() && categoryToRenderer["Top"].flipX)
        //    {
        //        // swap sprite in resolver
        //    }
        //    else if (isFacingRight() && !categoryToRenderer["Top"].flipX)
        //    {
        //        // swap sprite in resolver
        //    }
        //}
    }

    public void SetInstrumentSprite(string s)
    {
        categoryToRenderer["Instrument"].enabled = true;
        categoryToResolver["Instrument"].SetCategoryAndLabel("Instrument", s);
        categoryToResolver["Instrument"].ResolveSpriteToSpriteRenderer();

        if (s.Contains("Drum"))
        {
            categoryToRenderer["R_Holding"].enabled = true;
            categoryToResolver["R_Holding"].SetCategoryAndLabel("R_Holding", "Drumstick");
            categoryToResolver["R_Holding"].ResolveSpriteToSpriteRenderer();

            categoryToRenderer["L_Holding"].enabled = true;
            categoryToResolver["L_Holding"].SetCategoryAndLabel("L_Holding", "Drumstick");
            categoryToResolver["L_Holding"].ResolveSpriteToSpriteRenderer();
        }
    }

    public void SetHoldingSprite(string s)
    {
        categoryToRenderer["R_Holding"].enabled = true;
        categoryToResolver["R_Holding"].SetCategoryAndLabel("R_Holding", s);
        categoryToResolver["R_Holding"].ResolveSpriteToSpriteRenderer();
    }

    public void HideInstrumentSprite()
    {
        Debug.Log("Hide instrujment sprite " + gameObject.name);
        categoryToResolver["Instrument"].SetCategoryAndLabel("Instrument", "None");
        categoryToResolver["Instrument"].ResolveSpriteToSpriteRenderer();

        // Stupid unity: https://discussions.unity.com/t/spriteresolver-set-label-issue/923876/3
        categoryToResolver["R_Holding"].gameObject.SetActive(false);
        categoryToResolver["R_Holding"].SetCategoryAndLabel("R_Holding", "None");
        categoryToResolver["R_Holding"].gameObject.SetActive(true);
        categoryToResolver["R_Holding"].ResolveSpriteToSpriteRenderer();

        categoryToResolver["L_Holding"].gameObject.SetActive(false);
        categoryToResolver["L_Holding"].SetCategoryAndLabel("L_Holding", "None");
        categoryToResolver["L_Holding"].gameObject.SetActive(true);
        categoryToResolver["L_Holding"].ResolveSpriteToSpriteRenderer();
    }

    private void updateSpriteResolverMap()
    {
        foreach (var targetResolver in spriteResolvers)
        {
            if (targetResolver.GetLabel() == null)
            {
                Debug.LogError(gameObject.name + "'s targetresolver for " + targetResolver.gameObject.name + " is null");
                continue;
            }
            // don't save emotes
            if (targetResolver.GetLabel().StartsWith("E_") ||
                targetResolver.GetLabel().ToLower().Contains("roller") ||
                targetResolver.GetLabel().ToLower().Contains("inline") ||
                targetResolver.GetCategory() == "Instrument" ||
                targetResolver.GetCategory().Contains("Holding"))
                continue;
            categoryToLabelMap[GetSRCategory(targetResolver)] = targetResolver.GetLabel();
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            // Skip the stupid hint for starting convos
            categoryToEnabled[spriteRenderer.gameObject.name] = spriteRenderer.enabled
                || spriteRenderer.gameObject.name == "Eyeshadow"
                || spriteRenderer.gameObject.name == "Eyebrows"
                || spriteRenderer.gameObject.name.Contains("Shoe");
            if (spriteRenderer.gameObject.name == "Sk8board")
                categoryToEnabled[spriteRenderer.gameObject.name] = false;
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
            if (spriteRenderer.sharedMaterial.HasFloat("_HsvShift"))
            {
                hueShifts[spriteRenderer.gameObject.name] = spriteRenderer.sharedMaterial.GetFloat("_HsvShift");
            }
            
            categoryToColorMap[spriteRenderer.gameObject.name] = spriteRenderer.color;
        }
    }

    private string GetSRCategory(SpriteResolver sr)
    {
        return sr.GetCategory() ?? sr.gameObject.name;
    }

    public void SaveCharacter()
    {
        if (characterName.StartsWith("MG_") || characterName.StartsWith("_Closeup"))
            return;
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
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.sharedMaterial.HasFloat("_HsvShift"))
                {
                    spriteRenderer.sharedMaterial.SetFloat("_HsvShift", 0);
                }
            }
            updateSpriteResolverMap();
            updateSpriteColorMap();
            SetIsWearingFullFit(false);
            return;
        }
        CharacterData characterData = (CharacterData)ES3.Load(characterName);
        isWearingFullFit = characterData.IsWearingFullFit();
        categoryToLabelMap = characterData.CategoryToLabelMap();
        categoryToColorMap = characterData.CategoryToColorMap().ToDictionary(pair => pair.Key, pair => new Color(pair.Value[0], pair.Value[1], pair.Value[2], pair.Value[3]));
        hueShifts = characterData.HueShifts();
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
                    Debug.Log("categoryToLabelMap[category]" + categoryToLabelMap[category] + " = targetResolver.GetLabel()" + targetResolver.GetLabel());
                    categoryToLabelMap[category] = targetResolver.GetLabel();
                }
                targetResolver.SetCategoryAndLabel(category, categoryToLabelMap[category]);
                targetResolver.ResolveSpriteToSpriteRenderer();
            }
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.GetComponentInParent<TalkToMeHint>() != null)
                continue;
            spriteRenderer.color = categoryToColorMap.TryGetValue(spriteRenderer.gameObject.name, out Color value) ? value : Color.white;
            spriteRenderer.enabled = categoryToEnabled.GetValueOrDefault(spriteRenderer.gameObject.name);
            if (spriteRenderer.sharedMaterial.HasFloat("_HsvShift"))
                spriteRenderer.sharedMaterial.SetFloat("_HsvShift", hueShifts.TryGetValue(spriteRenderer.gameObject.name, out float value2) ? value2 : 0f);
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

    public Dictionary<string, float> HueShifts()
    {
        return hueShifts;
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
        Debug.Log("Move " + gameObject.name + " " + layer + idx, gameObject);
        if (sortingGroup == null)
        {
            sortingGroup = this.GetComponent<SortingGroup>();
        }
        sortingGroup.sortingLayerName = layer;
        sortingGroup.sortingOrder = idx;
    }

    public bool EmoteMouth(string emotion)
    {
        string label = (emotion.ToLower() == "default") ? categoryToLabelMap["Mouth"] : "E_" + emotion;
        bool hasChanged = label != categoryToResolver["Mouth"].GetLabel();
        categoryToResolver["Mouth"].SetCategoryAndLabel("Mouth", label);
        Debug.Log("Setting Mouth resolver to: " + categoryToResolver["Mouth"].GetLabel());
        categoryToResolver["Mouth"].ResolveSpriteToSpriteRenderer();
        Color color = (emotion.ToLower() == "default") ? categoryToColorMap["Mouth"] : Color.white;
        categoryToRenderer["Mouth"].color = color;
        return hasChanged;
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

    public void SetTop(string label)
    {
        categoryToResolver["R_Sleeve"].SetCategoryAndLabel("R_Sleeve", label);
        categoryToResolver["R_Sleeve"].ResolveSpriteToSpriteRenderer();
        categoryToResolver["Top"].SetCategoryAndLabel("Top", label);
        categoryToResolver["Top"].ResolveSpriteToSpriteRenderer();
        categoryToResolver["L_Sleeve"].SetCategoryAndLabel("L_Sleeve", label);
        categoryToResolver["L_Sleeve"].ResolveSpriteToSpriteRenderer();
    }

    public void SetFaceDetail(string label)
    {
        categoryToRenderer["Face_Detail"].enabled = true;
        if (categoryToResolver["Face_Detail"].GetLabel() != label)
            FacePop();
        categoryToResolver["Face_Detail"].SetCategoryAndLabel("Face_Detail", label);
        categoryToResolver["Face_Detail"].ResolveSpriteToSpriteRenderer();
    }

    private void SetSkateboard(string label)
    {
        categoryToResolver["Sk8board"].SetCategoryAndLabel("Skateboard", label);
        categoryToResolver["Sk8board"].ResolveSpriteToSpriteRenderer();
    }

    public bool EmoteEyes(string emotion)
    {
        string label = (emotion.ToLower() == "default") ? categoryToLabelMap["Eyes"] : "E_" + emotion;
        bool hasChanged = label != categoryToResolver["Eyes"].GetLabel();
        categoryToResolver["Eyes"].SetCategoryAndLabel("Eyes", label);
        Debug.Log("Setting Eyes resolver to: " + categoryToResolver["Eyes"].GetLabel());
        categoryToResolver["Eyes"].ResolveSpriteToSpriteRenderer();
        Color color = (emotion.ToLower() == "default") ? categoryToColorMap["Eyes"] : Color.white;
        categoryToRenderer["Eyes"].color = color;
        categoryToRenderer["Eyeshadow"].enabled = (emotion.ToLower() == "default");
        categoryToRenderer["Eyebrows"].enabled = (emotion.ToLower() == "default");
        categoryToRenderer["Face_Detail"].enabled = (emotion.ToLower() == "default" || characterName == "Rex");
        return hasChanged;
    }

    public void RollerskatesOnOff(bool rollerskating)
    {
        isRollerskating = rollerskating;
        //string label = isRollerskating ? "E_YellowSkate" : categoryToLabelMap["L_Shoe"];
        string label = isRollerskating ? MainCharacterState.GetRollerSkateLabel() : categoryToLabelMap["L_Shoe"];
        SetShoes(label);
    }

    public void UpdateSkates()
    {
        if (isRollerskating)
        {
            string label = MainCharacterState.GetRollerSkateLabel();
            SetShoes(label);
        }
        SetSkateboard(MainCharacterState.GetSkateboardLabel());
    }
}


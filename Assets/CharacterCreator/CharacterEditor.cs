using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterEditor : MonoBehaviour
{
    public bool unlockAllOutfits;
    private static string top = "Top";
    private static string lSleeve = "L_Sleeve";
    private static string rSleeve = "R_Sleeve";
    private static string crotch = "Crotch";
    private static string rPant = "R_Pant";
    private static string lPant = "L_Pant";
    private static string rSock = "R_Sock";
    private static string lSock = "L_Sock";
    private static string rShoe = "R_Shoe";
    private static string lShoe = "L_Shoe";

    private List<SpriteRenderer> skinRenderers;

    private SpriteLibraryAsset spriteLib;
    private Dictionary<string, SpriteResolver> categoryToResolver = new Dictionary<string, SpriteResolver>();
    private Dictionary<string, SpriteRenderer> categoryToRenderer = new Dictionary<string, SpriteRenderer>();
    private Dictionary<string, string[]> categoryToLabels = new Dictionary<string, string[]>();
    private Dictionary<string, int> categoryToLabelIdx = new Dictionary<string, int>();
    private Character character;
    private bool isFullBody;
    private string currentFaceCategory;

    public ColorPalette hairPalette;
    public ColorPalette mouthPalette;
    public ColorPalette shadowPalette;
    public ColorPalette faceDetailPalette;
    private Dictionary<string, ColorPalette> categoryToColorPalette;

    public Icons shirtIcons;
    public Icons bottomsIcons;
    public Icons socksIcons;
    public Icons shoesIcons;
    public Icons FBIcons;

    // Face icons
    public Icons hairIcons;
    public Icons bangsIcons;
    public Icons loTailsIcons;
    public Icons hiTailsIcons;

    public Icons eyebrowsIcons;
    public Icons eyesIcons;
    public Icons glassesIcons;

    public Icons mouthIcons;
    public Icons faceDetailIcons;
    public Icons eyeshadowIcons;

    public Icons earsIcons;
    public Icons earringsIcons;
    public Icons necklaceIcons;

    //private Dictionary<string, Icons> icons = new Dictionary<string, Icons>();
    private Phone phone;
    private GameObject characterGameObject;

    public void UnlockAllOutfits(bool value)
    {
        unlockAllOutfits = value;
        GetAvailableOptions();
    }

    // Start is called before the first frame update
    void Start()
    {
        phone = GameObject.FindFirstObjectByType<Phone>();
        if (phone != null)
            phone.gameObject.SetActive(false);
        skinRenderers = new List<SpriteRenderer>();
        character = GameObject.FindFirstObjectByType<Character>();
        characterGameObject = character.gameObject;
        SpriteResolver[] resolvers = characterGameObject.GetComponentsInChildren<SpriteResolver>(includeInactive: true);
        foreach (SpriteResolver resolver in resolvers)
        {
            categoryToResolver[resolver.gameObject.name] = resolver;
        }
        SpriteRenderer[] renderers = characterGameObject.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        foreach (SpriteRenderer renderer in renderers)
        {
            categoryToRenderer[renderer.gameObject.name] = renderer;
            if (renderer.CompareTag("BodyPart"))
                skinRenderers.Add(renderer);
        }
        SpriteLibrary fuck = characterGameObject.GetComponent<SpriteLibrary>();
        spriteLib = fuck.spriteLibraryAsset;

        categoryToColorPalette = new Dictionary<string, ColorPalette>();
        categoryToColorPalette.Add("Hair", hairPalette);
        categoryToColorPalette.Add("HiTails", hairPalette);
        categoryToColorPalette.Add("LoTails", hairPalette);
        categoryToColorPalette.Add("Bangs", hairPalette);
        categoryToColorPalette.Add("Eyebrows", hairPalette);
        categoryToColorPalette.Add("Face_Detail", faceDetailPalette);
        categoryToColorPalette.Add("Mouth", mouthPalette);
        categoryToColorPalette.Add("Eyeshadow", shadowPalette);

        GetAvailableOptions();
    }

    //private void OnEnable()
    //{
    //    if (phone == null)
    //    {
    //        Start();
    //    }
    //}

    private void UpdateAllIcons () {
        string[] labels;
        foreach (string category in spriteLib.GetCategoryNames())
        {
            if (unlockAllOutfits)
            {
                labels = spriteLib.GetCategoryLabelNames(category)
                    .Where(l => l.StartsWith("E_"))
                    .Where(l => !l.StartsWith("X_") || l.Equals("X_" + character.gameObject.name))
                    .ToArray();
            }
            else
            {
                labels = InventoryManager.GetMCInventory(category).ToArray();
                // if the inventory is empty, just unlock everything
                if (labels.Length == 0)
                    labels = spriteLib.GetCategoryLabelNames(category)
                        .Where(l => !l.StartsWith("E_"))
                        .Where(l => !l.StartsWith("X_") || l.Equals("X_" + character.gameObject.name))
                        .ToArray();
            }

            categoryToLabels[category] = labels;
            categoryToLabelIdx[category] = System.Array.IndexOf(labels, character.CategoryToLabelMap().GetValueOrDefault(category));
            categoryToLabelIdx[category] = categoryToLabelIdx[category] < 0 ? 0 : categoryToLabelIdx[category];
            UpdateIcons(category, labels);
        }
    }

    public void UpdateIcons(string category)
    {
        UpdateIcons(category, categoryToLabels[category]);
    }

   private void GetAvailableOptions()
    {
        //UpdateAllIcons();
        SetCurrentFaceCategory("Hair");
        HideLoTailsAndHairWithHijab();

        if (character.IsWearingFullFit())
            SelectFB();
        else
            SelectTopAndBottom();
    }

    private void SetOutfitChangedFlag(bool changed)
    {
        if (character.isMainCharacter() && changed)
        {
            MainCharacterState.SetOutfitChangedFlag(true);
        }
    }

    private void OnDestroy()
    {
        if (phone != null)
            phone.gameObject.SetActive(true);
    }

    private void UpdateIcons(string category, string[] labels)
    {
        Icons icons = null;
        if (category.Equals(lSock))
        {
            icons = socksIcons;
        }
        else if (category.Equals(lShoe))
        {
            icons = shoesIcons;
        }
        else if (category.Equals("FB_" + top))
        {
            icons = FBIcons;
        }
        else if (category.Equals(top))
        {
            icons = shirtIcons;
        }
        else if (category.Equals(crotch))
        {
            icons = bottomsIcons;
        }
        else if (category.Equals("Eyebrows"))
        {
            icons = eyebrowsIcons;
        }
        else if (category.Equals("Eyes"))
        {
            icons = eyesIcons;
        }
        else if (category.Equals("Glasses"))
        {
            icons = glassesIcons;
        }
        else if (category.Equals("Mouth"))
        {
            icons = mouthIcons;
        }
        else if (category.Equals("Face_Detail"))
        {
            icons = faceDetailIcons;
        }
        else if (category.Equals("Eyeshadow"))
        {
            icons = eyeshadowIcons;
        }
        else if (category.Equals("Ears"))
        {
            icons = earsIcons;
        }
        else if (category.Equals("Earrings"))
        {
            icons = earringsIcons;
        }
        else if (category.Equals("Necklace"))
        {
            icons = necklaceIcons;
        }
        if (icons != null)
            UpdateIcons(icons, categoryToLabelIdx[category], labels);
    }

    private void UpdateIcons(Icons icons, int idx, string[] labels)
    {
        int leftIdx = GetWrapAroundIndex(idx - 1, labels.Length - 1);
        int rightIdx = GetWrapAroundIndex(idx + 1, labels.Length - 1);
        icons.UpdateIcons(labels[leftIdx], labels[idx], labels[rightIdx]);
    }

    public void SetCurrentFaceCategoryColor(Color c)
    {
        categoryToRenderer[currentFaceCategory].color = c;
        // TODO:
        //faceIcons.UpdateIconsColor(c);
    }

    public void SetSkinColor(Color c)
    {
        foreach (SpriteRenderer sr in skinRenderers)
        {
            sr.color = c;
        }
        // TODO:
        //if (currentFaceCategory.Equals("Earrings"))
        //    faceIcons.UpdateIconsColor(c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisableNonFBRenderers()
    {
        categoryToRenderer.GetValueOrDefault(top).enabled = false;
        categoryToRenderer.GetValueOrDefault(crotch).enabled = false;
        categoryToRenderer.GetValueOrDefault(lPant).enabled = false;
        categoryToRenderer.GetValueOrDefault(rPant).enabled = false;
        categoryToRenderer.GetValueOrDefault(lSleeve).enabled = false;
        categoryToRenderer.GetValueOrDefault(rSleeve).enabled = false;
    }

    private void DisableFBRenderers()
    {
        categoryToRenderer.GetValueOrDefault("FB_" + top).enabled = false;
        categoryToRenderer.GetValueOrDefault("FB_" + lPant).enabled = false;
        categoryToRenderer.GetValueOrDefault("FB_" + rPant).enabled = false;
        categoryToRenderer.GetValueOrDefault("FB_" + lSleeve).enabled = false;
        categoryToRenderer.GetValueOrDefault("FB_" + rSleeve).enabled = false;
    }

    public void SelectTopAndBottom()
    {
        isFullBody = false;
        ChangeTop(0);
        ChangeBottom(0);
        DisableFBRenderers();
        character.SetIsWearingFullFit(isFullBody);
    }

    public void SelectFB()
    {
        isFullBody = true;
        ChangeFB(0);
        DisableNonFBRenderers();
        character.SetIsWearingFullFit(isFullBody);
    }

    public void ChangeFB(int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0 || !isFullBody);
        if (!isFullBody)
            SelectFB();
        int idx = categoryToLabelIdx.GetValueOrDefault("FB_" + top) + idxDelta;
        string[] labels = GetUnlockedLabels("FB_" + top);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx["FB_" + top] = idx;
        SetCategory("FB_" + top, label);

        string[] sleeveCategories = GetUnlockedLabels("FB_" + lSleeve);
        if (sleeveCategories.Contains(label))
        {
            SetCategory("FB_" + lSleeve, label);
            SetCategory("FB_" + rSleeve, label);
        }
        else
        {
            categoryToRenderer.GetValueOrDefault("FB_" + lSleeve).enabled = false;
            categoryToRenderer.GetValueOrDefault("FB_" + rSleeve).enabled = false;
        }

        string[] pantCategories = GetUnlockedLabels("FB_" + lPant);
        if (pantCategories.Contains(label))
        {
            SetCategory("FB_" + lPant, label);
            SetCategory("FB_" + rPant, label);
        }
        else
        {
            categoryToRenderer.GetValueOrDefault("FB_" + lPant).enabled = false;
            categoryToRenderer.GetValueOrDefault("FB_" + rPant).enabled = false;
        }
        if (idxDelta != 0)
            UpdateIcons(FBIcons, idx, labels);
    }

    public void ChangeTop(int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0 || isFullBody);
        if (isFullBody)
            SelectTopAndBottom();
        int idx = categoryToLabelIdx.GetValueOrDefault(top) + idxDelta;
        string[] labels = GetUnlockedLabels(top);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        
        string label = labels[idx];
        categoryToLabelIdx[top] = idx;
        SetCategory(top, label);
        SetSleevesIfPresent(label);
        UpdateIcons(shirtIcons, idx, labels);
    }

    private int GetWrapAroundIndex(int idx, int maxIdx)
    {
        if (idx > maxIdx)
            idx = 0;
        else if (idx < 0)
            idx = maxIdx;
        return idx;
    }

    public void ChangeSocks(int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0);
        int idx = categoryToLabelIdx.GetValueOrDefault(lSock) + idxDelta;
        string[] labels = GetUnlockedLabels(lSock);
        idx = GetWrapAroundIndex(idx, labels.Length -1);
        categoryToLabelIdx[lSock] = idx;
        categoryToLabelIdx[rSock] = idx;
        string label = labels[idx];
        SetCategory(lSock, label);
        SetCategory(rSock, label);
        UpdateIcons(socksIcons, idx, labels);
    }

    public void ChangeShoes(int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0);
        int idx = categoryToLabelIdx.GetValueOrDefault(lShoe) + idxDelta;
        string[] labels = GetUnlockedLabels(lShoe);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx[lShoe] = idx;
        categoryToLabelIdx[rShoe] = idx;
        SetCategory(lShoe, label);
        SetCategory(rShoe, label);
        UpdateIcons(shoesIcons, idx, labels);
    }

    public void ChangeBottom(int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0 || isFullBody);
        if (isFullBody)
            SelectTopAndBottom();
        int idx = categoryToLabelIdx.GetValueOrDefault(crotch) + idxDelta;
        string[] labels = GetUnlockedLabels(crotch);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx[crotch] = idx;
        SetCategory(crotch, label);
        SetPantsIfPresent(label);
        UpdateIcons(bottomsIcons, idx, labels);
    }

    private void SetSleevesIfPresent(string label)
    {
        string[] sleeveCategories = GetUnlockedLabels(lSleeve);
        if (sleeveCategories.Contains(label))
        {
            SetCategory(lSleeve, label);
            SetCategory(rSleeve, label);
        }
        else
        {
            categoryToRenderer.GetValueOrDefault(lSleeve).enabled = false;
            categoryToRenderer.GetValueOrDefault(rSleeve).enabled = false;
        }
    }

    private void SetPantsIfPresent(string label)
    {
        string[] pantCategories = GetUnlockedLabels(lPant);
        if (pantCategories.Contains(label))
        {
            SetCategory(lPant, label);
            SetCategory(rPant, label);
        }
        else
        {
            categoryToRenderer.GetValueOrDefault(lPant).enabled = false;
            categoryToRenderer.GetValueOrDefault(rPant).enabled = false;
        }
    }

    private void SetCategory(string category, string label)
    {
        SpriteResolver res = categoryToResolver.GetValueOrDefault(category);
        res.SetCategoryAndLabel(category, label);
        SpriteRenderer ren = categoryToRenderer.GetValueOrDefault(category);
        ren.enabled = true;
    }

    private void HideLoTailsAndHairWithHijab()
    {
        SpriteResolver res = categoryToResolver.GetValueOrDefault("HiTails");
        string label = res.GetLabel();
        if (label.Contains("Hijab"))
        {
            SetCategory("LoTails", "None");
            SetCategory("Hair", "None");
        }
    }

    private void HideEarringsWithoutEars()
    {
        SpriteResolver res = categoryToResolver.GetValueOrDefault("Ears");
        string label = res.GetLabel();
        if (label.Contains("None"))
        {
            SetCategory("Earrings", "None");
        }
    }

    public void ChangeFace(int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0);
        int idx = categoryToLabelIdx.GetValueOrDefault(currentFaceCategory,0) + idxDelta;
        string[] labels = GetUnlockedLabels(currentFaceCategory);

        if (idx > labels.Length - 1)
            idx = 0; 
        else if (idx < 0)
            idx = labels.Length - 1;

        categoryToLabelIdx[currentFaceCategory] = idx;
        string label = labels[idx];
        SetCategory(currentFaceCategory, label);
        HideLoTailsAndHairWithHijab();
        HideEarringsWithoutEars();
        //UpdateFaceIcons(currentFaceCategory, faceIcons, idx, labels);
    }

    public void ChangeFace(string category, int idxDelta)
    {
        SetOutfitChangedFlag(idxDelta != 0);
        int idx = categoryToLabelIdx.GetValueOrDefault(category, 0) + idxDelta;
        string[] labels = GetUnlockedLabels(category);

        if (idx > labels.Length - 1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;

        categoryToLabelIdx[category] = idx;
        string label = labels[idx];
        SetCategory(category, label);
        HideLoTailsAndHairWithHijab();
        HideEarringsWithoutEars();
        // TODO: update icons
        UpdateIcons(category, labels);
        //UpdateFaceIcons(category, faceIcons, idx, labels);
    }

    public void ChangeBangs(int idxDelta)
    {
        ChangeFace("Bangs", idxDelta);
    }

    public void ChangeHair(int idxDelta)
    {
        ChangeFace("Hair", idxDelta);
    }

    public void ChangeHairDetail(int idxDelta)
    {
        ChangeFace("HiTails", idxDelta);
    }

    public void ChangeLowerHair(int idxDelta)
    {
        ChangeFace("LoTails", idxDelta);
    }

    public void ChangeEyes(int idxDelta)
    {
        ChangeFace("Eyes", idxDelta);
    }

    public void ChangeEyebrows(int idxDelta)
    {
        ChangeFace("Eyebrows", idxDelta);
    }

    public void ChangeGlasses(int idxDelta)
    {
        ChangeFace("Glasses", idxDelta);
    }

    public void ChangeMouth(int idxDelta)
    {
        ChangeFace("Mouth", idxDelta);
    }

    public void ChangeFaceDetail(int idxDelta)
    {
        ChangeFace("Face_Detail", idxDelta);
    }

    public void ChangeEyeshadow(int idxDelta)
    {
        ChangeFace("Eyeshadow", idxDelta);
    }

    public void ChangeEars(int idxDelta)
    {
        ChangeFace("Ears", idxDelta);
    }

    public void ChangeEarrings(int idxDelta)
    {
        ChangeFace("Earrings", idxDelta);
    }

    public void ChangeNecklace(int idxDelta)
    {
        ChangeFace("Necklace", idxDelta);
    }

    public void SetCurrentFaceCategory(string category)
    {
        currentFaceCategory = category;
        foreach (ColorPalette cp in categoryToColorPalette.Values)
        {
            cp.gameObject.SetActive(false);    
        }
        if (categoryToColorPalette.ContainsKey(category))
            categoryToColorPalette[category].gameObject.SetActive(true);
        //ChangeFace(0);
    }

    private string[] GetUnlockedLabels(string category)
    {
        return categoryToLabels[category];
        //return categoryToLabels.GetValueOrDefault(category).Where(l => !l.StartsWith("X_") || l.Equals("X_" + gameObject.name)).ToArray();
    }
}

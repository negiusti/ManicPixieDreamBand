using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System;
using PixelCrushers.DialogueSystem;

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
    private string prevEarringsLabel;
    private string prevHairLabel;
    private string prevLoTailsLabel;
    private bool hijabHidingHair;
    private bool earsHidingEarrings;

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

    public Caboodle caboodle;


    public ColorPalettes faceColorPalettes;
    private GameObject characterGameObject;
    private GameObject goToPreviousSceneButton;
    private Coroutine cr;

    public void UnlockAllOutfits(bool value)
    {
        unlockAllOutfits = value;
        GetAvailableOptions();
    }

    public Color GetCategoryColor(string category)
    {
        return categoryToRenderer[category].color;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Phone.Instance != null)
            Phone.Instance.gameObject.SetActive(false);
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
        goToPreviousSceneButton = FindFirstObjectByType<GoToPreviousSceneButton>().gameObject;

        GetAvailableOptions();
        HideLoTailsAndHairWithHijab();
        HideEarringsWithoutEars();

        if (character.IsWearingFullFit())
            SelectFB();
        else
            SelectTopAndBottom();
        cr = StartCoroutine(ComplainAboutBodyPaint());
    }

    private System.Collections.IEnumerator ComplainAboutBodyPaint()
    {
        while (!Tutorial.changedSkin)
        {
            if (!DialogueManager.IsConversationActive && !caboodle.isActiveAndEnabled)
                DialogueManager.Instance.StartConversation("Tutorial/SkinTone" + UnityEngine.Random.Range(0, 3));
            yield return new WaitForSeconds(10);
        }
        yield return null;
    }

    private void Update()
    {
        goToPreviousSceneButton.SetActive(Tutorial.changedSkin);
    }

    private void GetAvailableOptions() {
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
                labels = spriteLib.GetCategoryLabelNames(category).Where(i => InventoryManager.GetMCInventory(category).Contains(i)).ToArray();
                // if the inventory is empty, just unlock everything
                if (labels.Length == 0)
                    labels = spriteLib.GetCategoryLabelNames(category)
                        .Where(l => !l.StartsWith("E_"))
                        .Where(l => !l.StartsWith("X_") || l.Equals("X_" + character.gameObject.name))
                        .ToArray();
            }

            categoryToLabels[category] = labels;
            categoryToLabelIdx[category] = Array.IndexOf(labels, character.CategoryToLabelMap().GetValueOrDefault(category));
            categoryToLabelIdx[category] = categoryToLabelIdx[category] < 0 ? 0 : categoryToLabelIdx[category];
        }
    }

    public void UpdateIcons(string category)
    {
        UpdateIcons(category, categoryToLabels[category]);
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
        if (Phone.Instance != null)
            Phone.Instance.gameObject.SetActive(true);
        if (cr != null)
            StopCoroutine(cr);
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
        else if (category.Equals("Hair"))
        {
            icons = hairIcons;
        }
        else if (category.Equals("Bangs"))
        {
            icons = bangsIcons;
        }
        else if (category.Equals("LoTails"))
        {
            icons = loTailsIcons;
        }
        else if (category.Equals("HiTails"))
        {
            icons = hiTailsIcons;
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
        UpdateIcons(currentFaceCategory);
    }

    public void SetSkinColor(Color c)
    {
        Tutorial.changedSkin = true;
        SetOutfitChangedFlag(true);
        foreach (SpriteRenderer sr in skinRenderers)
        {
            sr.color = c;
        }
        if (earsIcons != null)
        {
            earsIcons.UpdateIconsColor(c);
        }
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
        //if (idxDelta != 0)
        //    UpdateIcons(FBIcons, idx, labels);
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
        //if (idxDelta != 0)
        //    UpdateIcons(shirtIcons, idx, labels);
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
        //UpdateIcons(socksIcons, idx, labels);
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
        //UpdateIcons(shoesIcons, idx, labels);
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
        //if (idxDelta != 0)
        //    UpdateIcons(bottomsIcons, idx, labels);
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
        categoryToLabelIdx[category] = Array.IndexOf(categoryToLabels[category], label);
        UpdateIcons(category);
    }

    private void HideLoTailsAndHairWithHijab()
    {
        SpriteResolver res = categoryToResolver.GetValueOrDefault("HiTails");
        string label = res.GetLabel();
        if (label.Contains("Hijab"))
        {
            if (!hijabHidingHair)
            {
                prevLoTailsLabel = categoryToResolver["LoTails"].GetLabel();
                prevHairLabel = categoryToResolver["Hair"].GetLabel();
            }
            SetCategory("LoTails", "None");
            SetCategory("Hair", "None");
            hijabHidingHair = true;
        } else
        {
            if (hijabHidingHair)
            {
                SetCategory("LoTails", prevLoTailsLabel);
                SetCategory("Hair", prevHairLabel);
            }
            hijabHidingHair = false;
        }
    }

    private void HideEarringsWithoutEars()
    {
        SpriteResolver res = categoryToResolver.GetValueOrDefault("Ears");
        string label = res.GetLabel();
        if (label.Contains("None"))
        {
            if (!earsHidingEarrings)
                prevEarringsLabel= categoryToResolver["Earrings"].GetLabel();
            SetCategory("Earrings", "None");
            earsHidingEarrings = true;
        } else
        {
            if (earsHidingEarrings)
                SetCategory("Earrings", prevEarringsLabel);
            earsHidingEarrings = false;
        }
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

        string prevLabel = labels[categoryToLabelIdx[category]];

        categoryToLabelIdx[category] = idx;
        string label = labels[idx];
        SetCategory(category, label);
        HideLoTailsAndHairWithHijab();
        HideEarringsWithoutEars();
        SetCurrentFaceCategory(category);
        label = labels[categoryToLabelIdx[category]];
        //UpdateIcons(category, labels);

        if (label.Equals("None"))
        {
            faceColorPalettes.Close();
        }
        if (prevLabel.Equals("None") && prevLabel != label)
        {
            faceColorPalettes.SelectColorPalette(category);
        }
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
        Debug.Log("SetCurrentFaceCategory: " + currentFaceCategory + "->" + category);
        if (categoryToLabels[category][categoryToLabelIdx[category]].Equals("None"))
        {
            Debug.Log("Closing palette bc " + category + "label is None");
            faceColorPalettes.Close();
        }
        else
            faceColorPalettes.SelectColorPalette(category);

        if (currentFaceCategory == category)
        {
            return;
        }
        currentFaceCategory = category;
        caboodle.SelectCaboodleSection(category);
    }

    private string[] GetUnlockedLabels(string category)
    {
        return categoryToLabels[category];
        //return categoryToLabels.GetValueOrDefault(category).Where(l => !l.StartsWith("X_") || l.Equals("X_" + gameObject.name)).ToArray();
    }
}

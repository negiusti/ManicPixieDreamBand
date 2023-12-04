using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterEditor : MonoBehaviour
{
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

    // Hair
    public SpriteRenderer bangsSRen;
    public SpriteRenderer hairSRen;
    public SpriteRenderer tailsSRen;

    public SpriteRenderer eyebrowsSRen;
    public SpriteRenderer mouthSRen;
    public SpriteRenderer faceDetailSRen;
    public SpriteRenderer eyesShadowSRen;

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
    public ColorPalette tailsPalette;
    public ColorPalette bangsPalette;
    public ColorPalette browPalette;
    public ColorPalette mouthPalette;
    public ColorPalette shadowPalette;
    public ColorPalette faceDetailPalette;
    private Dictionary<string, ColorPalette> categoryToColorPalette;

    public Icons shirtIcons;
    public Icons bottomsIcons;
    public Icons socksIcons;
    public Icons shoesIcons;
    public Icons FBIcons;

    // Start is called before the first frame update
    void Start()
    {
        skinRenderers = new List<SpriteRenderer>();
        character = this.GetComponent<Character>();
        SpriteResolver[] resolvers = this.GetComponentsInChildren<SpriteResolver>();
        foreach (SpriteResolver resolver in resolvers)
        {
            categoryToResolver[resolver.gameObject.name] = resolver;
        }
        SpriteRenderer[] renderers = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            categoryToRenderer[renderer.gameObject.name] = renderer;
            if (renderer.CompareTag("BodyPart"))
                skinRenderers.Add(renderer);
        }
        string[] labels;
        SpriteLibrary fuck = this.GetComponent<SpriteLibrary>();
        spriteLib = fuck.spriteLibraryAsset;
        foreach (string category in spriteLib.GetCategoryNames())
        {
            labels = spriteLib.GetCategoryLabelNames(category).ToArray();
            categoryToLabels[category] = labels;
            categoryToLabelIdx[category] = System.Array.IndexOf(labels, character.CategoryToLabelMap()[category]);
        }
        
        if (character.IsWearingFullFit())
            SelectFB();
        else
            SelectTopAndBottom();

        categoryToColorPalette = new Dictionary<string, ColorPalette>();
        categoryToColorPalette.Add("Hair", hairPalette);
        categoryToColorPalette.Add("Tails", tailsPalette);
        categoryToColorPalette.Add("Bangs", bangsPalette);
        categoryToColorPalette.Add("FaceDetail", faceDetailPalette);
        categoryToColorPalette.Add("Eyebrows", browPalette);
        categoryToColorPalette.Add("Mouth", mouthPalette);
        categoryToColorPalette.Add("Eyeshadow", shadowPalette);
        Debug.Log("categoryToColorPalette is " + categoryToColorPalette.Keys);
        SetCurrentFaceCategory("Hair");
        HideTailsWithHijab();
    }

    public void SetCurrentFaceCategoryColor(Color c)
    {
        categoryToRenderer[currentFaceCategory].color = c;
    }

    public void SetSkinColor(Color c)
    {
        foreach (SpriteRenderer sr in skinRenderers)
        {
            sr.color = c;
        }
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
        categoryToRenderer.GetValueOrDefault("FB_" + crotch).enabled = false;
        categoryToRenderer.GetValueOrDefault("FB_" + lPant).enabled = false;
        categoryToRenderer.GetValueOrDefault("FB_" + rPant).enabled = false;
        //categoryToRenderer.GetValueOrDefault("FB_" + lSleeve).enabled = false;
        //categoryToRenderer.GetValueOrDefault("FB_" + rSleeve).enabled = false;
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
        if (!isFullBody)
            SelectFB();
        int idx = categoryToLabelIdx.GetValueOrDefault("FB_" + top) + idxDelta;
        string[] labels = GetUnlockedLabels("FB_" + top);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        int leftIdx = GetWrapAroundIndex(idx - 1, labels.Length - 1);
        int rightIdx = GetWrapAroundIndex(idx + 1, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx["FB_" + top] = idx;
        SetCategory("FB_" + top, label);
        SetCategory("FB_" + crotch, label);
        // TO DO (later)
        //string[] sleeveCategories = GetUnlockedLabels("FB_" + lSleeve);
        //if (sleeveCategories.Contains(label))
        //{
        //    SetCategory("FB_" + lSleeve, label);
        //    SetCategory("FB_" + rSleeve, label);
        //} else
        //{
        //    categoryToRenderer.GetValueOrDefault("FB_" + lSleeve).enabled = false;
        //    categoryToRenderer.GetValueOrDefault("FB_" + rSleeve).enabled = false;
        //}

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
        FBIcons.UpdateIcons(labels[leftIdx], labels[idx], labels[rightIdx]);
    }

    public void ChangeTop(int idxDelta)
    {
        if (isFullBody)
            SelectTopAndBottom();
        int idx = categoryToLabelIdx.GetValueOrDefault(top) + idxDelta;
        string[] labels = GetUnlockedLabels(top);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        int leftIdx = GetWrapAroundIndex(idx - 1 , labels.Length - 1);
        int rightIdx = GetWrapAroundIndex(idx + 1, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx[top] = idx;
        SetCategory(top, label);
        SetSleevesIfPresent(label);

        shirtIcons.UpdateIcons(labels[leftIdx], labels[idx], labels[rightIdx]);
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
        int idx = categoryToLabelIdx.GetValueOrDefault(lSock) + idxDelta;
        string[] labels = GetUnlockedLabels(lSock);
        idx = GetWrapAroundIndex(idx, labels.Length);
        int leftIdx = GetWrapAroundIndex(idx - 1, labels.Length);
        int rightIdx = GetWrapAroundIndex(idx + 1, labels.Length);

        categoryToLabelIdx[lSock] = idx;
        categoryToLabelIdx[rSock] = idx;

        if (idx == labels.Length)
        {
            // NONE OPTION
            categoryToRenderer[lSock].enabled = false;
            categoryToRenderer[rSock].enabled = false;
        }
        else
        {
            string label = labels[idx];
            SetCategory(lSock, label);
            SetCategory(rSock, label);
        }
        socksIcons.UpdateIcons(labels.ElementAtOrDefault(leftIdx) ?? "None", labels.ElementAtOrDefault(idx) ?? "None", labels.ElementAtOrDefault(rightIdx) ?? "None");
    }

    public void ChangeShoes(int idxDelta)
    {
        int idx = categoryToLabelIdx.GetValueOrDefault(lShoe) + idxDelta;
        string[] labels = GetUnlockedLabels(lShoe);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        int leftIdx = GetWrapAroundIndex(idx - 1, labels.Length - 1);
        int rightIdx = GetWrapAroundIndex(idx + 1, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx[lShoe] = idx;
        categoryToLabelIdx[rShoe] = idx;
        SetCategory(lShoe, label);
        SetCategory(rShoe, label);
        shoesIcons.UpdateIcons(labels[leftIdx], labels[idx], labels[rightIdx]);
    }

    public void ChangeBottom(int idxDelta)
    {
        if (isFullBody)
            SelectTopAndBottom();
        int idx = categoryToLabelIdx.GetValueOrDefault(crotch) + idxDelta;
        string[] labels = GetUnlockedLabels(crotch);
        idx = GetWrapAroundIndex(idx, labels.Length - 1);
        int leftIdx = GetWrapAroundIndex(idx - 1, labels.Length - 1);
        int rightIdx = GetWrapAroundIndex(idx + 1, labels.Length - 1);
        string label = labels[idx];
        categoryToLabelIdx[crotch] = idx;
        SetCategory(crotch, label);
        SetPantsIfPresent(label);
        bottomsIcons.UpdateIcons(labels[leftIdx], labels[idx], labels[rightIdx]);
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

    private void HideTailsWithHijab()
    {
        SpriteResolver res = categoryToResolver.GetValueOrDefault("Hair");
        SpriteRenderer sr = categoryToRenderer.GetValueOrDefault("Hair");
        string label = res.GetLabel();
        if (label.Contains("Hijab") && sr.enabled)
        {
            tailsSRen.enabled = false;
        }
    }

    public void ChangeFace(int idxDelta)
    {
        if (currentFaceCategory.Equals("Eyes") || currentFaceCategory.Equals("Mouth"))
        {
            ChangeEyesOrMouth(idxDelta);
            return;
        }
        int idx = categoryToLabelIdx.GetValueOrDefault(currentFaceCategory,0) + idxDelta;
        string[] labels = GetUnlockedLabels(currentFaceCategory);

        if (idx > labels.Length)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length;

        categoryToLabelIdx[currentFaceCategory] = idx;

        if (idx == labels.Length)
        {
            // NONE OPTION
            categoryToRenderer[currentFaceCategory].enabled = false;
        } else
        {
            string label = labels[idx];
            categoryToRenderer[currentFaceCategory].enabled = true;
            SetCategory(currentFaceCategory, label);
        }
        HideTailsWithHijab();
    }

    private void ChangeEyesOrMouth(int idxDelta)
    {
        int idx = categoryToLabelIdx.GetValueOrDefault(currentFaceCategory) + idxDelta;
        string[] labels = GetUnlockedLabels(currentFaceCategory);
        if (idx > labels.Length - 1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;
        string label = labels[idx];
        categoryToLabelIdx[currentFaceCategory] = idx;
        SetCategory(currentFaceCategory, label);
    }

    public void SetCurrentFaceCategory(string category)
    {
        Debug.Log("categoryToColorPalette is " + categoryToColorPalette.Keys);
        currentFaceCategory = category;
        Debug.Log("SETTING CATEGORY: " + category);
        foreach (ColorPalette cp in categoryToColorPalette.Values)
        {
            cp.gameObject.SetActive(cp.category.Equals(category));    
        }
    }

    private string[] GetUnlockedLabels(string category)
    {
        return categoryToLabels.GetValueOrDefault(category).Where(l => !l.StartsWith("X_") || l.Equals("X_" + gameObject.name)).ToArray();
    }
}

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
    
    // Skin
    public SpriteRenderer lArmSRen;
    public SpriteRenderer rArmSRen;
    public SpriteRenderer lLegSRen;
    public SpriteRenderer rLegSRen;
    public SpriteRenderer headSRen;
    public SpriteRenderer neckSRen;
    public SpriteRenderer torsoSRen;
    public SpriteRenderer crotchSRen;

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

    // Start is called before the first frame update
    void Start()
    {
        
        SpriteResolver[] resolvers = this.GetComponentsInChildren<SpriteResolver>();
        foreach (SpriteResolver resolver in resolvers)
        {
            categoryToResolver[resolver.gameObject.name] = resolver;
        }
        SpriteRenderer[] renderers = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            categoryToRenderer[renderer.gameObject.name] = renderer;
        }
        string[] labels;
        SpriteLibrary fuck = this.GetComponent<SpriteLibrary>();
        spriteLib = fuck.spriteLibraryAsset;
        foreach (string category in spriteLib.GetCategoryNames())
        {
            labels = spriteLib.GetCategoryLabelNames(category).ToArray();

            categoryToLabels[category] = labels;
            categoryToLabelIdx[category] = 0;
        }
        character = this.GetComponent<Character>();
        character.LoadCharacter();
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

        SetCurrentFaceCategory("Hair");
    }

    public void SetCurrentFaceCategoryColor(Color c)
    {
        categoryToRenderer[currentFaceCategory].color = c;
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
    }

    public void SelectFB()
    {
        isFullBody = true;
        ChangeFB(0);
        DisableNonFBRenderers();
    }

    public void ChangeFB(int idxDelta)
    {
        if (!isFullBody)
            SelectFB();
        int idx = categoryToLabelIdx.GetValueOrDefault("FB_" + top) + idxDelta;
        string[] labels = categoryToLabels.GetValueOrDefault("FB_" + top);
        if (idx > labels.Length -1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;
        string label = labels[idx];
        categoryToLabelIdx["FB_" + top] = idx;
        SetCategory("FB_" + top, label);
        SetCategory("FB_" + crotch, label);
        // TO DO (later)
        //string[] sleeveCategories = categoryToLabels.GetValueOrDefault("FB_" + lSleeve);
        //if (sleeveCategories.Contains(label))
        //{
        //    SetCategory("FB_" + lSleeve, label);
        //    SetCategory("FB_" + rSleeve, label);
        //} else
        //{
        //    categoryToRenderer.GetValueOrDefault("FB_" + lSleeve).enabled = false;
        //    categoryToRenderer.GetValueOrDefault("FB_" + rSleeve).enabled = false;
        //}

        string[] pantCategories = categoryToLabels.GetValueOrDefault("FB_" + lPant);
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
        
    }

    public void ChangeTop(int idxDelta)
    {
        if (isFullBody)
            SelectTopAndBottom();
        int idx = categoryToLabelIdx.GetValueOrDefault(top) + idxDelta;
        
        string[] labels = categoryToLabels.GetValueOrDefault(top);
        if (idx > labels.Length - 1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;
        string label = labels[idx];
        categoryToLabelIdx[top] = idx;
        SetCategory(top, label);
        SetSleevesIfPresent(label);
    }

    public void ChangeSocks(int idxDelta)
    {
        int idx = categoryToLabelIdx.GetValueOrDefault(lSock) + idxDelta;
        string[] labels = categoryToLabels.GetValueOrDefault(lSock);
        if (idx > labels.Length - 1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;
        string label = labels[idx];

        categoryToLabelIdx[lSock] = idx;
        categoryToLabelIdx[rSock] = idx;
        SetCategory(lSock, label);
        SetCategory(rSock, label);
    }

    public void ChangeShoes(int idxDelta)
    {
        int idx = categoryToLabelIdx.GetValueOrDefault(lShoe) + idxDelta;
        string[] labels = categoryToLabels.GetValueOrDefault(lShoe);
        if (idx > labels.Length - 1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;
        string label = labels[idx];
        categoryToLabelIdx[lShoe] = idx;
        categoryToLabelIdx[rShoe] = idx;
        SetCategory(lShoe, label);
        SetCategory(rShoe, label);
    }

    public void ChangeBottom(int idxDelta)
    {
        if (isFullBody)
            SelectTopAndBottom();
        int idx = categoryToLabelIdx.GetValueOrDefault(crotch) + idxDelta;
        string[] labels = categoryToLabels.GetValueOrDefault(crotch);
        if (idx > labels.Length - 1)
            idx = 0;
        else if (idx < 0)
            idx = labels.Length - 1;
        string label = labels[idx];
        categoryToLabelIdx[crotch] = idx;
        SetCategory(crotch, label);
        SetPantsIfPresent(label);
    }

    private void SetSleevesIfPresent(string label)
    {
        string[] sleeveCategories = categoryToLabels.GetValueOrDefault(lSleeve);
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
        string[] pantCategories = categoryToLabels.GetValueOrDefault(lPant);
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

    void SetHairColor(float r, float g, float b, float a)
    {
        eyebrowsSRen.color = new Color(r, g, b, a);
        bangsSRen.color = new Color(r, g, b, a);
        hairSRen.color = new Color(r, g, b, a);
    }

    void SetSkinColor(float r, float g, float b, float a)
    {
        lArmSRen.color = new Color(r, g, b, a);
        rArmSRen.color = new Color(r, g, b, a);
        lLegSRen.color = new Color(r, g, b, a);
        rLegSRen.color = new Color(r, g, b, a);
        headSRen.color = new Color(r, g, b, a);
        neckSRen.color = new Color(r, g, b, a);
        torsoSRen.color = new Color(r, g, b, a);
    }

    public void ChangeFace(int idxDelta)
    {
        if (currentFaceCategory.Equals("Eyes") || currentFaceCategory.Equals("Mouth"))
        {
            ChangeEyesOrMouth(idxDelta);
            return;
        }
        int idx = categoryToLabelIdx.GetValueOrDefault(currentFaceCategory) + idxDelta;
        string[] labels = categoryToLabels.GetValueOrDefault(currentFaceCategory);

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
    }

    private void ChangeEyesOrMouth(int idxDelta)
    {
        int idx = categoryToLabelIdx.GetValueOrDefault(currentFaceCategory) + idxDelta;
        string[] labels = categoryToLabels.GetValueOrDefault(currentFaceCategory);
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
        currentFaceCategory = category;
        Debug.Log("SETTING CATEGORY: " + category);
        foreach (ColorPalette cp in categoryToColorPalette.Values)
        {
            cp.gameObject.SetActive(cp.category.Equals(category));    
        }
    }
}

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterEditor : MonoBehaviour
{
    // SPRITE RESOLVERS

    //public SpriteResolver rShoeSRes;
    //public SpriteResolver lShoeSRes;
    //public SpriteResolver rSockSRes;
    //public SpriteResolver lSockSRes;
    //public SpriteResolver mouthSRes;
    //public SpriteResolver noseSRes;
    //public SpriteResolver eyesSRes;
    //public SpriteResolver eyebrowsSRes;
    //public SpriteResolver bangsSRes;
    //public SpriteResolver hairSRes;
    //public SpriteResolver necklaceSRes;
    static string top = "Top";
    static string lSleeve = "L_Sleeve";
    static string rSleeve = "R_Sleeve";
    static string crotch = "Crotch";
    static string rPant = "R_Pant";
    static string lPant = "L_Pant";
    static string rSock = "R_Sock";
    static string lSock = "L_Sock";
    static string rShoe = "R_Shoe";
    static string lShoe = "L_Shoe";
    //public SpriteResolver topSRes;
    //public SpriteResolver lSleeveSRes;
    //public SpriteResolver rSleeveSRes;
    //public SpriteResolver crotchSRes;
    //public SpriteResolver rPantSRes;
    //public SpriteResolver lPantSRes;

    // Body parts
    //public SpriteResolver lArmSRes;
    //public SpriteResolver rArmSRes;
    //public SpriteResolver lLegSRes;
    //public SpriteResolver rLegSRes;
    //public SpriteResolver headSRes;
    //public SpriteResolver neckSRes;
    //public SpriteResolver torsoSRes;

    // SPRITE RENDERERS

    // Hair
    public SpriteRenderer eyebrowsSRen;
    public SpriteRenderer bangsSRen;
    public SpriteRenderer hairSRen;

    //public SpriteRenderer rShoeSRen;
    //public SpriteRenderer lShoeSRen;
    //public SpriteRenderer rSockSRen;
    //public SpriteRenderer lSockSRen;
    //public SpriteRenderer mouthSRen;
    //public SpriteRenderer noseSRen;
    //public SpriteRenderer eyesSRen;
    //public SpriteRenderer necklaceSRen;
    //public SpriteRenderer topSRen;
    //public SpriteRenderer lSleeveSRen;
    //public SpriteRenderer rSleeveSRen;
    //public SpriteRenderer crotchSRen;
    //public SpriteRenderer rPantSRen;
    //public SpriteRenderer lPantSRen;

    // Body parts
    public SpriteRenderer lArmSRen;
    public SpriteRenderer rArmSRen;
    public SpriteRenderer lLegSRen;
    public SpriteRenderer rLegSRen;
    public SpriteRenderer headSRen;
    public SpriteRenderer neckSRen;
    public SpriteRenderer torsoSRen;

    private SpriteLibraryAsset spriteLib;
    private Dictionary<string, SpriteResolver> categoryToResolver = new Dictionary<string, SpriteResolver>();
    private Dictionary<string, SpriteRenderer> categoryToRenderer = new Dictionary<string, SpriteRenderer>();
    private Dictionary<string, string[]> categoryToLabels = new Dictionary<string, string[]>();
    private Dictionary<string, int> categoryToLabelIdx = new Dictionary<string, int>();
    private bool isFullBody;
    //private string[] FBLabels;
    //private string[] TopLabels;
    //private string[] BottomLabels;
    //private int FBLabelIdx = 0;
    //private int TopLabelIdx = 0;
    //private int BottomLabelIdx = 0;

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
            //if (category.Equals(top))
            //{
            //    FBLabels = labels.ToList().Where(l => l.StartsWith("FB_")).ToArray();
            //    TopLabels = labels.ToList().Where(l => !l.StartsWith("FB_")).ToArray();
            //} else if (category.Equals(lPant))
            //{
            //    BottomLabels = labels.ToList().Where(l => !l.StartsWith("FB_")).ToArray();
            //}
        }
        SelectTopAndBottom();
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

    //public void ChangeCategory(string category, int idxDelta)
    //{
    //    int idx = categoryToLabelIdx.GetValueOrDefault(category);
    //    idx+= idxDelta;
    //    string[] labels;
    //    labels = categoryToLabels.GetValueOrDefault(category);
    //    if (idx > labels.Length -1)
    //        idx = 0;
    //    else if (idx < 0)
    //        idx = labels.Length - 1;
    //    categoryToLabelIdx.Add(category, idx);
    //    SetCategory(category, labels[idx]);
    //}

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
}

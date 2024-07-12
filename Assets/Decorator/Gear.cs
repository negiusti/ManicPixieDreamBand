using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Gear : MonoBehaviour
{
    public bool Def;
    public bool shared;
    private string category;
    private string[] labels;
    private SpriteResolver spriteResolver;
    private SpriteLibrary spriteLibrary;
    private int index;
    private string label;
    private string saveKey;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        category = spriteResolver.GetCategory();
        saveKey = "gear_" + category;
        if (category == null)
        {
            Debug.Log("WHY THE FUCK IS THE CATEGORY NULL HERE: " + gameObject.name);
        }
        labels = InventoryManager.GetMCInventory(category).ToArray();
        if (shared || labels.Length == 0) // unlock everything
            labels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category).ToArray();
        // if (shared) label = "None";
        label = GetGearLabel(category);
        //Debug.Log("load gear: " + saveKey + label);
        index = Array.IndexOf(labels, label);
        //Debug.Log("FUCK YOU setting category and label: " + category + " " + label);
        spriteResolver.SetCategoryAndLabel(category, label);
        //Debug.Log("FUCK YOU the category is now: " + spriteResolver.GetCategory());
    }

    public static string GetGearLabel(string c)
    {
        return ES3.Load("gear_" + c, defaultValue: InventoryManager.defaultPurchaseables.data.First(p => p.category == c).items.First());
    }

    private void OnEnable()
    {
        if (spriteResolver == null)
            Start();
        if (Def)
        {
            label = ES3.Load(saveKey, defaultValue: spriteResolver.GetLabel());
            Debug.Log("load gear: " + saveKey + label);
            index = Array.IndexOf(labels, label);
            //Debug.Log("FUCK YOU setting category and label: " + category + " " + label);
            spriteResolver.SetCategoryAndLabel(category, label);
            //Debug.Log("FUCK YOU the category is now: " + spriteResolver.GetCategory());
        }
    }

    private void OnDestroy()
    {
        if (shared)
            return;
        saveKey = "gear_" + spriteResolver.GetCategory();
        Debug.Log("default: " + Def + " SAve gear: " + saveKey + spriteResolver.GetLabel());
        ES3.Save(saveKey, spriteResolver.GetLabel());
    }

    private void OnDisable()
    {
        if (shared)
            return;
        saveKey = "gear_" + spriteResolver.GetCategory();
        Debug.Log("default: " + Def + " SAve gear: " + saveKey + spriteResolver.GetLabel());
        ES3.Save(saveKey, spriteResolver.GetLabel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string Category()
    {
        if (category == null)
            Start();
        return category;
    }

    public string Label()
    {
        if (label == null)
            Start();
        return label;
    }

    //public void SetBand(string band)
    //{
    //    if (!shared)
    //        return;
    //    spriteResolver.SetCategoryAndLabel(category, label);
    //}

    public void Change(int delta)
    {
        index = GetWrapAroundIndex(index + delta, labels.Length - 1);
        label = labels[index];
        //Debug.Log("FUCK YOU setting category and label: " + category + " " + label);
        spriteResolver.SetCategoryAndLabel(category, label);
        //Debug.Log("FUCK YOU the category is now: " + spriteResolver.GetCategory());
    }

    private int GetWrapAroundIndex(int idx, int maxIdx)
    {
        if (idx > maxIdx)
            idx = 0;
        else if (idx < 0)
            idx = maxIdx;
        return idx;
    }
}

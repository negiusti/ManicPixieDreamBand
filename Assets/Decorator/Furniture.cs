using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Furniture : MonoBehaviour
{
    private string category;
    private string[] labels;
    private SpriteResolver spriteResolver;
    //private SpriteLibrary spriteLibrary;
    private int index;
    private string label;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        //spriteLibrary = GetComponent<SpriteLibrary>();
        category = spriteResolver.GetCategory();
        labels = InventoryManager.GetMCInventory(category).ToArray();
        //labels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category).ToArray();
        label = spriteResolver.GetLabel();
        index = Array.IndexOf(labels, label);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Category()
    {
        return category;
    }

    public void Change(int delta)
    {
        index = GetWrapAroundIndex(index + delta, labels.Length);
        label = labels[index];
        spriteResolver.SetCategoryAndLabel(category, label);
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

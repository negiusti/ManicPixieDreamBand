using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Furniture : MonoBehaviour
{
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
        labels = InventoryManager.GetMCInventory(category).ToArray();
        if (labels.Length == 0)
            labels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category).ToArray();
        saveKey = (SceneChanger.Instance.GetCurrentScene() == "Character_Editor" ? "Bedroom" : SceneChanger.Instance.GetCurrentScene()) + category + gameObject.name;
        label = ES3.Load(saveKey, defaultValue: spriteResolver.GetLabel());
        spriteResolver.SetCategoryAndLabel(category, label);
        index = Array.IndexOf(labels, label);
    }

    private void OnEnable()
    {
        if (spriteResolver == null)
            return;
        category = spriteResolver.GetCategory();
        labels = InventoryManager.GetMCInventory(category).ToArray();
        if (labels.Length == 0)
            labels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category).ToArray();
        label = ES3.Load(saveKey, defaultValue: spriteResolver.GetLabel());
        spriteResolver.SetCategoryAndLabel(category, label);
        index = Array.IndexOf(labels, label);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (spriteResolver == null)
            return;
        Debug.Log("Saving " + saveKey + " " + spriteResolver.GetLabel());
        ES3.Save(saveKey, spriteResolver.GetLabel());
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

    public void Change(int delta)
    {
        index = GetWrapAroundIndex(index + delta, labels.Length-1);
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

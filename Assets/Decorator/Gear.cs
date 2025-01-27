using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Gear : MonoBehaviour
{
    public bool shared;
    private string category;
    private SpriteResolver spriteResolver;
    private SpriteRenderer spriteRenderer;
    private string label;
    private string saveKey;
    static private HashSet<string> whenToShowInstInBar = new HashSet<string> { "open mic", "gig" };
    
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        category = spriteResolver.GetCategory();
        saveKey = "gear_" + category;
        if (category == null)
        {
            Debug.Log("WHY THE FUCK IS THE CATEGORY NULL HERE: " + gameObject.name);
        }
        label = GetGearLabel(category);
        spriteResolver.SetCategoryAndLabel(category, label);
        if (SceneChanger.Instance.GetCurrentScene() == "SmallBar")
        {
            spriteRenderer.enabled = whenToShowInstInBar.Any(w => Calendar.GetCurrentEvent().Name().ToLower().Contains(w));
        }
    }

    public static string GetGearLabel(string c)
    {
        return ES3.Load("gear_" + c, defaultValue: InventoryManager.defaultPurchaseables.data.First(p => p.category == c).items.First());
    }

    public void UpdateGearSelection()
    {
        if (shared)
            return;
        if (spriteResolver == null)
            Start();
        label = ES3.Load(saveKey, defaultValue: spriteResolver.GetLabel());
        spriteResolver.SetCategoryAndLabel(category, label);
    }

    //private void OnEnable()
    //{
    //    if (spriteResolver == null)
    //        Start();
    //    label = ES3.Load(saveKey, defaultValue: spriteResolver.GetLabel());
    //    Debug.Log("load gear: " + saveKey + label);
    //    spriteResolver.SetCategoryAndLabel(category, label);
    //}

    //private void OnDestroy()
    //{
    //    if (shared)
    //        return;
    //    saveKey = "gear_" + spriteResolver.GetCategory();
    //    Debug.Log("Save gear: " + saveKey + spriteResolver.GetLabel());
    //    ES3.Save(saveKey, spriteResolver.GetLabel());
    //}

    //private void OnDisable()
    //{
    //    if (shared)
    //        return;
    //    saveKey = "gear_" + spriteResolver.GetCategory();
    //    Debug.Log("Save gear: " + saveKey + spriteResolver.GetLabel());
    //    ES3.Save(saveKey, spriteResolver.GetLabel());
    //}

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

    //public void Change(int delta)
    //{
    //    index = GetWrapAroundIndex(index + delta, labels.Length - 1);
    //    label = labels[index];
    //    //Debug.Log("FUCK YOU setting category and label: " + category + " " + label);
    //    spriteResolver.SetCategoryAndLabel(category, label);
    //    //Debug.Log("FUCK YOU the category is now: " + spriteResolver.GetCategory());
    //}

    //private int GetWrapAroundIndex(int idx, int maxIdx)
    //{
    //    if (idx > maxIdx)
    //        idx = 0;
    //    else if (idx < 0)
    //        idx = maxIdx;
    //    return idx;
    //}
}

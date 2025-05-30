using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ItemSwapPhoneUI : MonoBehaviour
{
    private ItemSwapIcon icon;
    private Furniture furniture;
    private SpriteLibrary spriteLibrary;

    public GameObject notifIndicator;
    public string gearCategory;
    private List<Gear> gearInScene;
    private string[] gearLabels;
    private int gearIdx;
    private string gearLabel;
    private DecoratorApp decoratorApp;
    private GearApp gearApp;

    public string Category()
    {
        if (furniture != null)
            return furniture.Category();
        if (gearCategory != null)
            return gearCategory;
        return "None";
    }

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentInChildren<ItemSwapIcon>(includeInactive: true);
        spriteLibrary = GetComponentInChildren<SpriteLibrary>(includeInactive: true);
        decoratorApp = GetComponentInParent<DecoratorApp>(includeInactive: true);
        gearApp = GetComponentInParent<GearApp>(includeInactive: true);
    }

    private void OnEnable()
    {
        if (InventoryManager.defaultPurchaseables == null)
            return;
        if (icon == null)
            Start();
        if (gearCategory == null || gearCategory.Length == 0)
        {
            if (furniture != null)
                notifIndicator.SetActive(decoratorApp.HasNotification(furniture.Category()));
            return;
        }
        
        gearInScene = FindObjectsOfType<Gear>().Where(g => !g.shared).ToList();
        gearLabels = InventoryManager.GetMCInventory(gearCategory).ToArray();
        if (gearLabels.Length == 0) // unlock everything
        {
            Debug.Log("Unlocking everything bc there's nothing in inventory for category: " + gearCategory);
            gearLabels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(gearCategory).ToArray();
        }
        gearLabel = Gear.GetGearLabel(gearCategory);
        gearIdx = Array.IndexOf(gearLabels, gearLabel);
        icon.AssignItem(gearCategory, gearLabel);
        notifIndicator.SetActive(gearApp.HasNotification(gearCategory));
    }

    public void AssignItem(Furniture f, bool showNotif=false)
    {
        if (icon == null)
            Start();
        furniture = f;
        icon.AssignItem(f.Category(), f.Label());
        notifIndicator.SetActive(showNotif);
    }

    private void UpdateGearInScene()
    {
        if (gearInScene == null)
            OnEnable();
        foreach (Gear g in gearInScene)
        {
            g.UpdateGearSelection();
        }
    }

    public void SwapFurniture(int delta)
    {
        bool moreThanOne = furniture.Change(delta);
        if (!moreThanOne)
            Phone.Instance.NotificationMessage("Go shopping at the thrift store\nto unlock more furniture!");
        icon.UpdateIcon(furniture.Label());
        notifIndicator.SetActive(false);
        decoratorApp.ClearNotification(furniture.Category());
    }

    public void SwapGear(int delta)
    {
        gearIdx = GetWrapAroundIndex(gearIdx + delta, gearLabels.Length - 1);
        gearLabel = gearLabels[gearIdx];
        icon.UpdateIcon(gearLabel);
        string saveKey = "gear_" + gearCategory;
        ES3.Save(saveKey, gearLabel);
        UpdateGearInScene();
        notifIndicator.SetActive(false);
        gearApp.ClearNotification(gearCategory);
        bool moreThanOne = gearLabels.Length > 1;
        if (!moreThanOne)
            Phone.Instance.NotificationMessage("Go shopping at the music shop\nto unlock more gear!");
    }

    private int GetWrapAroundIndex(int idx, int maxIdx)
    {
        if (idx > maxIdx)
            idx = 0;
        else if (idx < 0)
            idx = maxIdx;
        return idx;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

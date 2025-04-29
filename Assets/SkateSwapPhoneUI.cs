using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SkateSwapPhoneUI : MonoBehaviour
{
    private ItemSwapIcon icon;
    private SpriteLibrary spriteLibrary;

    public GameObject notifIndicator;
    public bool isBoard;
    private string skateCategory;
    private SkateApp skateApp;
    private int skateIdx;
    private string[] skateLabels;
    private string skateLabel;

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentInChildren<ItemSwapIcon>(includeInactive: true);
        spriteLibrary = GetComponentInChildren<SpriteLibrary>(includeInactive: true);
        skateApp = GetComponentInParent<SkateApp>(includeInactive: true);
        skateCategory = isBoard ? MainCharacterState.SkateboardCategory : MainCharacterState.RollerskateCategory;
    }

    private void OnEnable()
    {
        if (InventoryManager.defaultPurchaseables == null)
            return;
        if (icon == null)
            Start();
        skateLabels = InventoryManager.GetMCInventory(skateCategory).ToArray();
        if (skateLabels.Length == 0) // unlock everything
        {
            Debug.Log("Unlocking everything bc there's nothing in inventory for category: " + skateCategory);
            skateLabels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(skateCategory).ToArray();
        }
        skateLabel = isBoard ? MainCharacterState.GetSkateboardLabel() : MainCharacterState.GetRollerSkateLabel();
        skateIdx = Array.IndexOf(skateLabels, skateLabel);
        icon.AssignItem(skateCategory, skateLabel);
        notifIndicator.SetActive(skateApp.HasNotification(skateCategory));
    }

    public void SwapSkates(int delta)
    {
        skateIdx = GetWrapAroundIndex(skateIdx + delta, skateLabels.Length - 1);
        skateLabel = skateLabels[skateIdx];
        icon.UpdateIcon(skateLabel);
        ES3.Save(skateCategory, skateLabel);
        UpdateSkatesOnMainCharacter();
        notifIndicator.SetActive(false);
        skateApp.ClearNotification(skateCategory);
        bool moreThanOne = skateLabels.Length > 1;
        if (!moreThanOne)
            Phone.Instance.NotificationMessage("Go shopping at the skate shop\nto unlock more skates!");
    }

    private void UpdateSkatesOnMainCharacter()
    {
        Characters.MainCharacter().UpdateSkates();
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

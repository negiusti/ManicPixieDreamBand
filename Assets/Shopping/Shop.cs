using PixelCrushers.DialogueSystem;
using UnityEngine;

public abstract class Shop : MonoBehaviour
{
    protected Shopkeeper shopkeeper;
    protected ShopDisplay[] displays;
    protected CustomDialogueScript customDialogue;
    protected Purchasable currentSelectedPurchasable;
    protected int lastInventoryRefreshDay;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        shopkeeper = FindFirstObjectByType<Shopkeeper>();
        displays = FindObjectsOfType<ShopDisplay>();

        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        lastInventoryRefreshDay = ES3.Load<int>("LastRefresh" + ShopName(), -1);
        if (HasDayPassed())
        {
            RandomizeDisplays();
        }
    }

    protected virtual bool HasDayPassed()
    {
        Debug.Log("lastInventoryRefreshDay" + lastInventoryRefreshDay + "date=" + Calendar.Date());
        return lastInventoryRefreshDay < Calendar.Date();
    }

    protected virtual void RandomizeDisplays()
    {
        Debug.Log("randomize displays");
        lastInventoryRefreshDay = Calendar.Date();
        ES3.Save<int>("LastRefresh" + ShopName(), lastInventoryRefreshDay);
        foreach (ShopDisplay d in displays)
        {
            d.Randomize();
        }
    }

    protected virtual void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(MakeAPurchase), this, SymbolExtensions.GetMethodInfo(() => MakeAPurchase()));
        Lua.RegisterFunction(nameof(CurrentPurchasablePrice), this, SymbolExtensions.GetMethodInfo(() => CurrentPurchasablePrice()));
        Lua.RegisterFunction(nameof(CurrentPurchasableName), this, SymbolExtensions.GetMethodInfo(() => CurrentPurchasableName()));
    }

    protected virtual void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(MakeAPurchase));
        Lua.UnregisterFunction(nameof(CurrentPurchasablePrice));
        Lua.UnregisterFunction(nameof(CurrentPurchasableName));
        //}
        ES3.Save<int>("LastRefresh" + ShopName(), lastInventoryRefreshDay);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public abstract void AskToBuy(Purchasable p);
    public abstract string ShopName();

    public void MakeAPurchase()
    {
        currentSelectedPurchasable.Buy();
        //currentSelectedPurchasable = null;
    }

    public double CurrentPurchasablePrice()
    {
        Debug.Log("CurrentPurchasablePrice() is " + currentSelectedPurchasable.price);
        return currentSelectedPurchasable == null ? 0 : currentSelectedPurchasable.price;
    }
    public string CurrentPurchasableName()
    {
        return currentSelectedPurchasable == null ? "" : currentSelectedPurchasable.itemName.ToString();
    }
    //public string CurrentPurchasableCategory()
    //{
    //    return currentSelectedPurchasable == null ? "" : currentSelectedPurchasable.category.ToString();
    //}
}

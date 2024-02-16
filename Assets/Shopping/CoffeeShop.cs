using PixelCrushers.DialogueSystem;

public class CoffeeShop : Shop
{
    private static string purchaseConvo = "CoffeeShop_Purchase";

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void AskToBuy(Purchasable p)
    {
        currentSelectedPurchasable = p;
        customDialogue.StartShopkeeperConvo(purchaseConvo);
    }

    public override string ShopName()
    {
        return "CoffeeShop";
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(MakeAPurchase), this, SymbolExtensions.GetMethodInfo(() => MakeAPurchase()));
        Lua.RegisterFunction(nameof(CurrentPurchasablePrice), this, SymbolExtensions.GetMethodInfo(() => CurrentPurchasablePrice()));
        Lua.RegisterFunction(nameof(CurrentPurchasableName), this, SymbolExtensions.GetMethodInfo(() => CurrentPurchasableName()));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(MakeAPurchase));
        Lua.UnregisterFunction(nameof(CurrentPurchasablePrice));
        Lua.UnregisterFunction(nameof(CurrentPurchasableName));
        //}
    }
}

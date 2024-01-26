using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class ThriftStore : Shop
{
    private static string purchaseConvo = "ThriftStore_Purchase";

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

    new void AskToBuy(Purchasable p)
    {
        base.AskToBuy(p);
        customDialogue.StartShopkeeperConvo(purchaseConvo);
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

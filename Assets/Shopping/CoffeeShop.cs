using PixelCrushers.DialogueSystem;
using UnityEngine;

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
        if (DialogueManager.IsConversationActive)
        {
            Debug.Log("Conversation already active");
            return;
        }
        currentSelectedPurchasable = p;
        customDialogue.StartShopkeeperConvo(purchaseConvo);
    }

    public override string ShopName()
    {
        return "CoffeeShop";
    }

    new void OnEnable()
    {
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}

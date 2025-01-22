using PixelCrushers.DialogueSystem;
using UnityEngine;

public class BobaShop : Shop
{
    private static string purchaseConvo = "BobaShop_Purchase";

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
        return "BobaShop";
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

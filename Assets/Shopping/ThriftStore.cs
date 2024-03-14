using PixelCrushers.DialogueSystem;

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

    public override void AskToBuy(Purchasable p)
    {
        currentSelectedPurchasable = p;
        customDialogue.StartShopkeeperConvo(purchaseConvo);
    }

    public override string ShopName()
    {
        return "ThriftStore";
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

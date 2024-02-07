using PixelCrushers.DialogueSystem;
using UnityEngine;

public abstract class Shop : MonoBehaviour
{
    protected Shopkeeper shopkeeper;
    protected ShopDisplay[] displays;
    protected CustomDialogueScript customDialogue;
    protected Purchasable currentSelectedPurchasable;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        shopkeeper = FindFirstObjectByType<Shopkeeper>();
        displays = FindObjectsOfType<ShopDisplay>();

        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        if (HasDayPassed())
        {
            RandomizeDisplays();
        }
    }

    // TO-DO: after implementing calendar system
    protected virtual bool HasDayPassed()
    {
        return false;
    }

    protected virtual void RandomizeDisplays()
    {
        foreach (ShopDisplay d in displays)
        {
            d.Randomize();
        }
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

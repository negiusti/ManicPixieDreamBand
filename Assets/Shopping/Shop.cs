using PixelCrushers.DialogueSystem;
using UnityEngine;

public abstract class Shop : MonoBehaviour
{
    protected Shopkeeper shopkeeper;
    protected Purchasable[] purchasables;
    protected CustomDialogueScript customDialogue;
    protected Purchasable currentSelectedPurchasable;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        shopkeeper = FindFirstObjectByType<Shopkeeper>();
        purchasables = FindObjectsOfType<Purchasable>();
        foreach (Purchasable p in purchasables)
        {
            p.SetShop(this);
        }
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        if (HasDayPassed())
        {
            RandomizePurchaseables();
        }
    }

    // TO-DO: after implementing calendar system
    protected virtual bool HasDayPassed()
    {
        return true;
    }

    protected virtual void RandomizePurchaseables()
    {
        foreach (Purchasable p in purchasables)
        {
            p.gameObject.SetActive(true);
            p.Randomize();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public abstract void AskToBuy(Purchasable p);

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
    public string CurrentPurchasableCategory()
    {
        return currentSelectedPurchasable == null ? "" : currentSelectedPurchasable.category.ToString();
    }
}

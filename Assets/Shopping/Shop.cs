using PixelCrushers.DialogueSystem;
using UnityEngine;

public abstract class Shop : MonoBehaviour
{
    protected Shopkeeper shopkeeper;
    protected Purchasable[] purchasables;
    protected CustomDialogueScript customDialogue;
    private Purchasable currentSelectedPurchasable;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        shopkeeper = FindFirstObjectByType<Shopkeeper>();
        purchasables = FindObjectsOfType<Purchasable>();
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void AskToBuy(Purchasable p)
    {
        currentSelectedPurchasable = p;
    }

    public void MakeAPurchase()
    {
        currentSelectedPurchasable.Buy();
        //currentSelectedPurchasable = null;
    }

    public string CurrentPurchasablePrice()
    {
        return currentSelectedPurchasable == null ? "" : currentSelectedPurchasable.price.ToString();
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

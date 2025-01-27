using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class Purchasable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] public string itemName;
    [SerializeField] public double price;
    [SerializeField] private bool sold;
    [SerializeField] private bool pixiecore;
    private string category;
    private Shop shop;
    private SpriteResolver spriteResolver;
    private SpriteLibraryAsset spriteLib;
    private string purchaseableName;
    private PriceTag priceTag;
    private ShopDisplay shopDisplay;
    private HashSet<string> questExclusives = new HashSet<string> { "Vintage Poster", "Punk Juice Shirt", "Daisy Dukes Shirt" };
    private Animator animator;

    private class PurchasableData
    {
        public string i;
        public double p;
        public bool s;
        public PurchasableData(string i, double p, bool s)
        {
            this.i = i;
            this.p = p;
            this.s = s;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shopDisplay = GetComponentInParent<ShopDisplay>();
        spriteResolver = GetComponent<SpriteResolver>();
        category = spriteResolver.GetCategory();
        itemName = spriteResolver.GetLabel();
        SpriteLibrary sl = GetComponent<SpriteLibrary>();
        spriteLib = sl.spriteLibraryAsset;
        purchaseableName = SceneChanger.Instance.GetCurrentScene() + "_" + this.gameObject.name;
        shop = FindObjectOfType<Shop>();
        priceTag = GetComponentInChildren<PriceTag>();
        if (priceTag == null)
        {
            Debug.LogError("Purchasable is missing price tag: " + gameObject.name);
        }
        if (animator == null)
        {
            Debug.LogError("Purchasable is missing animator: " + gameObject.name);
        }
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (spriteResolver == null)
            Start();
    }

    private void Save()
    {
        ES3.Save(purchaseableName, new PurchasableData(itemName, price, sold));
    }

    private void Load()
    {
        if (!ES3.KeyExists(purchaseableName))
        {
            return;
        }
        PurchasableData p = ES3.Load<PurchasableData>(purchaseableName);
        itemName = p.i;
        price = p.p;
        sold = p.s;
        spriteResolver.SetCategoryAndLabel(category, itemName);
        SetBought(sold);
    }

    private void OnDisable()
    {
        Save();
    }

    public string[] GetAvailableStock()
    {
        if (category == null)
            Start();

        if (pixiecore)
        {
            return spriteLib.GetCategoryLabelNames(category).Where(i => !i.Equals("None") &&
                !InventoryManager.GetMCInventory(category).Contains(i) &&
                i.ToLower().Contains("pixiecore")).ToArray();
        }

        HashSet<string> purchasedItems = InventoryManager.GetPurchasedItems(category);
        // Don't sell already purchased items or quest exclusives or items exclusive to NPCs.
        return spriteLib.GetCategoryLabelNames(category).Where(i => !i.Equals("None") &&
            !purchasedItems.Contains(i) &&
            !questExclusives.Contains(i) &&
            !i.StartsWith("X_") &&
            !i.ToLower().Contains("pixiecore")).ToArray();
    }

    public void Randomize()
    {
        string[] labels = GetAvailableStock();
        // Already bought everything in this category ¯\_(ツ)_/¯
        if (labels.Length == 0)
        {
            SetBought(true);
            return;
        }
        int randomIdx = Random.Range(0, labels.Length);
        itemName = labels[randomIdx];
        SetBought(false);
        spriteResolver.SetCategoryAndLabel(category, itemName);
        // TO-DO: look up price for new item
    }

    private void OnMouseDown()
    {
        Debug.Log("Ask shop + " + shop + " to buy");
        shop.AskToBuy(this);
    }

    private void OnMouseEnter()
    {
        priceTag.ShowPrice();
    }

    private void OnMouseExit()
    {
        priceTag.HidePrice();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }

    public void Buy()
    {
        MainCharacterState.ModifyBankBalance(price * -1.0);
        InventoryManager.AddToMCInventory(category, itemName);
        InventoryManager.RecordPurchase(category, itemName);
        shopDisplay.Randomize();
        //SetBought(true);
    }

    public bool IsBought()
    {
        return sold;
    }

    public string GetCategory()
    {
        if (category == null)
            Start();
        return category;
    }

    public void SetBought(bool bought)
    {
        sold = bought;
        this.gameObject.GetComponent<Collider2D>().enabled = !bought;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = !bought;
    }
}

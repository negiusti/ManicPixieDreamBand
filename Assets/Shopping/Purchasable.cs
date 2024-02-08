using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Purchasable : MonoBehaviour
{
    [SerializeField] public string itemName;
    [SerializeField] public double price;
    [SerializeField] private bool sold;
    private string category;
    private Shop shop;
    private SpriteResolver spriteResolver;
    private SpriteLibraryAsset spriteLib;
    private string purchaseableName;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        spriteResolver = this.GetComponent<SpriteResolver>();
        category = spriteResolver.GetCategory();
        itemName = spriteResolver.GetLabel();
        SpriteLibrary sl = this.GetComponent<SpriteLibrary>();
        spriteLib = sl.spriteLibraryAsset;
        purchaseableName = SceneChanger.Instance.GetCurrentScene() + "_" + this.gameObject.name;
        shop = FindObjectOfType<Shop>();
        Load();
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

    public void Randomize()
    {
        string[] labels = spriteLib.GetCategoryLabelNames(category).ToArray();
        int randomIdx = Random.Range(0, labels.Length);
        itemName = labels[randomIdx];
        sold = false;
        SetBought(false);
        spriteResolver.SetCategoryAndLabel(category, itemName);
        // TO-DO: exclude items that have already been purchased
        // TO-DO: look up price for new item
    }

    private void OnMouseDown()
    {
        Debug.Log("Ask shop + " + shop + " to buy");
        shop.AskToBuy(this);
    }

    private void OnMouseEnter()
    {
        // show price
    }

    private void OnMouseExit()
    {
        // hide price
    }

    public void Buy()
    {
        MainCharacterState.ModifyBankBalance(price * -1.0);
        // TODO: modify inventory
        sold = true;
        SetBought(true);
    }

    public void SetBought(bool bought)
    {
        this.gameObject.GetComponent<Collider2D>().enabled = !bought;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = !bought;
    }
}

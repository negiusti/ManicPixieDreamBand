using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Purchasable : MonoBehaviour
{
    public string itemName;
    public string category;
    public double price;
    private Shop shop;
    private SpriteResolver spriteResolver;
    private SpriteLibraryAsset spriteLib;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = this.GetComponent<SpriteResolver>();
        itemName = spriteResolver.GetLabel();
        category = spriteResolver.GetCategory();
        SpriteLibrary sl = this.GetComponent<SpriteLibrary>();
        spriteLib = sl.spriteLibraryAsset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShop(Shop s)
    {
        shop = s;
    }

    public void Randomize()
    {
        string[] labels = spriteLib.GetCategoryLabelNames(category).ToArray();
        int randomIdx = Random.Range(0, labels.Length);
        itemName = labels[randomIdx];
        spriteResolver.SetCategoryAndLabel(category, itemName);
        // TO-DO: exclude items that have already been purchased
        // TO-DO: look up price for new item
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
        this.gameObject.SetActive(false);
    }
}

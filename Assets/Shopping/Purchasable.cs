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

    // Start is called before the first frame update
    void Start()
    {
        // TO DO : FIND A WAY TO 
        //shop = 
        spriteResolver = this.GetComponent<SpriteResolver>();
        itemName = spriteResolver.GetLabel();
        category = spriteResolver.GetCategory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShop(Shop s)
    {
        shop = s;
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
        // TODO: set spriteresolver to none
        // TODO: disable collider
        MainCharacterState.ModifyBankBalance(price * -1.0);
        // TODO: modify inventory
    }
}

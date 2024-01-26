using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchasable : MonoBehaviour
{
    public string itemName;
    public string category;
    public double price;
    private Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        shop = FindFirstObjectByType<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        shop.AskToBuy(this);
    }

    private void OnMouseEnter()
    {
        // show price
    }

    private void MouseExit()
    {
        // hide price
    }

    public void Buy()
    {
        // replace sprite resolver with none
        MainCharacterState.ModifyBankBalance(price * -1.0);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceTag : MonoBehaviour
{
    private Purchasable purchasable;
    private TextMeshPro tmp;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        purchasable = this.GetComponentInParent<Purchasable>();
        tmp = this.GetComponentInChildren<TextMeshPro>();
        tmp.text = "$" + purchasable.price;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPrice()
    {
        animator.Play("Price_Show");
    }

    public void HidePrice()
    {
        animator.Play("Price_Hide");
    }
}

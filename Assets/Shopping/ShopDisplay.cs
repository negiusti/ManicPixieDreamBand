using System.Linq;
using UnityEngine;

public class ShopDisplay : MonoBehaviour
{
    private Purchasable[] childPurchasables;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        childPurchasables = GetComponentsInChildren<Purchasable>(true);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Randomize()
    {
        if (childPurchasables == null)
            Start();

        // All of this displays purchaseables have been purchased
        if (childPurchasables.All(p => p.GetAvailableStock().Length == 0))
            return;

        // Find a purchaseable that still has some items left to be purchased
        int randomIdx;
        do {
            randomIdx = Random.Range(0, childPurchasables.Length);
        } while (childPurchasables[randomIdx].GetAvailableStock().Length == 0);

        Debug.Log("randomize displays " + randomIdx);

        for (int i = 0; i < childPurchasables.Length; i ++)
        {
            bool selected = (i == randomIdx);
            if (selected)
            {
                childPurchasables[i].Randomize();
            } else
            {
                childPurchasables[i].SetBought(true);
            }

        }
        if(animator != null)
           animator.Play("Pop");
    }
}

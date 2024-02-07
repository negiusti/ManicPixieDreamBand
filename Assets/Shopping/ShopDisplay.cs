using UnityEngine;

public class ShopDisplay : MonoBehaviour
{
    private Purchasable[] childPurchasables;

    // Start is called before the first frame update
    void Start()
    {
        childPurchasables = this.GetComponentsInChildren<Purchasable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Randomize()
    {
        int randomIdx = Random.Range(0, childPurchasables.Length);
        for (int i = 0; i < childPurchasables.Length; i ++)
        {
            bool selected = (i == randomIdx);
            childPurchasables[i].gameObject.SetActive(selected);
            if (childPurchasables[i].gameObject.activeSelf)
            {
                childPurchasables[i].Randomize();
            }
        }
    }
}

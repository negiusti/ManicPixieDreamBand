using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BobaOrder : MonoBehaviour
{
    public BobaOrderItem[] orderItems;
    public TextMeshPro bathroomCode;
    private Animator animator;
    private Dictionary<BobaMiniGame.Step, BobaOrderItem> map;
    // Start is called before the first frame update
    void Start()
    {
        map = orderItems.ToDictionary(x => x.step, x => x);
        animator = GetComponent<Animator>();
    }

    public void RandomizeOrder()
    {
        if (animator == null)
            Start();
        foreach(BobaOrderItem item in orderItems)
        {
            item.Randomize();
        }
        bathroomCode.text = Random.Range(1000, 9999).ToString();
        animator.Play("NewOrderPop", -1, 0f);
    }

    public bool CheckOrderItem(BobaMiniGame.Step step, string key)
    {
        bool result = map[step].CurrentRequestedItem() == key;
        if (result)
            map[step].Check();
        else
            map[step].Fail();
        return result;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

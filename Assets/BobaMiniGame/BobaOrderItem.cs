using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaOrderItem : MonoBehaviour
{
    public BobaMiniGame.Step step;
    private SpriteRenderer line;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check()
    {
        line.color = Color.green;
        animator.Play("OrderItemCross");
    }

    public void Fail()
    {
        line.color = Color.red;
        animator.Play("OrderItemCross");
    }
}

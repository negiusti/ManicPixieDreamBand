using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Toppings : MonoBehaviour
{
    private Animator animator;
    private SpriteResolver[] srs;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        srs = GetComponentsInChildren<SpriteResolver>(includeInactive:true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetToppingType(string type)
    {
        foreach (SpriteResolver sr in srs)
        {
            sr.SetCategoryAndLabel("Toppings", type);
        }
    }

    public void AppearInCup()
    {
        animator.Play("ToppingsAppear");
    }
}

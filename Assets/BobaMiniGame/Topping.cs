using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Topping : MonoBehaviour
{
    private Animator animator;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;
    private BobaMiniGame mg;
    public Toppings toppings;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(11.5f, origialPos.y, origialPos.z);
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        if (mg.toppingsDone)
            return;
        mg.toppingsDone = true;
        toppings.SetToppingType(gameObject.name);
        StartCoroutine(lerp.Lerp(targetPos, 0.5f));
        animator.Play("FlavorPour");
        toppings.AppearInCup();
        mg.Next();
    }
}

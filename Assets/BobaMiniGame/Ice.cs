using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private Animator animator;
    public IceMachine iceMachine;
    public BobaMiniGame mg;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnIceMachine()
    {
        iceMachine.On();
    }

    public void TurnOffIceMachine()
    {
        iceMachine.Off();
    }

    public void SmallFill()
    {
        animator.SetBool("Medium", false);
        animator.SetBool("Large", false);
        animator.Play("Small");
        mg.Next();
    }

    public void MediumFill()
    {
        animator.SetBool("Medium", true);
        animator.SetBool("Large", false);
        animator.Play("Small");
        mg.Next();
    }

    public void LargeFill()
    {
        animator.SetBool("Medium", true);
        animator.SetBool("Large", true);
        animator.Play("Small");
        mg.Next();
    }
}

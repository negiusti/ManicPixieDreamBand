using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaboodleSection : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        animator.SetBool("Selected", true);
    }

    public void Unselect()
    {
        animator.SetBool("Selected", false);
    }

    public void DisableSection()
    {
        Unselect();
    }

    public void EnableSection()
    {

    }
}

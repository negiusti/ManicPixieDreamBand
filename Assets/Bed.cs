using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("DoneForTheDay", Calendar.DoneForTheDay());
        GetComponent<BiggerWhenHovered>().scaleFactor = Calendar.DoneForTheDay() ? 1.1f : 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToSleep()
    {
        Calendar.Sleep();
    }

    private void OnMouseDown()
    {
        if (Calendar.DoneForTheDay())
        {
            animator.Play("Sleep", -1, 0f);
            animator.SetBool("DoneForTheDay", false);
            GetComponent<BiggerWhenHovered>().scaleFactor = 1f;
        }
    }
}

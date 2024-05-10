using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkDoor : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void CloseTrunk()
    {
        anim.SetBool("HasWon", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkDoor : MonoBehaviour
{
    private Animator anim;

    private CarPackingMinigame mg;

    private void Start()
    {
        mg = GetComponentInParent<CarPackingMinigame>();
        anim = GetComponent<Animator>();
    }

    public void CloseTrunk()
    {
        anim.SetBool("HasWon", true);
    }

    public void CloseCarPacking()
    {
        mg.CloseMiniGame();
    }
}

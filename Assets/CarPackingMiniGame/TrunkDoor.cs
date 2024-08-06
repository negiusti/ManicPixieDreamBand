using UnityEngine;

public class TrunkDoor : MonoBehaviour
{
    private Animator anim;

    private CarPackingMiniGame mg;

    private void Start()
    {
        mg = GetComponentInParent<CarPackingMiniGame>();
        anim = GetComponent<Animator>();
    }

    public void CloseTrunk()
    {
        anim.SetBool("HasWon", true);
    }

    public void CloseCarPacking()
    {
        mg.Fade();
        //mg.CloseMiniGame();
    }
}

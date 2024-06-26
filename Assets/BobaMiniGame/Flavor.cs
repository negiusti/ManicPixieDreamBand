using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flavor : MonoBehaviour
{
    private Animator animator;
    public GameObject liquid;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;
    private BobaMiniGame mg;
    public Color color;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(11.5f, origialPos.y, origialPos.z);
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
    }

    void Update()
    {


    }

    private void OnMouseDown()
    {
        if (mg.flavorDone)
            return;
        mg.flavorDone = true;
        StartCoroutine(lerp.Lerp(targetPos, 0.5f));
        animator.Play("FlavorPour");
        StartCoroutine(liquid.GetComponent<LerpPosition>().LerpColor(new Vector3(color.r, color.g, color.b), animator.runtimeAnimatorController.animationClips.First(x => x.name == "FlavorPour").length));
        mg.Next();
    }
}

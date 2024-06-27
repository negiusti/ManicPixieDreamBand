using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Flavor : MonoBehaviour
{
    private Animator animator;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;
    private BobaMiniGame mg;
    private SpriteRenderer sr;
    private SpriteResolver resolver;
    public Color color;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(2.92f, 2.38f, origialPos.z);
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
        sr = GetComponent<SpriteRenderer>();
        resolver = GetComponent<SpriteResolver>();
    }

    void Update()
    {


    }

    public void ResetPosition()
    {
        if (mg == null)
            Start();
        transform.localPosition = origialPos;
        resolver.SetCategoryAndLabel(resolver.GetCategory(), "Entry");
        resolver.ResolveSpriteToSpriteRenderer();
        sr.sortingOrder = 50;
    }

    private void OnMouseDown()
    {
        if (mg.flavorDone)
            return;
        mg.flavorDone = true;
        GetComponent<Renderer>().sortingOrder = 51;
        StartCoroutine(lerp.Lerp(targetPos, 0.5f));
        animator.Play("FlavorPour", -1, 0f);
        StartCoroutine(mg.cup.liquid.GetComponent<LerpPosition>().LerpColor(new Vector3(color.r, color.g, color.b), animator.runtimeAnimatorController.animationClips.First(x => x.name == "FlavorPour").length));
        mg.Next(gameObject.name);
    }
}

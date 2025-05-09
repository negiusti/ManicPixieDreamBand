using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class Flavor : MonoBehaviour, IPointerDownHandler
{
    private Animator animator;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;
    private BobaMiniGame mg;
    private SpriteRenderer sr;
    private SpriteResolver resolver;
    public Color color;
    private AudioSource audioSource;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(2.92f, 1.25f, origialPos.z);
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
        sr = GetComponent<SpriteRenderer>();
        resolver = GetComponent<SpriteResolver>();
        audioSource = GetComponentInParent<AudioSource>();
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
        animator.Play("FlavorIdle", -1, 0f);
    }

    private void OnMouseDown()
    {
        if (mg.flavorDone || mg.CurrentStep() != BobaMiniGame.Step.Flavor)
            return;
        audioSource.Play();
        mg.flavorDone = true;
        GetComponent<Renderer>().sortingOrder = 51;
        StartCoroutine(lerp.Lerp(targetPos, 0.2f));
        animator.Play("FlavorPour", -1, 0f);
        StartCoroutine(mg.cup.liquid.GetComponent<LerpPosition>().LerpColor(new Vector3(color.r, color.g, color.b), animator.runtimeAnimatorController.animationClips.First(x => x.name == "FlavorPour").length));
        mg.Next(gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

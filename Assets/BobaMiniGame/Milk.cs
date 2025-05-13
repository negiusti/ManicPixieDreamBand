using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Milk : MonoBehaviour, IPointerDownHandler
{
    private Animator animator;
    public float rotationAngle = 25f;
    public float rotationSpeed = 5f;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;
    private BobaMiniGame mg;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(10f, origialPos.y, origialPos.z);
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponentInParent<AudioSource>(true);
    }

    void Update()
    {
        
    }

    public void ResetPosition()
    {
        if (mg == null)
            Start();
        transform.localPosition = origialPos;
        animator.Play("Idle");
    }

    private void OnMouseDown()
    {
        if (mg.milkDone || mg.CurrentStep() != BobaMiniGame.Step.Milk)
            return;
        mg.milkDone = true;
        sr.sortingOrder = 10;
        StartCoroutine(lerp.Lerp(targetPos, 0.5f));
        animator.Play("Pour");
        audioSource.Play();
        StartCoroutine(mg.cup.liquid.GetComponent<LerpPosition>().Lerp(mg.cup.liquid.transform.localPosition + Vector3.up * 4f, animator.runtimeAnimatorController.animationClips.First(x => x.name == "Pour").length));
        mg.Next(gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }

}

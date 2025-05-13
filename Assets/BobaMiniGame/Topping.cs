using UnityEngine;
using UnityEngine.EventSystems;

public class Topping : MonoBehaviour, IPointerDownHandler
{
    private Animator animator;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;
    private BobaMiniGame mg;
    private AudioSource audioSource;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(11.5f, origialPos.y, origialPos.z);
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
        audioSource = GetComponentInParent<AudioSource>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPosition()
    {
        if (mg == null)
            Start();
        transform.localPosition = origialPos;
        animator.Play("ToppingsIdle");
    }

    private void OnMouseDown()
    {
        if (mg.toppingsDone || mg.CurrentStep() != BobaMiniGame.Step.Toppings)
            return;
        audioSource.Play();
        mg.toppingsDone = true;
        mg.cup.GetComponentInChildren<Toppings>().SetToppingType(gameObject.name);
        StartCoroutine(lerp.Lerp(targetPos, 0.5f));
        animator.Play("ToppingsPour");
        mg.cup.GetComponentInChildren<Toppings>().AppearInCup();
        mg.Next(gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

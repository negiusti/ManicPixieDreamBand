using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MapsApp.Location location;
    public SpriteRenderer pin;
    public SpriteRenderer icon;
    private TextMeshPro tm;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private MapsApp app;
    private bool alreadyHere;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        tm = this.GetComponent<TextMeshPro>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        originalScale = spriteRenderer.gameObject.transform.localScale;
        app = this.GetComponentInParent<MapsApp>();
        alreadyHere = false;
        //pin.enabled = false;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (animator == null)
            return;
        UnHover();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHere()
    {
        alreadyHere = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        Hover();
    }

    public void SetNotHere()
    {
        alreadyHere = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public void Hover()
    {
        animator.SetBool("Hovered", true);
        Vector3 newScale = originalScale + Vector3.one * 5f;
        spriteRenderer.gameObject.transform.localScale = newScale;
        //tm.enabled = true;
        //pin.enabled = true;
        icon.color = new Color(1f, 1f, 1f, 1f);
        Debug.Log("hovering pin: " + location.ToString());
    }

    private void OnMouseEnter()
    {
        Hover();
    }

    public void UnHover()
    {
        if (alreadyHere)
            return;
        animator.SetBool("Hovered", false);
        //if (tm!= null)
        //    tm.enabled = false;
        if (spriteRenderer != null)
            spriteRenderer.gameObject.transform.localScale = originalScale;
        //if (pin != null)
        //    pin.enabled = false;
        icon.color = new Color(1f, 1f, 1f, 0.7f);
    }

    private void OnMouseExit()
    {
        UnHover();
    }

    public void Click()
    {
        Debug.Log("hovering pin: " + location.ToString());
        app.SetLocation(location);
        app.OpenPin(alreadyHere);
    }

    private void OnMouseDown()
    {
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

}

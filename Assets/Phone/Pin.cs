using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MapsApp.Location location;
    private TextMeshPro tm;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private MapsApp app;
    private bool alreadyHere;

    // Start is called before the first frame update
    void Start()
    {
        tm = this.GetComponent<TextMeshPro>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        originalScale = this.spriteRenderer.gameObject.transform.localScale;
        app = this.GetComponentInParent<MapsApp>();
        alreadyHere = false;
    }

    private void OnEnable()
    {
        UnHover();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHere()
    {
        alreadyHere = true;
        this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void SetNotHere()
    {
        alreadyHere = false;
        this.spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public void Hover()
    {
        Vector3 newScale = originalScale + Vector3.one * 5f;
        this.spriteRenderer.gameObject.transform.localScale = newScale;
        this.tm.enabled = true;
        Debug.Log("hovering pin: " + location.ToString());
    }

    private void OnMouseEnter()
    {
        Hover();
    }

    public void UnHover()
    {
        if (tm!= null)
            tm.enabled = false;
        if (spriteRenderer != null)
            spriteRenderer.gameObject.transform.localScale = originalScale;
    }

    private void OnMouseExit()
    {
        UnHover();
    }

    public void Click()
    {
        if (alreadyHere)
            return;
        Debug.Log("hovering pin: " + location.ToString());
        app.SetLocation(location);
        app.OpenPin();
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

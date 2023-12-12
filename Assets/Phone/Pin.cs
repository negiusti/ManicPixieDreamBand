using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public string LocationName;
    private TextMeshPro tm;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private MapsApp app;

    // Start is called before the first frame update
    void Start()
    {
        tm = this.GetComponent<TextMeshPro>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        originalScale = this.spriteRenderer.gameObject.transform.localScale;
        app = this.GetComponentInParent<MapsApp>();
        Debug.Log("hi");
    }

    private void OnEnable()
    {
        UnHover();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hover()
    {
        Vector3 newScale = originalScale + Vector3.one * 5f;
        this.spriteRenderer.gameObject.transform.localScale = newScale;
        this.tm.enabled = true;
        Debug.Log("hovering pin: " + LocationName);
    }

    void OnMouseEnter()
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

    void OnMouseExit()
    {
        UnHover();
    }

    public void Click()
    {
        Debug.Log("hovering pin: " + LocationName);
        app.SetLocation(LocationName);
        app.OpenPin();
    }

    void OnMouseDown()
    {
        Click();
    }
}

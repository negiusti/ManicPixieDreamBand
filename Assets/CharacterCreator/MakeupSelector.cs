using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeupSelector : MonoBehaviour
{
    public string categoryName;
    public CharacterEditor characterEditor;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;
    private MakeupSelector[] selectors;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        originalScale = this.spriteRenderer.gameObject.transform.localScale;
        selectors = FindObjectsOfType<MakeupSelector>();
        //spriteRenderer.material.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        characterEditor.SetCurrentFaceCategory(categoryName);
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = originalScale + Vector3.one * 0.2f;
        this.spriteRenderer.gameObject.transform.localScale = newScale;
    }

    private void OnMouseExit()
    {
        spriteRenderer.gameObject.transform.localScale = originalScale;
    }

    public void UnSelect()
    {

    }
}

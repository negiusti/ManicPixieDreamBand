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
    public Material defaultMat;
    public Material outlineMat;
    private bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        originalScale = this.spriteRenderer.gameObject.transform.localScale;
        selectors = this.transform.parent.gameObject.GetComponentsInChildren<MakeupSelector>();
        spriteRenderer.material = defaultMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Select();
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
        spriteRenderer.material = defaultMat;
    }

    private void UnSelectAll()
    {
        foreach (MakeupSelector selector in selectors)
        {
            selector.UnSelect();
        }
    }

    public void Select()
    {
        UnSelectAll();
        characterEditor.SetCurrentFaceCategory(categoryName);
        spriteRenderer.material = outlineMat;
    }
}

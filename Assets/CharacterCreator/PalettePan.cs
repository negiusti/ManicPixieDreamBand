﻿using UnityEngine;
using UnityEngine.UI;

public class PalettePan : MonoBehaviour
{
    private ColorPalette palette;
    private Image image;
    private PalettePan[] pans;
    private bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        palette = this.GetComponentInParent<ColorPalette>();
        pans = palette.gameObject.GetComponentsInChildren<PalettePan>();
    }

    private void OnDisable()
    {
        Unselect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetColor(Color c)
    {
        image.color = c;
    }

    public Color GetColor()
    {
        return image.color;
    }

    private void OnMouseDown()
    {
        palette.SetColor(image.color);
        Select();
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = Vector3.one * 1.1f;
        this.gameObject.transform.localScale = newScale;
    }

    private void UnselectAll()
    {
        foreach (PalettePan p in pans)
        {
            p.Unselect();
        }
    }

    public void Unselect()
    {
        isSelected = false;
        this.gameObject.transform.localScale = Vector3.one;
    }

    public void Select()
    {
        UnselectAll();
        isSelected = true;
        Vector3 newScale = Vector3.one * 1.1f;
        this.gameObject.transform.localScale = newScale;
    }

    private void OnMouseExit()
    {
        if (!isSelected)
            this.gameObject.transform.localScale = Vector3.one;
    }
}

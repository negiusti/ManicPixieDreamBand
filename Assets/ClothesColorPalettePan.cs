using UnityEngine;
using UnityEngine.UI;

public class ClothesColorPalettePan : MonoBehaviour
{
    private ClothesColorPalette palette;
    private Image image;
    private ClothesColorPalettePan[] pans;
    public SpriteRenderer[] srs;
    private bool isSelected;
    private Vector3 startingScale;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        palette = GetComponentInParent<ClothesColorPalette>();
        pans = palette.gameObject.GetComponentsInChildren<ClothesColorPalettePan>();
        startingScale = Vector3.one;
        transform.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        //Unselect();
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
        foreach (SpriteRenderer s in srs)
            s.color = image.color;
        Select();
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = startingScale * 1.1f;
        this.gameObject.transform.localScale = newScale;
    }

    private void UnselectAll()
    {
        foreach (ClothesColorPalettePan p in pans)
        {
            p.Unselect();
        }
    }

    public void Unselect()
    {
        isSelected = false;
        this.gameObject.transform.localScale = startingScale;
    }

    public void Select()
    {
        UnselectAll();
        isSelected = true;
        Vector3 newScale = startingScale * 1.1f;
        this.gameObject.transform.localScale = newScale;
    }

    private void OnMouseExit()
    {
        if (!isSelected)
            this.gameObject.transform.localScale = startingScale;
    }
}

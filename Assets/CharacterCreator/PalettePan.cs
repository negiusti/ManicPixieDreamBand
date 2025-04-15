using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PalettePan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private ColorPalette palette;
    private Image image;
    private PalettePan[] pans;
    private bool isSelected;
    private Vector3 startingScale;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        palette = this.GetComponentInParent<ColorPalette>();
        pans = palette.gameObject.GetComponentsInChildren<PalettePan>();
        startingScale = Vector3.one;
        transform.localScale = Vector3.one;
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
        Vector3 newScale = startingScale * 1.1f;
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class BiggerWhenHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector3 originalScale;
    public float scaleFactor;
    private RectTransform rect;
    private bool hovered;

    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
        originalScale = rect == null ? this.gameObject.transform.localScale : rect.localScale;
        if (scaleFactor < 1f)
        {
            scaleFactor = 1.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseExit()
    {
        if (rect == null)
            this.gameObject.transform.localScale = originalScale;
        else
            rect.localScale = originalScale;

        hovered = false;
    }

    private void OnMouseDown()
    {
        if (rect == null)
            this.gameObject.transform.localScale = originalScale;
        else
            rect.localScale = originalScale;
    }

    private void OnMouseDrag()
    {
        if (rect == null)
            this.gameObject.transform.localScale = originalScale;
        else
            rect.localScale = originalScale;
    }

    private void OnMouseUp()
    {
        if (hovered)
        {
            Vector3 newScale = originalScale * scaleFactor;
            if (rect == null)
                this.gameObject.transform.localScale = newScale;
            else
                rect.localScale = newScale;
        }
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = originalScale * scaleFactor;
        if (rect == null)
            this.gameObject.transform.localScale = newScale;
        else
            rect.localScale = newScale;

        hovered = true;
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

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp();
    }
}

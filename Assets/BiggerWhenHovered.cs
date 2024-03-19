using UnityEngine;
using UnityEngine.EventSystems;

public class BiggerWhenHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor;
    private RectTransform rect;

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
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = originalScale * scaleFactor;
        if (rect == null)
            this.gameObject.transform.localScale = newScale;
        else
            rect.localScale = newScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }
}

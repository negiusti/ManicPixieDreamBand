using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class BiggerWhenHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector3 originalScale;
    public float scaleFactor;
    private RectTransform rect;
    private bool hovered;
    private bool started;

    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
        originalScale = rect == null ? this.gameObject.transform.localScale : rect.localScale;

        if (scaleFactor < 1f)
        {
            scaleFactor = 1.1f;
        }
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (started)
            OnMouseExit();
    }

    private void OnMouseExit()
    {
        if (DialogueManager.IsConversationActive && !MiniGameManager.AnyActiveMiniGames())
            return;
        if (rect == null)
            this.gameObject.transform.localScale = originalScale;
        else
            rect.localScale = originalScale;

        hovered = false;
    }

    private void OnMouseDown()
    {
        if (DialogueManager.IsConversationActive && !MiniGameManager.AnyActiveMiniGames())
            return;
        if (rect == null)
            this.gameObject.transform.localScale = originalScale;
        else
            rect.localScale = originalScale;
    }

    private void OnMouseDrag()
    {
        if (DialogueManager.IsConversationActive && !MiniGameManager.AnyActiveMiniGames())
            return;
        if (rect == null)
            this.gameObject.transform.localScale = originalScale;
        else
            rect.localScale = originalScale;
    }

    private void OnMouseUp()
    {
        if (DialogueManager.IsConversationActive && !MiniGameManager.AnyActiveMiniGames())
            return;
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
        if (DialogueManager.IsConversationActive && !MiniGameManager.AnyActiveMiniGames())
            return;
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

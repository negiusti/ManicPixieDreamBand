using UnityEngine;
using UnityEngine.EventSystems;

public class MakeupSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public string categoryName;
    private CharacterEditor characterEditor;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        characterEditor = GameObject.FindObjectOfType<CharacterEditor>();
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
        //Vector3 newScale = originalScale * 1.1f;
        //transform.localScale = newScale;
    }

    private void OnMouseExit()
    {
        //transform.localScale = originalScale;
    }

    public void Select()
    {
        characterEditor.SetCurrentFaceCategory(categoryName);
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

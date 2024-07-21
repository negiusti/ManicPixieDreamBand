using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneColorPalettePan : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("CHANGE COLOR to: " + image.color.r + " " + image.color.g + " " + image.color.b); ;
        Phone.Instance.ChangePhoneColor(image.color);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CHANGE COLOR to: " + image.color.r + " " + image.color.g + " " + image.color.b); ;
        Phone.Instance.ChangePhoneColor(image.color);
    }
}

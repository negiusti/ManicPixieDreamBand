using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class GiftableItem : MonoBehaviour, IPointerDownHandler
{
    public SpriteResolver itemIcon;
    public SpriteRenderer itemRen;
    private Image slotImg;
    public Image itemImg;
    private string itemName;

    // Start is called before the first frame update
    void Start()
    {
        slotImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static string AddSpaceBeforeCapitals(string input)
    {
        // Regex pattern: Look for a capital letter that is preceded by a non-space character (except at the start of the string)
        return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }

    public void SetItemIcon(string itemName)
    {
        if (itemRen == null)
            Start();
        // add space before capital letters
        this.itemName = AddSpaceBeforeCapitals(itemName);
        itemIcon.SetCategoryAndLabel("Item", this.itemName);
        itemIcon.ResolveSpriteToSpriteRenderer();
        itemImg.sprite = itemRen.sprite;
    }

    private void OnMouseDown()
    {
        InventoryManager.RemovePerishableItem(itemName);
        // close window
        // NPC reaction
    }

    private void OnEnable()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

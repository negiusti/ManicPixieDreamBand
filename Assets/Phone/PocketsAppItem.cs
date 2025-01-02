using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PocketsAppItem : MonoBehaviour, IPointerDownHandler
{
    public SpriteResolver itemIcon;
    private SpriteResolver slotIcon;
    public SpriteRenderer itemRen;
    private SpriteRenderer slotRen;
    private Image slotImg;
    public Image itemImg;
    private string itemName;

    private HashSet<string> edibles = new HashSet<string> { "Coffee", "Boba", "Root Beer", "Bastani", "Balal", "Pan De Ube", "Turon", "Croissant", "Pizza" };

    // Start is called before the first frame update
    void Start()
    {
        slotIcon = GetComponent<SpriteResolver>();
        slotImg = GetComponent<Image>();
        slotRen = GetComponent<SpriteRenderer>();
        // randomize slot icon
        slotIcon.SetCategoryAndLabel("Slot", Random.Range(0, 3).ToString());
        slotIcon.ResolveSpriteToSpriteRenderer();
        slotImg.sprite = slotRen.sprite;
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

    private bool isEdible()
    {
        return edibles.Contains(itemName);
    }

    private void OnMouseDown()
    {
        Debug.Log("Drinking... " + itemName);
        if(isEdible())
        {
            Characters.MainCharacter().GetComponent<Movement>().Drink(itemName);
            InventoryManager.RemovePerishableItem(itemName);
            Phone.Instance.Lock();
        }
    }

    private void OnEnable()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

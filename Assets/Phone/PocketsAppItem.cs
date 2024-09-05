using System.Collections.Generic;
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

    private HashSet<string> drinkables = new HashSet<string> { "Coffee", "Boba" };

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

    public void SetItemIcon(string itemName)
    {
        if (itemRen == null)
            Start();
        this.itemName = itemName;
        itemIcon.SetCategoryAndLabel("Item", itemName);
        itemIcon.ResolveSpriteToSpriteRenderer();
        itemImg.sprite = itemRen.sprite;
    }

    private bool isDrinkable()
    {
        return drinkables.Contains(itemName);
    }

    private void OnMouseDown()
    {
        Debug.Log("Drinking... " + itemName);
        if(isDrinkable())
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

using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PocketsAppItem : MonoBehaviour
{
    private SpriteResolver itemIcon;
    private SpriteResolver slotIcon;
    private SpriteRenderer itemRen;
    private SpriteRenderer slotRen;
    private Image slotImg;
    private Image itemImg;


    // Start is called before the first frame update
    void Start()
    {
        itemIcon = GetComponentInChildren<SpriteResolver>(includeInactive: true);
        itemImg = GetComponentInChildren<Image>(includeInactive: true);
        itemRen = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
        slotIcon = GetComponent<SpriteResolver>();
        slotImg = GetComponent<Image>();
        slotRen = GetComponent<SpriteRenderer>();
        // randomize slot icon
        slotIcon.SetCategoryAndLabel("Slot", Random.Range(0, 3).ToString());
        slotIcon.ResolveSpriteToSpriteRenderer();
        //itemRen.enabled = true;
        //itemImg.sprite = itemRen.sprite;
        //itemRen.enabled = false;
        slotImg.sprite = slotRen.sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemIcon(string itemName)
    {
        Debug.Log("SetItemIcon: " + itemName);
        if (itemRen == null)
            Start();
        itemIcon.SetCategoryAndLabel("Item", itemName);
        itemRen.enabled = true;
        itemIcon.ResolveSpriteToSpriteRenderer();
        itemImg.sprite = itemRen.sprite;
        itemRen.enabled = false;
        Debug.Log("SetItemIcon: " + itemRen.sprite.name);
        Debug.Log("SetItemIcon: " + itemImg.sprite.name);
    }

    private void OnEnable()
    {
    }
}

using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PocketsAppItem : MonoBehaviour
{
    public SpriteResolver itemIcon;
    private SpriteResolver slotIcon;
    public SpriteRenderer itemRen;
    private SpriteRenderer slotRen;
    private Image slotImg;
    public Image itemImg;


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
        itemIcon.SetCategoryAndLabel("Item", itemName);
        itemIcon.ResolveSpriteToSpriteRenderer();
        itemImg.sprite = itemRen.sprite;
    }

    private void OnEnable()
    {
    }
}

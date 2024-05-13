using UnityEngine;
using UnityEngine.U2D.Animation;

public class PocketsAppItem : MonoBehaviour
{
    private SpriteResolver itemIcon;
    private SpriteResolver slotIcon;

    // Start is called before the first frame update
    void Start()
    {
        itemIcon = this.GetComponentInChildren<SpriteResolver>(true);
        slotIcon = this.GetComponent<SpriteResolver>();
        // randomize slot icon
        slotIcon.SetCategoryAndLabel("Slot", Random.Range(0, 3).ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemIcon(string itemName)
    {
        itemIcon = this.GetComponentInChildren<SpriteResolver>(true);
        itemIcon.SetCategoryAndLabel("Item", itemName);
    }

    private void OnEnable()
    {
    }
}

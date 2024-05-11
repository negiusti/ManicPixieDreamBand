using UnityEngine;
using UnityEngine.U2D.Animation;

public class PocketsAppItem : MonoBehaviour
{
    private SpriteResolver itemIcon;
    private SpriteResolver slotIcon;
    //public TextMeshPro tmptmp;
    // Start is called before the first frame update
    void Start()
    {
        itemIcon = this.GetComponentInChildren<SpriteResolver>(true);
        slotIcon = this.GetComponent<SpriteResolver>();
        //tmptmp = this.GetComponentInChildren<TextMeshPro>(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemIcon(string itemName)
    {
        // TO DO after yaz makes icon art
        itemIcon.SetCategoryAndLabel("Item", itemName);
        //tmptmp.text ="";
    }

    private void OnEnable()
    {
        slotIcon = this.GetComponent<SpriteResolver>();
        // randomize slot icon
        slotIcon.SetCategoryAndLabel("Slot", Random.Range(0, 3).ToString());
    }
}

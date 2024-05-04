using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PocketsAppItem : MonoBehaviour
{
    private SpriteResolver itemIcon;
    private SpriteResolver slotIcon;
    private TextMeshPro tmptmp;
    // Start is called before the first frame update
    void Start()
    {
        itemIcon = this.GetComponentInChildren<SpriteResolver>();
        slotIcon = this.GetComponent<SpriteResolver>();
        tmptmp = this.GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemIcon(string itemName)
    {
        // TO DO after yaz makes icon art
        //itemIcon.SetCategoryAndLabel("Item", itemName);
        tmptmp.text = itemName;
    }

    private void OnEnable()
    {
        if (slotIcon == null)
            slotIcon = this.GetComponent<SpriteResolver>();
        // randomize slot icon
        slotIcon.SetCategoryAndLabel("Slot", Random.Range(0, 3).ToString());
    }
}

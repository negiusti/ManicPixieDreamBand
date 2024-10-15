using System.Collections.Generic;
using UnityEngine;

public class GiftingInventory : PhoneApp
{
    public GiftableItem itemTemplate;
    private Dictionary<InventoryManager.PerishableItem, int> perishableItems;
    private List<GiftableItem> itemIcons;
    public string recevingNPC;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        itemIcons = new List<GiftableItem>();
        perishableItems = InventoryManager.GetGiftablePocketItems();
        foreach (KeyValuePair<InventoryManager.PerishableItem, int> itemAndCount in perishableItems)
        {
            for (int i = 0; i < itemAndCount.Value; i++)
            {
                GiftableItem item = Instantiate(itemTemplate, itemTemplate.gameObject.transform.parent);
                item.gameObject.SetActive(true);
                item.SetItemIcon(itemAndCount.Key.ToString());
                itemIcons.Add(item);
            }
        }
    }

    private void OnDisable()
    {
        foreach (GiftableItem i in itemIcons)
        {
            Destroy(i.gameObject);
        }
        itemIcons.Clear();
    }

    public override void Save()
    {
        // Nothing to do here
    }

    public override void Load()
    {
        // Nothing to do here
    }
}

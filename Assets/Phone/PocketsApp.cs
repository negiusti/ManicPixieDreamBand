using System.Collections.Generic;
using UnityEngine;

public class PocketsApp : PhoneApp
{
    public PocketsAppItem itemTemplate;
    private Dictionary<InventoryManager.PerishableItem, int> perishableItems;
    private Dictionary<InventoryManager.Item, int> items;
    private List<PocketsAppItem> itemIcons;

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
        if (Phone.Instance == null)
            return;
        Phone.Instance.ClearNotificationFor("Pockets");
        phoneIcon.HideNotificationIndicator();
        itemIcons = new List<PocketsAppItem>();
        items = InventoryManager.GetPocketItems();
        perishableItems = InventoryManager.GetPerishablePocketItems();
        foreach (KeyValuePair<InventoryManager.Item, int> itemAndCount in items) {
            for (int i = 0; i < itemAndCount.Value; i++)
            {
                PocketsAppItem item = Instantiate(itemTemplate, itemTemplate.gameObject.transform.parent);
                item.gameObject.SetActive(true);
                item.SetItemIcon(itemAndCount.Key.ToString());
                itemIcons.Add(item);
            }
        }

        foreach (KeyValuePair<InventoryManager.PerishableItem, int> itemAndCount in perishableItems)
        {
            for (int i = 0; i < itemAndCount.Value; i++)
            {
                PocketsAppItem item = Instantiate(itemTemplate, itemTemplate.gameObject.transform.parent);
                item.gameObject.SetActive(true);
                item.SetItemIcon(itemAndCount.Key.ToString());
                itemIcons.Add(item);
            }
        }
    }

    private void OnDisable()
    {
        foreach (PocketsAppItem i in itemIcons)
        {
            Destroy(i.gameObject);
        }
        itemIcons.Clear();
    }
}

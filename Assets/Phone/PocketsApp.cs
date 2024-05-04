using System.Collections.Generic;
using UnityEngine;

public class PocketsApp : MonoBehaviour
{
    public PocketsAppItem itemTemplate;
    private Dictionary<InventoryManager.PerishableItem, int> perishableItems;
    private Dictionary<InventoryManager.Item, int> items;
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
        items = InventoryManager.GetPocketItems();
        perishableItems = InventoryManager.GetPerishablePocketItems();
        foreach (KeyValuePair<InventoryManager.Item, int> itemAndCount in items) {
            for (int i = 0; i < itemAndCount.Value; i++)
            {
                PocketsAppItem item = Instantiate(itemTemplate, transform);
                item.gameObject.SetActive(true);
                item.SetItemIcon(itemAndCount.Key.ToString());
            }
        }

        foreach (KeyValuePair<InventoryManager.PerishableItem, int> itemAndCount in perishableItems)
        {
            for (int i = 0; i < itemAndCount.Value; i++)
            {
                PocketsAppItem item = Instantiate(itemTemplate, transform);
                item.gameObject.SetActive(true);
                item.SetItemIcon(itemAndCount.Key.ToString());
            }
        }
    }
}

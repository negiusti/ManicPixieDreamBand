using UnityEngine;

public class ItemSwapPhoneUI : MonoBehaviour
{
    private ItemSwapIcon icon;
    private Furniture furniture;
    private Gear gear;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentInChildren<ItemSwapIcon>(includeInactive:true);
    }

    public void AssignItem(Furniture f)
    {
        if (icon == null)
            Start();
        furniture = f;
        icon.AssignItem(f.Category(), f.Label());
    }

    public void AssignItem(Gear g)
    {
        if (icon == null)
            Start();
        gear = g;
        icon.AssignItem(g.Category(), g.Label());
    }

    public void SwapFurniture(int delta)
    {
        furniture.Change(delta);
        icon.UpdateIcon(furniture.Label());
    }

    public void SwapGear(int delta)
    {
        gear.Change(delta);
        icon.UpdateIcon(gear.Label());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

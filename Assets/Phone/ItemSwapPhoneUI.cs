using UnityEngine;

public class ItemSwapPhoneUI : MonoBehaviour
{
    private ItemSwapIcon icon;
    private Furniture furniture;
    private Gear gear;
    public Gear defaultGear;

    public string Category()
    {
        if (furniture != null)
            return furniture.Category();
        if (gear != null)
            return gear.Category();
        if (defaultGear != null)
            return defaultGear.Category();
        return "None";
    }

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentInChildren<ItemSwapIcon>(includeInactive:true);
    }

    public void UseDefaultGear()
    {
        if (icon == null)
            Start();
        defaultGear.gameObject.SetActive(true);
        gear = defaultGear;
        icon.AssignItem(gear.Category(), gear.Label());
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
        defaultGear.gameObject.SetActive(false);
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

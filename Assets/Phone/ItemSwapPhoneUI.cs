using UnityEngine;

public class ItemSwapPhoneUI : MonoBehaviour
{
    private ItemSwapIcon icon;
    private Furniture furniture;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentInChildren<ItemSwapIcon>(includeInactive:true);
    }

    public void AssignFurniture(Furniture f)
    {
        if (icon == null)
            Start();
        furniture = f;
        icon.AssignFurniture(f.Category(), f.Label());
    }

    public void SwapFurniture(int delta)
    {
        furniture.Change(delta);
        icon.UpdateIcon(furniture.Label());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

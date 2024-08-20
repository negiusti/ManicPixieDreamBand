using System.Collections.Generic;

public class GearApp : PhoneApp
{
    private HashSet<string> gearCategoryNotifications;

    // Start is called before the first frame update
    void Start()
    {
        gearCategoryNotifications = new HashSet<string>();
    }

    public void Reset()
    {
        if (gearCategoryNotifications != null)
            gearCategoryNotifications.Clear();
    }

    public void ClearNotification(string category)
    {
        if (gearCategoryNotifications == null)
            Start();
        gearCategoryNotifications.Remove(category);
        gearCategoryNotifications.Remove(InventoryManager.GetInventoryCategory(category));
    }

    public void AddNotification(string category)
    {
        if (gearCategoryNotifications == null)
            Start();
        gearCategoryNotifications.Add(category);
        gearCategoryNotifications.Add(InventoryManager.GetInventoryCategory(category));
    }

    public bool HasNotification(string category)
    {
        if (gearCategoryNotifications == null)
            Start();
        return gearCategoryNotifications.Contains(category) || gearCategoryNotifications.Contains(InventoryManager.GetInventoryCategory(category));
    }
}

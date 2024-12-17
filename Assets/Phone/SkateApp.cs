using System.Collections.Generic;

public class SkateApp : PhoneApp
{
    private HashSet<string> skateCategoryNotifications;

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    public void Reset()
    {
        if (skateCategoryNotifications != null)
            skateCategoryNotifications.Clear();
    }

    public void ClearNotification(string category)
    {
        if (skateCategoryNotifications == null)
            Start();
        skateCategoryNotifications.Remove(category);
        skateCategoryNotifications.Remove(InventoryManager.GetInventoryCategory(category));
    }

    public void AddNotification(string category)
    {
        if (skateCategoryNotifications == null)
            Start();
        skateCategoryNotifications.Add(category);
        skateCategoryNotifications.Add(InventoryManager.GetInventoryCategory(category));
    }

    public bool HasNotification(string category)
    {
        if (skateCategoryNotifications == null)
            Start();
        return skateCategoryNotifications.Contains(category) || skateCategoryNotifications.Contains(InventoryManager.GetInventoryCategory(category));
    }

    public override void Save()
    {
        ES3.Save("SkateCategoryNotif", skateCategoryNotifications == null ? new HashSet<string>() : skateCategoryNotifications);
    }

    public override void Load()
    {
        skateCategoryNotifications = ES3.Load("SkateCategoryNotif", new HashSet<string>());
    }
}

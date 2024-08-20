using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DecoratorApp : PhoneApp
{
    public ItemSwapPhoneUI itemSwapTemplate;
    public Transform container;
    private HashSet<Furniture> furniture;
    private Camera cam;
    private TextMeshPro tmp;
    public GameObject RoomPreview;
    private HashSet<string> furnitureCategoryNotifications;
    private ScrollRect scrollView;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        cam = GetComponentInChildren<Camera>();
        tmp = GetComponentInChildren<TextMeshPro>();
        if (furnitureCategoryNotifications == null)
            furnitureCategoryNotifications = new HashSet<string>();
        scrollView = GetComponentInChildren<ScrollRect>(includeInactive: true);
        Refresh();
    }

    public void Reset()
    {
        if (furnitureCategoryNotifications != null)
            furnitureCategoryNotifications.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.isActiveAndEnabled)
            cam.transform.SetPositionAndRotation(new Vector3(0f, 0f, 10f), Quaternion.identity);
    }

    void ScrollToTopOfScrollView()
    {
        if (scrollView != null && scrollView.verticalScrollbar != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.content);
            scrollView.verticalNormalizedPosition = Mathf.Clamp(scrollView.verticalNormalizedPosition, 1f, 1f);
            scrollView.verticalScrollbar.value = 1f;
            scrollView.verticalNormalizedPosition = 1f;
        }
    }

    private void OnEnable()
    {
        if (Phone.Instance == null)
            return;
        if (SceneChanger.Instance.GetCurrentScene().Equals("Bedroom"))
            Phone.Instance.ClearNotificationFor("Decorator");
        ScrollToTopOfScrollView();
    }

    public void ClearNotification(string category)
    {
        Debug.Log("REMOVE NOTIF FOR: " + category);
        Debug.Log("REMOVE NOTIF FOR: " + InventoryManager.GetInventoryCategory(category));
        if (furnitureCategoryNotifications == null)
            Start();
        furnitureCategoryNotifications.Remove(category);
        furnitureCategoryNotifications.Remove(InventoryManager.GetInventoryCategory(category));
    }

    public void AddNotification(string category)
    {
        Debug.Log("ADD NOTIF FOR: " + category);
        Debug.Log("ADD NOTIF FOR: " + InventoryManager.GetInventoryCategory(category));
        if (furnitureCategoryNotifications == null)
            Start();
        furnitureCategoryNotifications.Add(category);
        furnitureCategoryNotifications.Add(InventoryManager.GetInventoryCategory(category));
    }

    public bool HasNotification(string category)
    {
        Debug.Log("HAS NOTIF FOR: " + category + furnitureCategoryNotifications.Contains(category));
        Debug.Log("HAS NOTIF FOR: " + InventoryManager.GetInventoryCategory(category) + furnitureCategoryNotifications.Contains(InventoryManager.GetInventoryCategory(category)));
        if (furnitureCategoryNotifications == null)
            Start();
        return furnitureCategoryNotifications.Contains(category) || furnitureCategoryNotifications.Contains(InventoryManager.GetInventoryCategory(category));
    }

    private void Refresh()
    {
        for (int i = 1; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
        furniture?.Clear();
        FindEditableItems();

        HashSet<Furniture> withNotifs = furniture.Where(f => HasNotification(f.Category())).ToHashSet();
        HashSet<Furniture> withoutNotifs = furniture.Where(f => !HasNotification(f.Category())).ToHashSet();
        foreach (Furniture f in withNotifs)
        {
            ItemSwapPhoneUI itemSwap = Instantiate(itemSwapTemplate, container);
            itemSwap.gameObject.SetActive(true);
            itemSwap.AssignItem(f, true);
        }
        foreach (Furniture f in withoutNotifs)
        {
            ItemSwapPhoneUI itemSwap = Instantiate(itemSwapTemplate, container);
            itemSwap.gameObject.SetActive(true);
            itemSwap.AssignItem(f, false);
        }
        if (furniture.Count == 0)
        {
            tmp.enabled = true;
            cam.enabled = false;
            RoomPreview.SetActive(false);
        }
        else
        {
            tmp.enabled = false;
            cam.enabled = true;
            RoomPreview.SetActive(true);
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        Refresh();
    }

    private void FindEditableItems()
    {
        furniture = new HashSet<Furniture>(FindObjectsOfType<Furniture>());
    }
}

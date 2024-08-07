using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecoratorApp : PhoneApp
{
    public ItemSwapPhoneUI itemSwapTemplate;
    public Transform container;
    private HashSet<Furniture> furniture;
    private Camera cam;
    private TextMeshPro tmp;
    public GameObject RoomPreview;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        cam = GetComponentInChildren<Camera>();
        tmp = GetComponentInChildren<TextMeshPro>();
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.isActiveAndEnabled)
            cam.transform.SetPositionAndRotation(new Vector3(0f, 0f, 10f), Quaternion.identity);
    }

    private void OnEnable()
    {
        if (Phone.Instance == null)
            return;
        if (SceneChanger.Instance.GetCurrentScene().Equals("Bedroom"))
            Phone.Instance.ClearNotificationFor("Decorator");
    }

    private void Refresh()
    {
        for (int i = 1; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
        furniture?.Clear();
        FindEditableItems();
        foreach (Furniture f in furniture)
        {
            ItemSwapPhoneUI itemSwap = Instantiate(itemSwapTemplate, container);
            itemSwap.gameObject.SetActive(true);
            itemSwap.AssignItem(f);
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

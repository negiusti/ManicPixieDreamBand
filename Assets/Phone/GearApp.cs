using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GearApp : MonoBehaviour
{
    public ItemSwapPhoneUI itemSwapTemplate;
    public Transform container;
    private HashSet<Gear> gear;
    private Camera cam;
    public GameObject RoomPreview;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += ChangedActiveScene;
        cam = GetComponentInChildren<Camera>();
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
    }

    private void Refresh()
    {
        for (int i = 1; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
        FindEditableItems();
        foreach (Gear g in gear)
        {
            ItemSwapPhoneUI itemSwap = Instantiate(itemSwapTemplate, container);
            itemSwap.gameObject.SetActive(true);
            itemSwap.AssignItem(g);
        }
        if (gear.Count == 0)
        {
            cam.enabled = false;
            RoomPreview.SetActive(false);
        }
        else
        {
            cam.enabled = true;
            RoomPreview.SetActive(true);
        }
    }

    private void ChangedActiveScene(Scene current, LoadSceneMode mode)
    {
        Refresh();
    }

    private void FindEditableItems()
    {
        gear = new HashSet<Gear>(FindObjectsOfType<Gear>());
    }

}

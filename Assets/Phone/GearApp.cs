using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GearApp : PhoneApp
{
    //public Transform container;
    //private Dictionary<string, Gear> gear;
    //private Dictionary<string, ItemSwapPhoneUI> gearSwapUI;
    //private Camera cam;
    //public GameObject RoomPreview;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.sceneLoaded += ChangedActiveScene;
        //cam = GetComponentInChildren<Camera>();
        //gearSwapUI = container.gameObject.GetComponentsInChildren<ItemSwapPhoneUI>(true).ToDictionary(i => i.Category(), i => i);
        //Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        //if (cam.isActiveAndEnabled)
        //    cam.transform.SetPositionAndRotation(new Vector3(0f, 0f, 10f), Quaternion.identity);
    }

    //private void OnEnable()
    //{
    //    if (Phone.Instance == null)
    //        return;
    //    if (cam == null)
    //        Start();
    //    if (gear.Count == 0)
    //    {
    //        cam.enabled = false;
    //        RoomPreview.SetActive(false);
    //    }
    //    else
    //    {
    //        cam.enabled = true;
    //        RoomPreview.SetActive(true);
    //    }
    //}

    //private void Refresh()
    //{
    //    if (gearSwapUI == null)
    //        return;

    //    FindEditableItems();
    //    foreach (string gearType in gearSwapUI.Keys)
    //    {
    //        if (gear.ContainsKey(gearType))
    //            gearSwapUI[gearType].AssignItem(gear[gearType]);
    //    }
    //    if (gear.Count == 0)
    //    {
    //        cam.enabled = false;
    //        RoomPreview.SetActive(false);
    //    }
    //    else
    //    {
    //        cam.enabled = true;
    //        RoomPreview.SetActive(true);
    //    }

    //}

    //private void ChangedActiveScene(Scene current, LoadSceneMode mode)
    //{
    //    Refresh();
    //}

    //private void FindEditableItems()
    //{
    //    gear = FindObjectsOfType<Gear>().Where(g => !g.shared).ToDictionary(g => g.Category(), g => g);
    //}
}

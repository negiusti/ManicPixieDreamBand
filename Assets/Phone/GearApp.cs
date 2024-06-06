using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GearApp : MonoBehaviour
{
    public ItemSwapPhoneUI itemSwapTemplate;
    public Transform container;
    private HashSet<Gear> Gear;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
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
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i));
        }
        FindEditableItems();
        foreach (Gear g in Gear)
        {
            ItemSwapPhoneUI itemSwap = Instantiate(itemSwapTemplate, container);
            itemSwap.gameObject.SetActive(true);
            itemSwap.AssignItem(g);
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        Refresh();
    }

    private void FindEditableItems()
    {
        Gear = new HashSet<Gear>(FindObjectsOfType<Gear>());
    }

}

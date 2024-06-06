using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecoratorApp : MonoBehaviour
{
    public ItemSwapPhoneUI itemSwapTemplate;
    public Transform container;
    private HashSet<Furniture> furniture;

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
        foreach (Furniture f in furniture)
        {
            ItemSwapPhoneUI itemSwap = Instantiate(itemSwapTemplate, container);
            itemSwap.gameObject.SetActive(true);
            itemSwap.AssignItem(f);
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

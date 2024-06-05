using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecoratorApp : ItemSwapper
{
    public ItemSwapPhoneUI itemSwapTemplate;
    public Transform container;

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
            itemSwap.AssignFurniture(f);
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        Refresh();
    }
}

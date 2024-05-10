using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaboodleDrawer : MonoBehaviour
{
    public GameObject content;
    public CaboodleDrawer[] otherDrawers;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        foreach (CaboodleDrawer drawer in otherDrawers)
        {
            drawer.UnselectDrawer();
        }
        content.SetActive(true);
    }

    public void UnselectDrawer()
    {
        content.SetActive(false);
    }
}

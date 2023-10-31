using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerSelectScript : MonoBehaviour
{
    public DrawerScript topsDrawer;
    public DrawerScript bottomsDrawer;
    public DrawerScript sockDrawer;
    public DrawerScript shoesDrawer;
    DrawerScript[] drawers = new DrawerScript[4];
    int selected = 0;

    // Start is called before the first frame update
    void Start()
    {
        drawers[0] = topsDrawer;
        drawers[1] = bottomsDrawer;
        drawers[2] = sockDrawer;
        drawers[3] = shoesDrawer;
        drawers[selected].SelectDrawer();
        //Transform parentTransform = transform;

        //foreach (Transform child in parentTransform)
        //{
        //    // Access the child object and do something with it
        //    GameObject childObject = child.gameObject;
        //    Debug.Log("Child Object Name: " + childObject.name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && selected < 3)
        {
            drawers[selected].UnselectDrawer();
            selected++;
            drawers[selected].SelectDrawer();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selected > 0)
        {
            drawers[selected].UnselectDrawer();
            selected--;
            drawers[selected].SelectDrawer();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerSelectScript : MonoBehaviour
{
    public DrawerScript topsDrawer;
    public DrawerScript bottomsDrawer;
    public DrawerScript sockDrawer;
    public DrawerScript shoesDrawer;
    public DrawerScript hatDrawer;
    private float shiftAmount = 2.13f;
    DrawerScript[] drawers = new DrawerScript[5];
    SpriteRenderer[] spriteRenderers = new SpriteRenderer[5];
    int selected = 0;

    // Start is called before the first frame update
    void Start()
    {
        drawers[0] = topsDrawer;
        drawers[1] = bottomsDrawer;
        drawers[2] = sockDrawer;
        drawers[3] = shoesDrawer;
        drawers[4] = hatDrawer;
        spriteRenderers[0] = topsDrawer.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderers[1] = bottomsDrawer.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderers[2] = sockDrawer.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderers[3] = shoesDrawer.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderers[4] = hatDrawer.gameObject.GetComponent<SpriteRenderer>();
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
        if (Input.GetKeyDown(KeyCode.DownArrow) && selected < 4)
        {
            drawers[selected].UnselectDrawer();
            selected++;
            if (!spriteRenderers[selected].enabled)
            {
                // UGH SCROLL !!!!
                // move all drawers UP
                foreach (DrawerScript d in drawers)
                {
                    d.gameObject.transform.position = new Vector3(d.gameObject.transform.position.x, d.gameObject.transform.position.y + shiftAmount, d.gameObject.transform.position.z);
                }
                // make top drawer(s) invisible
                spriteRenderers[0].enabled = false;
                // make current drawer visible
                spriteRenderers[selected].enabled = true;
            }
            drawers[selected].SelectDrawer();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selected > 0)
        {
            drawers[selected].UnselectDrawer();
            selected--;
            if (!spriteRenderers[selected].enabled)
            {
                // UGH SCROLL !!!!
                // move all drawers DOWN
                foreach (DrawerScript d in drawers)
                {
                    d.gameObject.transform.position = new Vector3(d.gameObject.transform.position.x, d.gameObject.transform.position.y - shiftAmount, d.gameObject.transform.position.z);
                }
                // make bottom drawer(s) invisible
                spriteRenderers[4].enabled = false;
                // make current drawer visible
                spriteRenderers[selected].enabled = true;
            }
            drawers[selected].SelectDrawer();
        }
    }
}

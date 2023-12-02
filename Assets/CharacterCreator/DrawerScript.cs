using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class DrawerScript : MonoBehaviour
{
    private SpriteResolver targetResolver;
    private SpriteLibraryAsset LibraryAsset;
    private bool selected = false;
    public DrawerScript[] otherDrawers;
    public Canvas canvas;
    private Icons icons;

    // Start is called before the first frame update
    void Start()
    {
        SpriteLibrary fuck = this.GetComponentInParent<SpriteLibrary>();
        LibraryAsset = fuck.spriteLibraryAsset;
        targetResolver = GetComponent<SpriteResolver>();
        //canvas = this.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        icons = this.GetComponentInChildren<Icons>();
        icons.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (selected && Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    foreach (AssetSwapper a in assetSwapper)
        //    {
        //        a.SelectLeft();
        //    }
        //}
        //if (selected && Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    foreach (AssetSwapper a in assetSwapper)
        //    {
        //        a.SelectRight();
        //    }
        //}
        //if (Input.GetMouseButtonDown(0) && !hovered)
        //{
        //    UnselectDrawer();
        //}
    }

    void OnMouseEnter()
    {
        // Called when the cursor enters the collider
        Debug.Log("Mouse entered the collider of " + gameObject.name);
        targetResolver.SetCategoryAndLabel("Drawer", "Open");
        icons.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        // Called when the cursor exits the collider
        Debug.Log("Mouse exited the collider of " + gameObject.name);

        if (!selected)
        {
            targetResolver.SetCategoryAndLabel("Drawer", "Closed");
            canvas.enabled = false;
            icons.gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        SelectDrawer();
    }

    public void SelectDrawer()
    {
        selected = true;
        canvas.enabled = true;
        targetResolver.SetCategoryAndLabel("Drawer", "Open");
        icons.gameObject.SetActive(true);
        foreach (DrawerScript drawer in otherDrawers)
        {
            drawer.UnselectDrawer();
        }
    }

    public void UnselectDrawer()
    {
        selected = false;
        canvas.enabled = false;
        targetResolver.SetCategoryAndLabel("Drawer", "Closed");
        icons.gameObject.SetActive(false);
    }
}

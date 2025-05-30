using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class DrawerScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private SpriteResolver targetResolver;
    private SpriteLibraryAsset LibraryAsset;
    private bool selected = false;
    public DrawerScript[] otherDrawers;
    public Canvas canvas;
    private Icons icons;
    private Collider2D coll;
    private ClothesColorPalette palette;
    public CharacterEditor.ClothesCategory category;

    // Start is called before the first frame update
    void Start()
    {
        SpriteLibrary fuck = GetComponentInParent<SpriteLibrary>();
        LibraryAsset = fuck.spriteLibraryAsset;
        targetResolver = GetComponent<SpriteResolver>();
        //canvas = this.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        icons = GetComponentInChildren<Icons>();
        coll = GetComponent<Collider2D>();
        palette = GetComponentInChildren<ClothesColorPalette>();
        HideIcons();
    }

    private void ShowIcons()
    {
        icons.gameObject.SetActive(true);
    }

    private void HideIcons()
    {
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

    public void OnMouseEnter()
    {
        // Called when the cursor enters the collider
        Debug.Log("Mouse entered the collider of " + gameObject.name);
        targetResolver.SetCategoryAndLabel("Drawer", "Open");
        ShowIcons();
        if (!selected)
            icons.DisableIconColliders();
    }

    public void OnMouseExit()
    {
        // Called when the cursor exits the collider
        Debug.Log("Mouse exited the collider of " + gameObject.name);

        if (!selected)
        {
            targetResolver.SetCategoryAndLabel("Drawer", "Closed");
            canvas.enabled = false;
            HideIcons();
        }
    }

    public void OnMouseDown()
    {
        SelectDrawer();
    }

    public void SelectDrawer()
    {
        if (selected == true)
            return;
        selected = true;
        canvas.enabled = true;
        targetResolver.SetCategoryAndLabel("Drawer", "Open");
        ShowIcons();
        if (palette != null)
            palette.gameObject.SetActive(true);
        foreach (DrawerScript drawer in otherDrawers)
        {
            drawer.UnselectDrawer();
        }
        coll.enabled = false;
        icons.EnableIconColliders();
        CharacterEditor.currCategory = category;
    }

    public void UnselectDrawer()
    {
        if (!selected == true)
            return;
        selected = false;
        canvas.enabled = false;
        targetResolver.SetCategoryAndLabel("Drawer", "Closed");
        HideIcons();
        if (palette != null)
            palette.gameObject.SetActive(false);
        coll.enabled = true;
        icons.DisableIconColliders();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

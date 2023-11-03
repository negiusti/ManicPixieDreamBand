using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class DrawerScript : MonoBehaviour
{
    private SpriteResolver targetResolver;
    private SpriteLibraryAsset LibraryAsset;
    private bool selected = false;
    public AssetSwapper[] assetSwapper;
    // Start is called before the first frame update
    void Start()
    {
        SpriteLibrary fuck = this.GetComponentInParent<SpriteLibrary>();
        LibraryAsset = fuck.spriteLibraryAsset;
        targetResolver = GetComponent<SpriteResolver>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            foreach(AssetSwapper a in assetSwapper) {
                a.SelectLeft();
            }
        }
        if (selected && Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach (AssetSwapper a in assetSwapper)
            {
                a.SelectRight();
            }
        }
    }

    void OnMouseEnter()
    {
        // Called when the cursor enters the collider
        Debug.Log("Mouse entered the collider of " + gameObject.name);
        SelectDrawer();
    }

    void OnMouseExit()
    {
        // Called when the cursor exits the collider
        Debug.Log("Mouse exited the collider of " + gameObject.name);
        UnselectDrawer();
    }

    public void SelectDrawer()
    {
        selected = true;
        targetResolver.SetCategoryAndLabel("Drawer", "Open");
    }

    public void UnselectDrawer()
    {
        selected = false;
        targetResolver.SetCategoryAndLabel("Drawer", "Closed");
    }
}

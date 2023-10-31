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

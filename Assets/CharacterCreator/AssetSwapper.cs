using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AssetSwapper : MonoBehaviour
{
    public SpriteResolver targetResolver;
    public SpriteLibraryAsset LibraryAsset;
    public string targetCategory;
    private int index = 0;
    private string[] labels;
    public void SelectRandom()
    {
        index = (index + 1 >= labels.Length) ? 0 : index + 1;
        targetResolver.SetCategoryAndLabel(targetCategory, labels[index]);
    }

    public void SelectLeft()
    {
        index = (index - 1 < 0) ? 0 : index - 1;
        targetResolver.SetCategoryAndLabel(targetCategory, labels[index]);
    }
    public void SelectRight()
    {
        index = (index + 1 >= labels.Length) ? labels.Length -1 : index + 1;
        targetResolver.SetCategoryAndLabel(targetCategory, labels[index]);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteLibrary fuck = this.GetComponentInParent<SpriteLibrary>();
        LibraryAsset = fuck.spriteLibraryAsset;
        labels = LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
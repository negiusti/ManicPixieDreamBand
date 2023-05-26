using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AssetSwapper : MonoBehaviour
{
    public SpriteResolver targetResolver;
    public SpriteLibraryAsset LibraryAsset;
    public string targetCategory;
    private int index = 0;

    public void SelectRandom()
    {
        string[] labels = LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();
        index = (index + 1 >= labels.Length) ? index = 0 : index + 1;
        targetResolver.SetCategoryAndLabel(targetCategory, labels[index]);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
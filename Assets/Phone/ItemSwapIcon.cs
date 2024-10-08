using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ItemSwapIcon : MonoBehaviour
{
    private Image img;
    private SpriteRenderer spriteRenderer;
    private SpriteResolver resolver;
    private SpriteLibrary spriteLibrary;
    private string category;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        resolver = GetComponent<SpriteResolver>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        category = resolver.GetCategory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignItem(string cat, string label)
    {
        Debug.Log("Assign category and label: " + cat + " " + label);
        if (spriteLibrary == null)
            Start();
        category = spriteLibrary.spriteLibraryAsset.GetCategoryNames().Contains(cat + "_Icons") ? cat + "_Icons" : cat;
        UpdateIcon(label);
    }

    public void UpdateIcon(string label)
    {
        Debug.Log("Set category and label: " + category + " " + label);
        resolver.SetCategoryAndLabel(category, label);
        resolver.ResolveSpriteToSpriteRenderer();
        img.sprite = spriteRenderer.sprite;
    }

}

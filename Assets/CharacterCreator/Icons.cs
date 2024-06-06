using UnityEngine;
using UnityEngine.U2D.Animation;

public class Icons : MonoBehaviour
{
    private CharacterEditor characterEditor;
    private string iconCategory;
    public string category;
    public GameObject leftIcon;
    public GameObject middleIcon;
    public GameObject rightIcon;
    private SpriteResolver leftSpriteResolver;
    private SpriteResolver middleSpriteResolver;
    private SpriteResolver rightSpriteResolver;
    private SpriteRenderer leftSpriteRen;
    private SpriteRenderer middleSpriteRen;
    private SpriteRenderer rightSpriteRen;
    private Collider2D[] colls;
    private Animator midAnim;
    // Start is called before the first frame update
    void Start()
    {
        characterEditor = FindFirstObjectByType<CharacterEditor>();
        leftSpriteResolver = leftIcon.GetComponent<SpriteResolver>();
        middleSpriteResolver = middleIcon.GetComponent<SpriteResolver>();
        rightSpriteResolver = rightIcon.GetComponent<SpriteResolver>();
        leftSpriteRen = leftIcon.GetComponent<SpriteRenderer>();
        middleSpriteRen = middleIcon.GetComponent<SpriteRenderer>();
        rightSpriteRen = rightIcon.GetComponent<SpriteRenderer>();
        colls = this.GetComponentsInChildren<Collider2D>(includeInactive:true);
        midAnim = middleIcon.GetComponent<Animator>();
        midAnim.enabled = false;
        iconCategory = (middleSpriteResolver.GetCategory() == null || middleSpriteResolver.GetCategory().Equals("None_Icons")) ? transform.parent.gameObject.name : middleSpriteResolver.GetCategory();
        if (category == null)
            category = iconCategory;
        characterEditor.UpdateIcons(category);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TO-DO: use category or icon category ??
    public string GetCategory()
    {
        return category;
    }

    // USED FOR FACE CATEGORIES ONLY SO FAR
    public void SetCurrentCategory()
    {
        characterEditor.SetCurrentFaceCategory(category);
    }

    public void UpdateIcons(string leftLabel, string middleLabel, string rightLabel)
    {
        if (characterEditor == null)
        {
            Start();
        }
        Debug.Log("HI " + gameObject.name);
        midAnim.enabled = false;
        leftSpriteResolver.SetCategoryAndLabel(iconCategory, leftLabel);
        middleSpriteResolver.SetCategoryAndLabel(iconCategory, middleLabel);
        rightSpriteResolver.SetCategoryAndLabel(iconCategory, rightLabel);
        midAnim.enabled = true;
        if (midAnim.gameObject != null && midAnim.isActiveAndEnabled)
            midAnim.Play("IconPop", -1, 0f);

        if (rightSpriteResolver != null && rightSpriteResolver.GetLabel() == "None")
        {
            rightSpriteResolver.SetCategoryAndLabel("None_Icons", category);
        }
        if (leftSpriteResolver != null && leftSpriteResolver.GetLabel() == "None")
        {
            leftSpriteResolver.SetCategoryAndLabel("None_Icons", category);
        }
        if (middleSpriteResolver != null && middleSpriteResolver.GetLabel() == "None")
        {
            middleSpriteResolver.SetCategoryAndLabel("None_Icons", category);
        }
        UpdateIconsColor(characterEditor.GetCategoryColor(category));
    }

    public void UpdateIconsColor(Color c)
    {
        Color faded = new Color(c.r, c.g, c.b, .40f);
        Color fadedWhite = new Color(1f, 1f, 1f, .40f);
        
        leftSpriteRen.color = faded;
        middleSpriteRen.color = c;
        rightSpriteRen.color = faded;

        if (rightSpriteResolver != null && (rightSpriteResolver.GetCategory() == "None_Icons" || rightSpriteResolver.GetLabel() == "None"))
        {
            rightSpriteRen.color = fadedWhite;
            rightSpriteResolver.SetCategoryAndLabel("None_Icons", category);
        }
        if (leftSpriteResolver != null && (leftSpriteResolver.GetCategory() == "None_Icons" || leftSpriteResolver.GetLabel() == "None"))
        {
            leftSpriteRen.color = fadedWhite;
            leftSpriteResolver.SetCategoryAndLabel("None_Icons", category);
        }
        if (middleSpriteResolver != null && (middleSpriteResolver.GetCategory() == "None_Icons" || middleSpriteResolver.GetLabel() == "None"))
        {
            middleSpriteRen.color = Color.white;
            middleSpriteResolver.SetCategoryAndLabel("None_Icons", category);
        }
    }

    public void DisableIconColliders()
    {
        foreach (Collider2D c in colls)
        {
            c.enabled = false;
        }
    }

    public void EnableIconColliders()
    {
        foreach (Collider2D c in colls)
        {
            c.enabled = true;
        }
    }
}

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
        iconCategory = middleSpriteResolver.GetCategory() == null ? transform.parent.gameObject.name : middleSpriteResolver.GetCategory();
        if (category == null)
            category = iconCategory;
        characterEditor.UpdateIcons(category);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetCategory()
    {
        middleSpriteResolver = middleIcon.GetComponent<SpriteResolver>();
        return middleSpriteResolver.GetCategory();
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
        if (midAnim.gameObject != null)
            midAnim.Play("IconPop", -1, 0f);
        //midAnim.CrossFade("IconPop", .5f);

        if (rightSpriteResolver.GetLabel().Equals("None"))
        {
            rightSpriteResolver.SetCategoryAndLabel("Sock_Icons", "None");
        }
        if (leftSpriteResolver.GetLabel().Equals("None"))
        {
            leftSpriteResolver.SetCategoryAndLabel("Sock_Icons", "None");
        }
        if (middleSpriteResolver.GetLabel().Equals("None"))
        {
            middleSpriteResolver.SetCategoryAndLabel("Sock_Icons", "None");
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

        if (rightSpriteResolver.GetLabel().Equals("None"))
        {
            rightSpriteRen.color = fadedWhite;
            rightSpriteResolver.SetCategoryAndLabel("Sock_Icons", "None");
        }
        if (leftSpriteResolver.GetLabel().Equals("None"))
        {
            leftSpriteRen.color = fadedWhite;
            leftSpriteResolver.SetCategoryAndLabel("Sock_Icons", "None");
        }
        if (middleSpriteResolver.GetLabel().Equals("None"))
        {
            middleSpriteRen.color = Color.white;
            middleSpriteResolver.SetCategoryAndLabel("Sock_Icons", "None");
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

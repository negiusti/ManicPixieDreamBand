using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Icons : MonoBehaviour
{
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
    private HashSet<string> smallCategories;
    private HashSet<string> mediumCategories;
    private Vector3 bigScale;
    private Vector3 smallScale;
    private Vector3 medScale;
    private Animator midAnim;
    // Start is called before the first frame update
    void Start()
    {
        leftSpriteResolver = leftIcon.GetComponent<SpriteResolver>();
        middleSpriteResolver = middleIcon.GetComponent<SpriteResolver>();
        rightSpriteResolver = rightIcon.GetComponent<SpriteResolver>();
        leftSpriteRen = leftIcon.GetComponent<SpriteRenderer>();
        middleSpriteRen = middleIcon.GetComponent<SpriteRenderer>();
        rightSpriteRen = rightIcon.GetComponent<SpriteRenderer>();
        colls = this.GetComponentsInChildren<Collider2D>();
        smallCategories = new HashSet<string> { "Necklace", "Eyebrows", "Mouth"};
        mediumCategories = new HashSet<string> { "Eyes", "Face_Detail", "Eyeshadow"};
        bigScale = new Vector3(19f, 19f, 19f);
        medScale = new Vector3(35f, 35f, 35f);
        smallScale = new Vector3(60f, 60f, 60f);
        midAnim = middleIcon.GetComponent<Animator>();
        midAnim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateIcons(string leftLabel, string middleLabel, string rightLabel)
    {
        midAnim.enabled = false;
        leftSpriteResolver.SetCategoryAndLabel(leftSpriteResolver.GetCategory(), leftLabel);
        middleSpriteResolver.SetCategoryAndLabel(middleSpriteResolver.GetCategory(), middleLabel);
        rightSpriteResolver.SetCategoryAndLabel(rightSpriteResolver.GetCategory(), rightLabel);
        midAnim.enabled = true;
        midAnim.Play("IconPop", -1, 0f);
        //midAnim.CrossFade("IconPop", .5f);
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


    // USED ONLY FOR FACE ICONS
    public void UpdateIcons(string category, string leftLabel, string middleLabel, string rightLabel)
    {
        // Resize the icon sprites
        if (smallCategories.Contains(category))
        {
            leftSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = smallScale;
            middleSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = smallScale * 1.2f;
            rightSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = smallScale;
        } else if (mediumCategories.Contains(category))
        {
            leftSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = medScale;
            middleSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = medScale * 1.2f;
            rightSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = medScale;
        } else
         {
            leftSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = bigScale;
            middleSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = bigScale * 1.2f;
            rightSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = bigScale;
        }

        // Actually set the icon sprite images
        leftSpriteResolver.SetCategoryAndLabel(category, leftLabel);
        middleSpriteResolver.SetCategoryAndLabel(category, middleLabel);
        rightSpriteResolver.SetCategoryAndLabel(category, rightLabel);

        Color fadedWhite = new Color(1f, 1f, 1f, .40f);
        // Overwrite the icon with the none icon if necessary
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

        // Resize the colliders to fit the icons
        Vector2 S = leftSpriteRen.sprite.bounds.size;
        leftSpriteRen.gameObject.GetComponent<BoxCollider2D>().size = S;
        leftSpriteRen.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);

        //S = middleSpriteRen.sprite.bounds.size;
        //middleSpriteRen.gameObject.GetComponent<BoxCollider2D>().size = S;
        //middleSpriteRen.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((S.x / 2), 0);

        S = rightSpriteRen.sprite.bounds.size;
        rightSpriteRen.gameObject.GetComponent<BoxCollider2D>().size = S;
        rightSpriteRen.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);

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

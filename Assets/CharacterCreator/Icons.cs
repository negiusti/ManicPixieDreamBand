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
    private Vector3 bigScale;
    private Vector3 smallScale;
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
        smallCategories = new HashSet<string> { "Necklace", "Eyebrows", "Mouth", "Face_Detail"};
        bigScale = new Vector3(19f, 19f, 19f);
        smallScale = new Vector3(60f, 60f, 60f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateIcons(string leftLabel, string middleLabel, string rightLabel)
    {
        leftSpriteResolver.SetCategoryAndLabel(leftSpriteResolver.GetCategory(), leftLabel);
        middleSpriteResolver.SetCategoryAndLabel(middleSpriteResolver.GetCategory(), middleLabel);
        rightSpriteResolver.SetCategoryAndLabel(rightSpriteResolver.GetCategory(), rightLabel);
    }

    public void UpdateIconsColor(Color c)
    {
        leftSpriteRen.color = c;
        middleSpriteRen.color = c;
        rightSpriteRen.color = c;
    }


    // USED ONLY FOR FACE ICONS
    public void UpdateIcons(string category, string leftLabel, string middleLabel, string rightLabel)
    {
        if (smallCategories.Contains(category))
        {
            leftSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = smallScale;
            middleSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = smallScale * 1.2f;
            rightSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = smallScale;
        } else
         {
            leftSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = bigScale;
            middleSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = bigScale * 1.2f;
            rightSpriteResolver.gameObject.GetComponent<RectTransform>().localScale = bigScale;
        }
        leftSpriteResolver.SetCategoryAndLabel(category, leftLabel);
        middleSpriteResolver.SetCategoryAndLabel(category, middleLabel);
        rightSpriteResolver.SetCategoryAndLabel(category, rightLabel);
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

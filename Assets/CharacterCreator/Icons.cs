using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        leftSpriteResolver = leftIcon.GetComponent<SpriteResolver>();
        middleSpriteResolver = middleIcon.GetComponent<SpriteResolver>();
        rightSpriteResolver = rightIcon.GetComponent<SpriteResolver>();
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
}
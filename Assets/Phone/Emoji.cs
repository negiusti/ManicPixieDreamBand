using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Emoji : MonoBehaviour
{
    private SpriteResolver emojiIcon;
    private SpriteRenderer emojiRen;
    private Image emojiImg;

    // Start is called before the first frame update
    void Start()
    {
        emojiIcon = GetComponent<SpriteResolver>();
        emojiImg = GetComponent<Image>();
        emojiRen = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmoji(string emojiName)
    {
        if (emojiIcon == null)
        {
            Start();
        }
        emojiIcon.SetCategoryAndLabel("Emoji", emojiName);
        emojiIcon.ResolveSpriteToSpriteRenderer();
        emojiImg.sprite = emojiRen.sprite;
    }

    public void SetRomanceEmoji(int romanceLvl)
    {
        if (romanceLvl < 1)
            return;
        emojiIcon.SetCategoryAndLabel("Romance", romanceLvl.ToString());
        emojiIcon.ResolveSpriteToSpriteRenderer();
        emojiImg.sprite = emojiRen.sprite;
    }
}

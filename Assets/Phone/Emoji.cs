using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Emoji : MonoBehaviour
{
    public SpriteResolver emojiIcon;
    public SpriteRenderer emojiRen;
    public Image emojiImg;

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
        emojiIcon.SetCategoryAndLabel("Emoji", emojiName);
        emojiIcon.ResolveSpriteToSpriteRenderer();
        emojiImg.sprite = emojiRen.sprite;
    }

    public void SetRomanceEmoji(int romanceLvl)
    {
        emojiIcon.SetCategoryAndLabel("RomanceEmoji", romanceLvl.ToString());
        emojiIcon.ResolveSpriteToSpriteRenderer();
        emojiImg.sprite = emojiRen.sprite;
    }
}

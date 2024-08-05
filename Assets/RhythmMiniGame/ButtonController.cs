using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public SpriteResolver guitarString;
    public KeyCode keyCode;
    private Color pressedColor = new Color(219/255f, 101/255f, 156/255f);
    private Color unpressedColor = new Color(1f, 197/255f, 220/255f);

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            spriteRenderer.sprite = pressedSprite;
            spriteRenderer.color = pressedColor;
            guitarString.SetCategoryAndLabel("String", "Wiggly");
        }
        if (Input.GetKeyUp(keyCode))
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = unpressedColor;
            guitarString.SetCategoryAndLabel("String", "Still");
        }
    }
}

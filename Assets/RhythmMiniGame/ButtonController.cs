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
            spriteRenderer.color = Color.green;
            guitarString.SetCategoryAndLabel("String", "Wiggly");
        }
        if (Input.GetKeyUp(keyCode))
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = Color.cyan;
            guitarString.SetCategoryAndLabel("String", "Still");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ButtonController : MonoBehaviour
{
    public StarSpawnerScript starSpawner;
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public SpriteResolver guitarString;
    public KeyCode keyCode;
    private Color pressedColor = new Color(254/255f, 89/255f, 136/255f);
    private Color unpressedColor = new Color(253/255f, 197/255f, 235/255f);
    private bool touchingStar;
    private BassMiniGame mg;

    // Start is called before the first frame update
    void Start()
    {
        mg = GetComponentInParent<BassMiniGame>(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
        spriteRenderer.color = unpressedColor;
        touchingStar = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            spriteRenderer.sprite = pressedSprite;
            spriteRenderer.color = pressedColor;
            guitarString.SetCategoryAndLabel("String", "Wiggly");
            if (!touchingStar)
            {
                starSpawner.WrongNote();
                mg.PlayBadSound();
            }
        }
        if (Input.GetKeyUp(keyCode))
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = unpressedColor;
            guitarString.SetCategoryAndLabel("String", "Still");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Star"))
        {
            touchingStar = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Star"))
        {
            touchingStar = false;
        }
            
    }
}

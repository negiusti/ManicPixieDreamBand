using UnityEngine;
using UnityEngine.U2D.Animation;

public class ImprovButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public SpriteResolver guitarString;
    public KeyCode keyCode;
    private Color pressedColor = new Color(254 / 255f, 89 / 255f, 136 / 255f);
    private Color unpressedColor = new Color(253 / 255f, 197 / 255f, 235 / 255f);
    private ImprovMiniGame mg;
    public int idx;

    // Start is called before the first frame update
    void Start()
    {
        mg = GetComponentInParent<ImprovMiniGame>(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
        spriteRenderer.color = unpressedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            spriteRenderer.sprite = pressedSprite;
            spriteRenderer.color = pressedColor;
            guitarString.SetCategoryAndLabel("String", "Wiggly");
            mg.PlayPentatonicNote(idx);
        }
        if (Input.GetKeyUp(keyCode))
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = unpressedColor;
            guitarString.SetCategoryAndLabel("String", "Still");
        }
    }
}
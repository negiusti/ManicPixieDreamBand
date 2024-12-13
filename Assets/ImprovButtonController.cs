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
    private bool alternate;
    private GameObject starTemplate;

    // Start is called before the first frame update
    void Start()
    {
        mg = GetComponentInParent<ImprovMiniGame>(true);
        starTemplate = GetComponentInChildren<StarMoverScript>(true).gameObject;
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
            int clipIndex = idx * 2;
            clipIndex += alternate ? 1 : 0;
            mg.PlayPentatonicNote(clipIndex);
            GameObject star = Instantiate(starTemplate, transform);
            star.SetActive(true);
            alternate = !alternate;
        }
        if (Input.GetKeyUp(keyCode))
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = unpressedColor;
            guitarString.SetCategoryAndLabel("String", "Still");
        }
    }
}

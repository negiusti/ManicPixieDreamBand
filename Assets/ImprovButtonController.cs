using UnityEngine;
using UnityEngine.U2D.Animation;
using Rewired;

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
    private Player player;
    private string inputName;

    // Start is called before the first frame update
    void Start()
    {
        mg = GetComponentInParent<ImprovMiniGame>(true);
        starTemplate = GetComponentInChildren<StarMoverScript>(true).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
        spriteRenderer.color = unpressedColor;
        player = ReInput.players.GetPlayer(0);
        switch (keyCode)
        {
            case KeyCode.Alpha1:
                inputName = "E string";
                break;
            case KeyCode.Alpha2:
                inputName = "A string";
                break;
            case KeyCode.Alpha3:
                inputName = "D string";
                break;
            case KeyCode.Alpha4:
                inputName = "G string";
                break;
            default:
                break;
        }
    }

        // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown(inputName))
        //if (Input.GetKeyDown(keyCode))
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
        if (player.GetButtonUp(inputName))
        //if (Input.GetKeyUp(keyCode))
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = unpressedColor;
            guitarString.SetCategoryAndLabel("String", "Still");
        }
    }
}
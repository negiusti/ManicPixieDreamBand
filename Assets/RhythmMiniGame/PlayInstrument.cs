using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayInstrument : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteResolver spriteResolver;
    private bool withinRange;
    public KeyCode keyToTrigger;
    private PlayerMovement playerMovement;
    public GameObject minigame;
    private bool isPlayingGuitar;
    private string instLabel;

    // Start is called before the first frame update
    void Start()
    {
        withinRange = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteResolver = this.GetComponent<SpriteResolver>();
        instLabel = spriteResolver.GetLabel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayingGuitar && withinRange && Input.GetKey(keyToTrigger))
        {
            spriteRenderer.enabled = false;
            isPlayingGuitar = true;
            playerMovement.PlayGuitar(instLabel);
        }
        if (isPlayingGuitar && !minigame.activeSelf)
        {
            spriteRenderer.enabled = true;
            isPlayingGuitar = false;
            playerMovement.StopPlayingGuitar();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            withinRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            withinRange = false;
        }
    }
}

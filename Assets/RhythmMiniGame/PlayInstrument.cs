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
    private bool isPlayingInstrument;
    private string instLabel;
    private float startTime;
    //public enum Instrument
    //{
    //    Drums,
    //    Guitar,
    //    Bass
    //};
    
    //public Instrument instrument;

    // Start is called before the first frame update
    void Start()
    {
        withinRange = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteResolver = this.GetComponent<SpriteResolver>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isPlayingInstrument && withinRange && Input.GetKey(keyToTrigger))
        {
            spriteRenderer.enabled = false;
            isPlayingInstrument = true;
            instLabel = spriteResolver.GetLabel();
            startTime = Time.time;
            playerMovement.PlayInstrument(instLabel, transform.position.x);
        }
        if (isPlayingInstrument && !minigame.activeSelf && Time.time - startTime > 1f)
        {
            spriteRenderer.enabled = true;
            isPlayingInstrument = false;
            playerMovement.StopPlayingInstrument();
        }
    }

    public bool isBeingPlayed()
    {
        return isPlayingInstrument;
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

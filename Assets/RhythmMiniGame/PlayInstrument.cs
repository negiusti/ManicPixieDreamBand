using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayInstrument : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteResolver spriteResolver;
    private bool withinRange;
    public KeyCode keyToTrigger;
    private Movement musicianMovement;
    public GameObject minigame;
    private bool isPlayingInstrument;
    private string instLabel;
    private float startTime;

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
        // Only applies to MainCharacter
        if (!isPlayingInstrument && withinRange && Input.GetKey(keyToTrigger))
        {
            Play();
            JamCoordinator.StartJam();
        }
        if (isPlayingInstrument && !minigame.activeSelf && Time.time - startTime > 1f)
        {
            Stop();
            JamCoordinator.EndJam();
        }
    }

    // Used by MainCharacter only (the movement is set by the collider)
    public void Play()
    {
        Play(musicianMovement);
    }

    // Used by NPCs
    public void Play(Movement movement)
    {
        spriteRenderer.enabled = false;
        isPlayingInstrument = true;
        instLabel = spriteResolver.GetLabel();
        startTime = Time.time;
        musicianMovement = movement;
        musicianMovement.PlayInstrument(instLabel, transform.position);
    }

    // Used by NPCs as well as MainCharacter
    public void Stop()
    {
        spriteRenderer.enabled = true;
        isPlayingInstrument = false;
        musicianMovement.StopPlayingInstrument();
    }

    public bool IsBeingPlayed()
    {
        return isPlayingInstrument;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            musicianMovement = other.gameObject.GetComponent<Movement>();
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
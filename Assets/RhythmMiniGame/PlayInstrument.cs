using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class PlayInstrument : MonoBehaviour, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private SpriteResolver spriteResolver;
    private bool withinRange;
    public KeyCode keyToTrigger;
    private Movement musicianMovement;
    public MiniGame minigame;
    private bool isPlayingInstrument;
    private string instLabel;
    private float startTime;
    private Vector3 spawnPos;
    private string layer;
    private int layerOrder;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        withinRange = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteResolver = this.GetComponent<SpriteResolver>();
        spawnPos = this.transform.GetChild(0).transform.position;
        layer = spriteRenderer.sortingLayerName;
        layerOrder = spriteRenderer.sortingOrder;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Only applies to MainCharacter, this is only for playing solo, not with the band
        if (!isPlayingInstrument && withinRange && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && InteractionEnabled())
        {
            //Play();
            //JamCoordinator.StartJam("LEMON BOY");
            //JamCoordinator.StartJam("The Storm");
            Play(FindFirstObjectByType<PlayerMovement>());
            MiniGameManager.StartMiniGame("Solo");
            //minigame.OpenMiniGame();
        }
        //if (isPlayingInstrument && !minigame.IsMiniGameActive() && Time.time - startTime > 1f)
        //{
        //    Stop();
        //    JamCoordinator.EndJam();
        //}
    }

    private void OnMouseDown()
    {
        if (!isPlayingInstrument && withinRange && InteractionEnabled())
        {
            Play(FindFirstObjectByType<PlayerMovement>());
            MiniGameManager.StartMiniGame("Solo");
        }
    }

    public Vector3 SpawnPos()
    {
        return spawnPos;
    }

    // Used by MainCharacter only (the movement is set by the collider)
    public void Play()
    {
        Play(musicianMovement);
    }

    // Used by NPCs
    public void Play(Movement movement)
    {
        if (isPlayingInstrument)
        {
            Debug.Log("Already playing...");
            return;
        }
        isPlayingInstrument = true;
        spriteRenderer.enabled = false;
        instLabel = spriteResolver.GetLabel();
        startTime = Time.time;
        musicianMovement = movement;
        musicianMovement.PlayInstrument(instLabel, spawnPos, layer, layerOrder);
    }

    // Used by NPCs as well as MainCharacter
    public void Stop()
    {
        if (!isPlayingInstrument)
            return;
        isPlayingInstrument = false;
        spriteRenderer.enabled = true;
        musicianMovement.StopPlayingInstrument();
        //minigame.CloseMiniGame();
    }

    public bool IsBeingPlayed()
    {
        return isPlayingInstrument;
    }

    private bool InteractionEnabled()
    {
        return !Characters.MainCharacter().gameObject.GetComponent<Movement>().IsSkating() &&
            Phone.Instance.IsLocked() &&
            !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen() &&
            !SceneChanger.Instance.IsLoadingScreenOpen() &&
            !DialogueManager.IsConversationActive &&
            !MiniGameManager.AnyActiveMiniGames();
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player") && !other.GetComponent<Movement>().IsSkating())
    //    {
    //        musicianMovement = other.gameObject.GetComponent<Movement>();
    //        withinRange = true;
    //        animator.enabled = true;
    //    }
    //}

    public void SetInstrument(string category, string label)
    {
        spriteResolver.SetCategoryAndLabel(category, label);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && InteractionEnabled())
        {
            musicianMovement = other.gameObject.GetComponent<Movement>();
            withinRange = true;
        } else
        {
            withinRange = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            withinRange = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPlayingInstrument && withinRange && InteractionEnabled())
        {
            Play(FindFirstObjectByType<PlayerMovement>());
            MiniGameManager.StartMiniGame("Solo");
        }
    }
}

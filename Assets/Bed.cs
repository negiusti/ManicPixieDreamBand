using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class Bed : MonoBehaviour, IPointerDownHandler
{
    private Animator animator;
    private SleepingScreenMiniGame sleepingScreen;
    private bool inRange;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.SetBool("DoneForTheDay", Calendar.DoneForTheDay());
        //GetComponent<BiggerWhenHovered>().scaleFactor = Calendar.DoneForTheDay() ? 1.1f : 1f;
        sleepingScreen = GetComponentInChildren<SleepingScreenMiniGame>();
        player = ReInput.players.GetPlayer(0);
        inRange = false;
        animator.SetBool("InRange", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Calendar.DoneForTheDay() && InteractionEnabled() && player.GetButtonDown("Interact"))
        {
            sleepingScreen.OpenMiniGame();
        }
    }

    public void SleepyTime()
    {
        //animator.SetBool("DoneForTheDay", Calendar.DoneForTheDay());
        //GetComponent<BiggerWhenHovered>().scaleFactor = Calendar.DoneForTheDay() ? 1.1f : 1f;
    }

    private void OnMouseDown()
    {
        if (inRange && Calendar.DoneForTheDay() && InteractionEnabled())
        {            
            //animator.SetBool("DoneForTheDay", false);
            //GetComponent<BiggerWhenHovered>().scaleFactor = 1f;
            sleepingScreen.OpenMiniGame();
        }
    }

    private bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            Phone.Instance.IsLocked() &&
            !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen() &&
            !DialogueManager.IsConversationActive &&
            !MiniGameManager.AnyActiveMiniGames();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Calendar.DoneForTheDay() && InteractionEnabled())
        {
            inRange = true;
            animator.SetBool("InRange", true);
        }
        else
        {
            inRange = false;
            animator.SetBool("InRange", false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            animator.SetBool("InRange", false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }

}

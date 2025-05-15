using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Rewired;

public class OutdoorLocation : MonoBehaviour
{
    private bool inRange;
    private string location;
    private GameObject prompt;
    private Animator sign;
    public bool isBusiness;
    public bool isOpen;
    private SpriteResolver openCloseSign;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        location = gameObject.name;
        Canvas p = GetComponentInChildren<Canvas>();
        if (p != null)
            prompt = p.gameObject;

        if(prompt != null)
            prompt.SetActive(false);
        sign = GetComponentInChildren<Animator>();
        openCloseSign = GetComponentInChildren<SpriteResolver>();
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && DialogueManager.IsConversationActive)
        {
            prompt.SetActive(false);
            if (sign != null)
                sign.SetBool("Show", false);
            inRange = false;
            return;
        }
        if (player.GetButtonDown("Interact") && inRange && InteractionEnabled())
        {
            if (isBusiness)
                GameManager.miscSoundEffects.Play("businessdoor");
            else
                GameManager.miscSoundEffects.Play("Door");
            SceneChanger.Instance.ChangeScene(location);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !DialogueManager.IsConversationActive)
        {
            CheckOpenClose();
            if (sign != null)
                sign.SetBool("Show", true);
            if (isOpen)
            {
                prompt.SetActive(true);
                inRange = true;
            }
        }
    }

    private void CheckOpenClose()
    {
        switch(location)
        {
            case "TattooShop":
                if (JobSystem.CurrentJob() == JobSystem.PunkJob.Tattoo && ConvoRequirements.CurrentEventType() == "JobEvent")
                    OpenSign();
                else
                    CloseSign();
                break;
            case "SkateShop":
                if (Calendar.WasDNDCompletedToday())
                    CloseSign();
                else
                    OpenSign();
                break;
            case "SmallBar":
                if (Calendar.IsNight())
                    OpenSign();
                else
                {
                    if (Calendar.GetCurrentEvent().Name().ToLower().Contains("gig") ||
                        Calendar.GetCurrentEvent().Name().ToLower().Contains("keys"))
                    {
                        OpenSign();
                    } else
                    {
                        CloseSign();
                    }
                }
                break;
            default:
                OpenSign();
                break;
        }
    }

    private void OpenSign()
    {
        isOpen = true;
        if (openCloseSign!= null)
            openCloseSign.SetCategoryAndLabel("Sign", "Open");
    }

    private void CloseSign()
    {
        isOpen = false;
        if (openCloseSign != null)
            openCloseSign.SetCategoryAndLabel("Sign", "Closed");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckOpenClose();
            prompt.SetActive(false);
            if (sign != null)
                sign.SetBool("Show", false);
            inRange = false;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.U2D.Animation;
//using UnityEngine.SceneManagement;

public class Phone : MonoBehaviour
{
    public static Phone Instance;
    public GameObject icons;
    public GameObject background;
    public GameObject backButton;
    public AppHeader appHeader;
    public GameObject notificationIndicator;
    private PhoneMessages messagesApp;
    private BankApp bankApp;
    private MapsApp mapsApp;
    private SpriteResolver backgroundResolver;
    private bool isLocked;
    private Animator animator;
    private CustomDialogueScript customDialogue;
    public StandardUIMenuPanel txtResponsePanel;

    enum PhoneState
    {
        Home,
        Messages,
        Map,
        Pin,
        Settings,
        Photos,
        Bank,
        Convo
    };

    // State stack
    private Stack<PhoneState> phoneStateStack;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.activeSceneChanged += ChangedActiveScene;
        backgroundResolver = background.GetComponent<SpriteResolver>();
        phoneStateStack = new Stack<PhoneState>();
        phoneStateStack.Push(PhoneState.Home);
        //contactsList = SaveSystem.LoadContactsList();
        bankApp = this.GetComponentInChildren<BankApp>();
        mapsApp = this.GetComponentInChildren<MapsApp>();
        messagesApp = this.GetComponentInChildren<PhoneMessages>();
        animator = this.GetComponent<Animator>();
        isLocked = true;
        Lock();
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        if (messagesApp.HasPendingConvos())
            EnableNotificationIndicator();
        else
            DisableNotificationIndicator();
        //txtResponsePanel = this.GetComponentInChildren<PixelCrushers.DialogueSystem.Wrappers.StandardUIMenuPanel>();
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    // Update is called once per frame
    void Update()
    {

        // if we don't have the convo open rn, stop the convo when it needs our response
        if (!phoneStateStack.Peek().Equals(PhoneState.Convo) && customDialogue.IsTxtConvoActive())
        {
            txtResponsePanel.Close();
            customDialogue.StopCurrentConvo();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            // Toggle phone visibility
            ToggleLock();
        }
    }

    private bool IsTxtResponseMenuOpen()
    {
        return txtResponsePanel != null && txtResponsePanel.gameObject.activeSelf;
    }

    public void EnableNotificationIndicator()
    {
        notificationIndicator.SetActive(true);
    }

    public void DisableNotificationIndicator()
    {
        notificationIndicator.SetActive(false);
    }

    public string GetContactNameFromConvoName(string convoName)
    {
        if (!customDialogue.IsTxtConvo(convoName))
        {
            Debug.LogError("conversation is not a txt convo: " + convoName);
        }
        return convoName.Split('_')[1];
    }

    public void GoHome()
    {
        phoneStateStack = new Stack<PhoneState>();
        phoneStateStack.Push(PhoneState.Home);
        CloseApps();
        backgroundResolver.SetCategoryAndLabel("Background", "Home");
    }

    public void OpenMessages()
    {
        SetAppHeader("Messages");
        if (phoneStateStack.Peek() != PhoneState.Messages)
            phoneStateStack.Push(PhoneState.Messages);
        messagesApp.gameObject.SetActive(true);
        messagesApp.OpenContacts();
        HideIcons();
        backgroundResolver.SetCategoryAndLabel("Background", "Contacts");
    }

    public void OpenConvo()
    {
        appHeader.gameObject.SetActive(false);
        if (phoneStateStack.Peek() != PhoneState.Convo)
            phoneStateStack.Push(PhoneState.Convo);
        backgroundResolver.SetCategoryAndLabel("Background", "Messages");
    }

    public void OpenPin()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Maps2");
        if (phoneStateStack.Peek() != PhoneState.Pin)
            phoneStateStack.Push(PhoneState.Pin);
    }

    public void ReceiveMsg(string conversation)
    {
        messagesApp.ReceiveMsg(GetContactNameFromConvoName(conversation), conversation);
        EnableNotificationIndicator();
    }

    public void OpenBank()
    {
        SetAppHeader("Hellcorp Bank");
        bankApp.gameObject.SetActive(true);
        bankApp.UpdateBankBalance();
        if (phoneStateStack.Peek() != PhoneState.Bank)
            phoneStateStack.Push(PhoneState.Bank);
        HideIcons();
        backgroundResolver.SetCategoryAndLabel("Background", "Bank");
    }

    public MainCharacter GetMainCharacter()
    {
        return GameObject.FindFirstObjectByType<MainCharacter>();
    }

    public void OpenMap()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Maps1");
        mapsApp.gameObject.SetActive(true);
        mapsApp.Open();
        SetAppHeader("");
        if (phoneStateStack.Peek() != PhoneState.Map)
            phoneStateStack.Push(PhoneState.Map);
        HideIcons();
    }

    public void OpenSettings()
    {
        SetAppHeader("Settings");
        if (phoneStateStack.Peek() != PhoneState.Settings)
            phoneStateStack.Push(PhoneState.Settings);
        HideIcons();
    }

    public void OpenPhotos()
    {
        SetAppHeader("Photos");
        if (phoneStateStack.Peek() != PhoneState.Photos)
            phoneStateStack.Push(PhoneState.Photos);
        HideIcons();
    }

    public void GoBack()
    {
        PhoneState state = phoneStateStack.Peek();
        if (state.Equals(PhoneState.Home))
        {
            GoHome();
            return;
        }

        phoneStateStack.Pop();
        state = phoneStateStack.Peek();
        Debug.Log("poppin " + state.ToString());
        switch (state)
        {
            case PhoneState.Messages:
                OpenMessages();
                break;

            case PhoneState.Map:
                OpenMap();
                break;

            case PhoneState.Settings:
                OpenSettings();
                break;

            case PhoneState.Photos:
                OpenPhotos();
                break;

            case PhoneState.Bank:
                OpenBank();
                break;

            case PhoneState.Home:
                GoHome();
                break;

            default:
                Debug.Log("State not found: " + state.ToString());
                break;
        }
    }

    private void SetAppHeader(string appName)
    {
        appHeader.gameObject.SetActive(true);
        appHeader.SetAppHeader(appName);
    }

    public void CompleteConvo(string convoName)
    {
        messagesApp.CompleteConvo(GetContactNameFromConvoName(convoName));
        DisableNotificationIndicator();
    }

    private void HideIcons()
    {
        backButton.SetActive(true);
        icons.SetActive(false);
    }

    private void CloseApps()
    {
        bankApp.gameObject.SetActive(false);
        appHeader.gameObject.SetActive(false);
        messagesApp.gameObject.SetActive(false);
        mapsApp.gameObject.SetActive(false);
        backButton.SetActive(false);
        ShowIcons();
    }

    private void ShowIcons()
    {   
        icons.SetActive(true);
    }
    public void ToggleLock()
    {
        isLocked = !isLocked;
        if (isLocked)
            Lock();
        else
            Unlock();
    }
    public void Lock()
    {
        isLocked = true;
        GoHome();
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            child.Translate(Vector3.down * 20f, Space.Self);
        }
        //transform.Translate(Vector3.down * 20f, Space.World);
    }

    // TODO: Don't unlock the phone during conversations, minigames, etc!!!
    private bool ShouldNotUnlock()
    {
        return DialogueManager.isConversationActive && !customDialogue.IsCurrentConvoTxt();
    }

    private void Unlock()
    {
        isLocked = false;
        if (ShouldNotUnlock())
        {
            isLocked = true;
            return;
        }
        float deltaY = background.transform.position.y;
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            
            child.Translate(Vector3.up * -deltaY, Space.Self);
        }
        //transform.Translate(Vector3.up * -background.transform.position.y, Space.World);
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

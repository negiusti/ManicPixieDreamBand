using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.U2D.Animation;
using System.Collections;

public class Phone : MonoBehaviour
{
    public static Phone Instance;
    public GameObject HUDIcon;
    public GameObject icons;
    public GameObject background;
    public GameObject backButton;
    public AppHeader appHeader;
    public GameObject notificationIndicator;
    private PhoneMessages messagesApp;
    private BankApp bankApp;
    private MapsApp mapsApp;
    private CalendarApp calendarApp;
    private PocketsApp pocketsApp;
    private GearApp gearApp;
    private DecoratorApp decoratorApp;
    private SpriteResolver backgroundResolver;
    private bool isLocked;
    private CustomDialogueScript customDialogue;
    public StandardUIMenuPanel txtResponsePanel;
    private PhoneNotifications notifications;
    public GameObject TransitionScreen;
    

    enum PhoneState
    {
        Home,
        Messages,
        Map,
        Pin,
        Settings,
        Photos,
        Bank,
        Convo,
        Calendar,
        Pockets,
        Decorator,
        Gear
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
        bankApp = this.GetComponentInChildren<BankApp>(true);
        mapsApp = this.GetComponentInChildren<MapsApp>(true);
        messagesApp = this.GetComponentInChildren<PhoneMessages>(true);
        calendarApp = this.GetComponentInChildren<CalendarApp>(true);
        pocketsApp = this.GetComponentInChildren<PocketsApp>(true);
        decoratorApp = this.GetComponentInChildren<DecoratorApp>(true);
        gearApp = this.GetComponentInChildren<GearApp>(true);
        //animator = this.GetComponent<Animator>();
        notifications = this.GetComponentInChildren<PhoneNotifications>();
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

    public PocketsApp GetPocketsApp()
    {
        return pocketsApp;
    }

    public void NotificationMessage(string txt)
    {
        notifications.Add(txt);
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
        return txtResponsePanel != null && txtResponsePanel.gameObject.activeSelf && txtResponsePanel.isOpen;
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

    public void OpenTxtConvoWith(string contact)
    {
        OpenMessages();
        messagesApp.OpenTxtConvoWith(contact);
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

    public void OpenCalendar()
    {
        if (Calendar.IsNight())
            backgroundResolver.SetCategoryAndLabel("Background", "Calendar_Night");
        else
            backgroundResolver.SetCategoryAndLabel("Background", "Calendar_Day");

        calendarApp.gameObject.SetActive(true);
        //calendarApp.Open();
        SetAppHeader("CAL PAL");
        if (phoneStateStack.Peek() != PhoneState.Calendar)
            phoneStateStack.Push(PhoneState.Calendar);
        HideIcons();
    }
    
    public void OpenPockets()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Pockets");

        pocketsApp.gameObject.SetActive(true);
        SetAppHeader("Pocketz");
        if (phoneStateStack.Peek() != PhoneState.Pockets)
            phoneStateStack.Push(PhoneState.Pockets);
        HideIcons();
    }

    public void OpenGear()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Gear");

        gearApp.gameObject.SetActive(true);
        SetAppHeader("Gear");
        if (phoneStateStack.Peek() != PhoneState.Gear)
            phoneStateStack.Push(PhoneState.Gear);
        HideIcons();
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            Vector3 target = child.transform.localPosition + (Vector3.left * 250f);
            StartCoroutine(Lerp(child.gameObject, target, 0.5f));
        }
    }

    public void OpenDecorator()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Decorator");

        decoratorApp.gameObject.SetActive(true);
        SetAppHeader("Decorator");
        if (phoneStateStack.Peek() != PhoneState.Decorator)
            phoneStateStack.Push(PhoneState.Decorator);
        HideIcons();

        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            Vector3 target = child.transform.localPosition + (Vector3.left * 250f);
            StartCoroutine(Lerp(child.gameObject, target, 0.5f));
        }
    }

    public void CloseDecorator()
    {
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            float deltaX = background.transform.localPosition.x - Camera.main.gameObject.transform.position.x;
            Vector3 target = child.transform.localPosition + (Vector3.left * deltaX);
            StartCoroutine(Lerp(child.gameObject, target, 0.5f));
        }
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
        if (state.Equals(PhoneState.Decorator) || state.Equals(PhoneState.Gear))
        {
            CloseDecorator();
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

            case PhoneState.Calendar:
                OpenCalendar();
                break;

            case PhoneState.Pockets:
                OpenPockets();
                break;

            case PhoneState.Decorator:
                OpenDecorator();
                break;

            case PhoneState.Gear:
                OpenGear();
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
        TransitionScreen.GetComponent<Animator>().Play("ScreenTransition");
        backButton.SetActive(true);
        icons.SetActive(false);
    }

    private void CloseApps()
    {
        TransitionScreen.GetComponent<Animator>().Play("ScreenTransition");
        
        CloseDecorator();
        bankApp.gameObject.SetActive(false);
        appHeader.gameObject.SetActive(false);
        messagesApp.gameObject.SetActive(false);
        mapsApp.gameObject.SetActive(false);
        calendarApp.gameObject.SetActive(false);
        pocketsApp.gameObject.SetActive(false);
        decoratorApp.gameObject.SetActive(false);
        gearApp.gameObject.SetActive(false);
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
        HUDIcon.GetComponent<SpriteResolver>().SetCategoryAndLabel("PhoneHUDIcon", "Locked");
        GoHome();
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            float deltaX = background.transform.localPosition.x - Camera.main.gameObject.transform.position.x;
            Vector3 target = child.transform.localPosition + (Vector3.down * 1600f) + (Vector3.left * deltaX);
            StartCoroutine(Lerp(child.gameObject, target, 0.5f));
        }
    }

    // TODO: Don't unlock the phone during conversations, minigames, etc!!!
    private bool ShouldNotUnlock()
    {
        return DialogueManager.isConversationActive && !customDialogue.IsCurrentConvoTxt();
    }

    public void Unlock()
    {
        isLocked = false;
        if (ShouldNotUnlock() || Camera.main == null)
        {
            isLocked = true;
            return;
        }
        HUDIcon.GetComponent<SpriteResolver>().SetCategoryAndLabel("PhoneHUDIcon", "Unlocked");
        float deltaY = background.transform.localPosition.y - Camera.main.gameObject.transform.position.y;
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            
            Vector3 target = child.transform.localPosition + (Vector3.up * -deltaY);
            StartCoroutine(Lerp(child.gameObject, target, 0.5f));
        }
    }

    public IEnumerator Lerp(GameObject go, Vector3 targetLocalPosition, float duration)
    {
        Vector3 startPosition = go.transform.localPosition;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.SmoothStep(0, 1, factor);

            go.transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, factor);

            yield return null;
        }
        go.transform.localPosition = targetLocalPosition;
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Phone.Instance == null)
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

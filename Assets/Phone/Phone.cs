using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.U2D.Animation;
using System.Collections;
using System.Linq;
using Rewired;
using System.Text.RegularExpressions;

public class Phone : MonoBehaviour
{
    public static Phone Instance;
    public GameObject HUDIcon;
    public GameObject PhoneCase;
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
    private SkateApp skateApp;
    private DecoratorApp decoratorApp;
    private SettingsApp settingsApp;
    private PhotosApp photosApp;
    private SpriteResolver backgroundResolver;
    private bool isLocked;
    private CustomDialogueScript customDialogue;
    public StandardUIMenuPanel txtResponsePanel;
    private PhoneNotifications notifications;
    public GameObject TransitionScreen;
    public HashSet<string> appNotifications;
    private Player player;

    private AudioSource audioSource;
    public AudioClip lockSound;
    public AudioClip unlockSound;
    public AudioClip txtRingtone;
    public AudioClip rcvTextSound;
    public AudioClip sntTextSound;

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
        Gear,
        Skate
    };

    // State stack
    private Stack<PhoneState> phoneStateStack;

    // Start is called before the first frame update
    void Start()
    {
        backgroundResolver = background.GetComponent<SpriteResolver>();
        phoneStateStack = new Stack<PhoneState>();
        phoneStateStack.Push(PhoneState.Home);
        bankApp = this.GetComponentInChildren<BankApp>(true);
        mapsApp = this.GetComponentInChildren<MapsApp>(true);
        messagesApp = this.GetComponentInChildren<PhoneMessages>(true);
        calendarApp = this.GetComponentInChildren<CalendarApp>(true);
        pocketsApp = this.GetComponentInChildren<PocketsApp>(true);
        decoratorApp = this.GetComponentInChildren<DecoratorApp>(true);
        gearApp = this.GetComponentInChildren<GearApp>(true);
        skateApp = this.GetComponentInChildren<SkateApp>(true);
        settingsApp = this.GetComponentInChildren<SettingsApp>(true);
        photosApp = this.GetComponentInChildren<PhotosApp>(true);
        notifications = this.GetComponentInChildren<PhoneNotifications>();
        audioSource = this.GetComponent<AudioSource>();
        isLocked = true;
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        player = ReInput.players.GetPlayer(0);
        //if (appNotifications == null) appNotifications = new HashSet<string>();

        Load(); // appNotifications get set here

        //if (messagesApp.HasPendingConvos())
        //    appNotifications.Add("Messages");

        Lock();
        //txtResponsePanel = this.GetComponentInChildren<PixelCrushers.DialogueSystem.Wrappers.StandardUIMenuPanel>();
    }

    private void OnEnable()
    {
        if (backgroundResolver == null)
            return;
        Lock();
    }

    public void PlayTxtRcvSound()
    {
        if (phoneStateStack.Peek() != PhoneState.Convo)
            return;
        audioSource.clip = rcvTextSound;
        audioSource.Play();
    }

    public void PlayTxtSntSound()
    {
        if (phoneStateStack.Peek() != PhoneState.Convo)
            return;
        audioSource.clip = sntTextSound;
        audioSource.Play();
    }

    public void UpdateRomanceEmoji(string npc)
    {
        messagesApp.UpdateRomanceEmoji(npc);
    }

    public void UnlockEmoji(string contactName, string emojiName)
    {
        messagesApp.UnlockEmoji(contactName, emojiName);
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public void Reset()
    {
        messagesApp.Reset();
        appNotifications.Clear();
        decoratorApp.Reset();
        gearApp.Reset();
        skateApp.Reset();
    }

    public void SendNotificationTo(string app, string arg)
    {
        Debug.Log("Send notifs for: " + app);
        if (appNotifications == null)
            appNotifications = new HashSet<string>();
        appNotifications.Add(app);
        switch (app)
        {
            case "Decorator":
                decoratorApp.AddNotification(arg);
                break;
            case "Gear":
                gearApp.AddNotification(arg);
                break;
            case "Skate":
                skateApp.AddNotification(arg);
                break;
            default:
                break;
        }
    }

    public void SendNotificationTo(string app)
    {
        Debug.Log("Send notifs for: " + app);
        if (appNotifications == null)
            appNotifications = new HashSet<string>();
        appNotifications.Add(app);
        //switch (app)
        //{
        //    case "Calendar":
        //        calendarApp.ShowNotification();
        //        break;
        //    case "Messages":
        //        messagesApp.ShowNotification();
        //        break;
        //    case "Pockets":
        //        pocketsApp.ShowNotification();
        //        break;
        //    case "Decorator":
        //        decoratorApp.ShowNotification();
        //        break;
        //    case "Maps":
        //        mapsApp.ShowNotification();
        //        break;
        //    case "Gear":
        //        gearApp.ShowNotification();
        //        break;
        //    case "Bank":
        //        bankApp.ShowNotification();
        //        break;
        //    case "Settings":
        //        settingsApp.ShowNotification();
        //        break;
        //    case "Photos":
        //        // TODO: add photos app
        //        break;
        //    default:
        //        break;
        //}
    }

    public void ClearNotificationFor(string app)
    {
        Debug.Log("Clear notifs for: " + app);
        if (appNotifications == null)
            return;
        appNotifications.Remove(app);
    }

    public PocketsApp GetPocketsApp()
    {
        return pocketsApp;
    }

    public void ChangePhoneColor (Color c) {
        Debug.Log("CHANGE COLOR to: "+ c.r + " " + c.g + " " + c.b);
        HUDIcon.GetComponent<SpriteRenderer>().color = c;
        PhoneCase.GetComponent<SpriteRenderer>().color = c;
    }

    public void NotificationMessage(string txt)
    {
        notifications.Add(txt);
    }

    //private void OnApplicationQuit()
    //{
    //    Save();
    //}

    public void Save()
    {
        if (appNotifications == null)
            return;

        foreach (PhoneApp app in GetComponentsInChildren<PhoneApp>(true))
        {
            app.Save();
        }
        ES3.Save("AppNotif", appNotifications);
        ES3.Save("PhoneColor", HUDIcon.GetComponent<SpriteRenderer>().color);
    }

    public void Load()
    {
        // Apps should load automatically in their start() method...
        appNotifications = ES3.Load("AppNotif", new HashSet<string>());
        ChangePhoneColor(ES3.Load("PhoneColor", Color.white));
    }

    // Update is called once per frame
    void Update()
    {

        // if we don't have the convo open rn, stop the convo when it needs our response
        if (!phoneStateStack.Peek().Equals(PhoneState.Convo) && customDialogue.IsTxtConvoActive() && !DialogueManager.lastConversationStarted.Contains("Opt"))
        {
            txtResponsePanel.Close();
            customDialogue.StopCurrentConvo();
        }

        if (!phoneStateStack.Peek().Equals(PhoneState.Convo) && txtResponsePanel.isActiveAndEnabled)
        {
            txtResponsePanel.gameObject.SetActive(false);
        }

        if (phoneStateStack.Peek().Equals(PhoneState.Convo) && !txtResponsePanel.isActiveAndEnabled)
        {
            txtResponsePanel.gameObject.SetActive(true);
        }

        // Don't let the HUD icon block speech bubbles
        if (DialogueManager.IsConversationActive && !customDialogue.IsTxtConvoActive())
        {
            HUDIcon.SetActive(false);
        } else
        {
            HUDIcon.SetActive(true);
            if (isLocked)
            {
                HUDIcon.GetComponent<Animator>().SetBool("Locked", true);
                HUDIcon.GetComponent<SpriteResolver>().SetCategoryAndLabel("PhoneHUDIcon", "Locked");
            }
            else
            {
                HUDIcon.GetComponent<Animator>().SetBool("Locked", false);
                HUDIcon.GetComponent<SpriteResolver>().SetCategoryAndLabel("PhoneHUDIcon", "Unlocked");
            }
        }
        if (appNotifications.Count > 0)
            EnableNotificationIndicator();
        else
            DisableNotificationIndicator();


        if (player.GetButtonDown("Toggle Phone"))
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
        return AddSpaceBeforeCapitals(convoName.Split('/')[1]);
    }

    public static string AddSpaceBeforeCapitals(string input)
    {
        // Regex pattern: Look for a capital letter that is preceded by a non-space character (except at the start of the string)
        if (input=="JJ")
            return input;
        return Regex.Replace(input, "(?<!^|\\s)([A-Z])", " $1");
        //return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
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
        ClearNotificationFor("Messages");
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

    public void ReceiveMsg(string conversation, bool forceOpenPhone = false)
    {
        audioSource.clip = txtRingtone;
        audioSource.Play();
        if (forceOpenPhone)
        {
            gameObject.SetActive(true);
            GoHome();
            Unlock();
        }
        messagesApp.ReceiveMsg(GetContactNameFromConvoName(conversation), conversation);
        if (conversation.Contains("Opt"))
            DialogueManager.Instance.StartConversation(conversation);
        if (forceOpenPhone)
            OpenTxtConvoWith(GetContactNameFromConvoName(conversation));
        else
            appNotifications.Add("Messages");
    }

    public void OpenBank()
    {
        SetAppHeader("Hellcorp Bank");
        ClearNotificationFor("Bank");
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
        ClearNotificationFor("Maps");
        SetAppHeader("");
        if (phoneStateStack.Peek() != PhoneState.Map)
            phoneStateStack.Push(PhoneState.Map);
        HideIcons();
    }

    public void OpenCalendar()
    {
        if (Instance == null)
            return;
        
        ClearNotificationFor("Calendar");
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
        ClearNotificationFor("Pockets");
        pocketsApp.gameObject.SetActive(true);
        SetAppHeader("Pocketz");
        if (phoneStateStack.Peek() != PhoneState.Pockets)
            phoneStateStack.Push(PhoneState.Pockets);
        HideIcons();
    }

    public void OpenGear()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Gear");
        ClearNotificationFor("Gear");
        gearApp.gameObject.SetActive(true);
        SetAppHeader("Music Gear");
        if (phoneStateStack.Peek() != PhoneState.Gear)
            phoneStateStack.Push(PhoneState.Gear);
        HideIcons();
    }

    public void OpenSkate()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Skate");
        ClearNotificationFor("Skate");
        skateApp.gameObject.SetActive(true);
        SetAppHeader("Skate Gear");
        if (phoneStateStack.Peek() != PhoneState.Skate)
            phoneStateStack.Push(PhoneState.Skate);
        HideIcons();
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
        if (Camera.main == null)
            return;
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
        ClearNotificationFor("Settings");
        if (phoneStateStack.Peek() != PhoneState.Settings)
            phoneStateStack.Push(PhoneState.Settings);
        HideIcons();
        settingsApp.gameObject.SetActive(true);
    }

    public void OpenPhotos()
    {
        SetAppHeader("Photos");
        ClearNotificationFor("Photos");
        if (phoneStateStack.Peek() != PhoneState.Photos)
            phoneStateStack.Push(PhoneState.Photos);
        HideIcons();
        photosApp.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        PhoneState state = phoneStateStack.Peek();
        // Force text messages to be answered
        if (state.Equals(PhoneState.Convo) && customDialogue.IsTxtConvoActive())
        {
            return;
        }

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

            case PhoneState.Skate:
                OpenSkate();
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
        appNotifications.Remove("Messages");
        //DisableNotificationIndicator();
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
        skateApp.gameObject.SetActive(false);
        settingsApp.gameObject.SetActive(false);
        photosApp.gameObject.SetActive(false);
        backButton.SetActive(false);
        ShowIcons();
    }

    private void ShowIcons()
    {   
        icons.SetActive(true);

        // Lock the maps app until the appropriate time in the tutorial
        GameObject mapIcon = icons.GetComponentsInChildren<PhoneIcon>(true).First(i => i.appName == "Maps").gameObject;
        if (!Tutorial.joinedTheBand)
        {
            mapIcon.SetActive(false);
        }
        else if (!mapIcon.activeSelf && Tutorial.joinedTheBand)
        {
            mapIcon.SetActive(true);
            SendNotificationTo("Maps");
        }
    }

    public void ToggleLock()
    {
        bool prev = isLocked;
        isLocked = !isLocked;
        if (isLocked)
            Lock();
        else
            Unlock();
        if (prev != isLocked)
            Tutorial.hasClosedPhone = true;
    }

    public void Lock()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null)
            return;

        isLocked = true;
        // For text responses
        if (DialogueManager.isConversationActive && customDialogue.IsCurrentConvoTxt())
        {
            isLocked = false;
            return;
        }
        audioSource.clip = lockSound;
        audioSource.Play();
        HUDIcon.GetComponent<Animator>().SetBool("Locked", true);
        HUDIcon.GetComponent<SpriteResolver>().SetCategoryAndLabel("PhoneHUDIcon", "Locked");
        GoHome();

        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            float deltaX = background.transform.localPosition.x - mainCam.gameObject.transform.position.x;
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
        audioSource.clip = unlockSound;
        audioSource.Play();
        GoHome();
        foreach (PhoneIcon i in icons.GetComponentsInChildren<PhoneIcon>())
            i.Refresh();
        HUDIcon.SetActive(true);
        HUDIcon.GetComponent<Animator>().SetBool("Locked", false);
        HUDIcon.GetComponent<SpriteResolver>().SetCategoryAndLabel("PhoneHUDIcon", "Unlocked");
        float deltaX = background.transform.localPosition.x - Camera.main.gameObject.transform.position.x;
        float deltaY = background.transform.localPosition.y - Camera.main.gameObject.transform.position.y;
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("Menu"))
                continue;
            // Move each child object
            
            Vector3 target = child.transform.localPosition + (Vector3.up * -deltaY) + (Vector3.left * deltaX);
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

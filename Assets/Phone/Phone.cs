using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.U2D.Animation;
using UnityEngine.SceneManagement;

public class Phone : MonoBehaviour
{
    public static Phone Instance;
    public GameObject icons;
    public GameObject background;
    public GameObject backButton;
    public AppHeader appHeader;
    private PhoneMessages messagesApp;
    private BankApp bankApp;
    private MapsApp mapsApp;
    private SpriteResolver backgroundResolver;
    private bool isLocked;
    private Animator animator;
    
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
        SceneManager.activeSceneChanged += ChangedActiveScene;
        backgroundResolver = background.GetComponent<SpriteResolver>();
        phoneStateStack = new Stack<PhoneState>();
        phoneStateStack.Push(PhoneState.Home);
        //contactsList = SaveSystem.LoadContactsList();
        bankApp = this.GetComponentInChildren<BankApp>();
        mapsApp = this.GetComponentInChildren<MapsApp>();
        messagesApp = this.GetComponentInChildren<PhoneMessages>();
        animator = this.GetComponent<Animator>();
        GoHome();
        isLocked = true;
        Lock();
    }

    // Update is called once per frame
    void Update()
    {
        //if (animator.enabled && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    animator.enabled = false;
        //}
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        string currentName = current.name;

        if (currentName == null)
        {
            // Scene1 has been removed
            currentName = "Replaced";
        }

        Canvas c = this.GetComponent<Canvas>();
        c.worldCamera = Camera.main;

        Debug.Log("Scenes: " + currentName + ", " + next.name);
    }

    public void GoHome()
    {
        phoneStateStack = new Stack<PhoneState>();
        phoneStateStack.Push(PhoneState.Home);
        // Hide all app content and show icons
        CloseApps();
        // Change background
        backgroundResolver.SetCategoryAndLabel("Background", "Home");
    }

    public void OpenMessages()
    {
        SetAppHeader("Messages");
        if (phoneStateStack.Peek() != PhoneState.Messages)
            phoneStateStack.Push(PhoneState.Messages);
        // Show messages interface
        messagesApp.gameObject.SetActive(true);
        messagesApp.OpenContacts();
        // Hide icons
        HideIcons();
        // Change background
        backgroundResolver.SetCategoryAndLabel("Background", "Messages");
    }

    public void OpenConvo()
    {
        appHeader.gameObject.SetActive(false);
        if (phoneStateStack.Peek() != PhoneState.Convo)
            phoneStateStack.Push(PhoneState.Convo);
    }

    public void OpenPin()
    {
        backgroundResolver.SetCategoryAndLabel("Background", "Maps2");
        if (phoneStateStack.Peek() != PhoneState.Pin)
            phoneStateStack.Push(PhoneState.Pin);
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
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenSettings()
    {
        SetAppHeader("Settings");
        if (phoneStateStack.Peek() != PhoneState.Settings)
            phoneStateStack.Push(PhoneState.Settings);
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenPhotos()
    {
        SetAppHeader("Photos");
        if (phoneStateStack.Peek() != PhoneState.Photos)
            phoneStateStack.Push(PhoneState.Photos);
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
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
    private void Lock()
    {
        foreach (Transform child in transform)
        {
            // Move each child object
            child.Translate(Vector3.down * 7);
        }
        transform.Translate(Vector3.down * 7);
        //this.transform.position = new Vector3(-1.141271f, -6.38f, -0.5258861f);
    }

    private void Unlock()
    {
        //animator.enabled = true;
        //animator.ResetTrigger("UnlockPhone");
        //animator.Play("Phone_Open_Anim", 0, 0f);
        //animator.SetTrigger("UnlockPhone");

        foreach (Transform child in transform)
        {
            // Move each child object
            child.Translate(Vector3.up * 7);
        }
        transform.Translate(Vector3.up * 7);
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

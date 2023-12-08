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
    public PhoneMessages messages;
    public BankApp bankApp;
    private SpriteResolver backgroundResolver;
    private bool isLocked;
    
    enum PhoneState
    {
        Home,
        Messages,
        Map,
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
        GoHome();
        isLocked = true;
        //Lock();
    }

    // Update is called once per frame
    void Update()
    {
      
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
        messages.gameObject.SetActive(true);
        messages.OpenContacts();
        // Hide icons
        HideIcons();
        // Change background
        backgroundResolver.SetCategoryAndLabel("Background", "Messages");
    }

    public void OpenConvo()
    {
        // TODO PHONE UI CHANGES HERE
        appHeader.gameObject.SetActive(false);
        phoneStateStack.Push(PhoneState.Convo);
    }

    public void OpenBank()
    {
        SetAppHeader("Hellcorp Bank");
        bankApp.gameObject.SetActive(true);
        bankApp.UpdateBankBalance();
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
        SetAppHeader("Map");
        phoneStateStack.Push(PhoneState.Map);
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenSettings()
    {
        SetAppHeader("Settings");
        phoneStateStack.Push(PhoneState.Settings);
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenPhotos()
    {
        SetAppHeader("Photos");
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
        messages.gameObject.SetActive(false);
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
        foreach (Transform child in transform)
        {
            // Move each child object
            child.Translate(Vector3.up * 7);
        }
        transform.Translate(Vector3.up * 7);
        //this.transform.position = new Vector3(-1.141271f, 0.140902f, -0.5258861f);
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

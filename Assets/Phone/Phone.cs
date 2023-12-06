using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.U2D.Animation;

public class Phone : MonoBehaviour
{
    public GameObject icons;
    public GameObject background;
    public GameObject backButton;
    public PhoneMessages messages;
    private SpriteResolver backgroundResolver;
    private bool isLocked;
    enum PhoneState
    {
        Home,
        Messages,
        Map,
        Settings,
        Photos,
        Convo
    };

    // State stack
    private Stack<PhoneState> phoneStateStack;

    // Start is called before the first frame update
    void Start()
    {
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

    public void GoHome()
    {
        // Hide all app content and show icons
        CloseApps();
        // Change background
        backgroundResolver.SetCategoryAndLabel("Background", "Home");
    }

    public void OpenMessages()
    {
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
        phoneStateStack.Push(PhoneState.Convo);
    }

    public void OpenMap()
    {
        phoneStateStack.Push(PhoneState.Map);
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenSettings()
    {
        phoneStateStack.Push(PhoneState.Settings);
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenPhotos()
    {
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

            case PhoneState.Home:
                GoHome();
                break;

            default:
                Debug.Log("State not found: " + state.ToString());
                break;
        }
    }

    private void HideIcons()
    {
        backButton.SetActive(true);
        icons.SetActive(false);
    }

    private void CloseApps()
    {
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
}

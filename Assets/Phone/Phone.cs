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
    public GameObject messagesApp;
    public PhoneMessages messages;
    private SpriteResolver backgroundResolver;
    private List<string> contactsList;
    enum PhoneState
    {
        Home,
        Messages,
        Map,
        Settings,
        Photos
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
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void GoHome()
    {
        // Hide all app content and show icons
        CloseApps();
        //DialogueManager.
        // Change background
        backgroundResolver.SetCategoryAndLabel("Background", "Home");
    }

    public void OpenMessages()
    {
        // Show messages interface
        //ShowContacts();
        messagesApp.SetActive(true);
        // Hide icons
        HideIcons();
        //Testing!!!! TODO change this
        messages.StartTestConvo();
        // Change background
        backgroundResolver.SetCategoryAndLabel("Background", "Messages");
    }

    private void ShowContacts()
    {
        foreach (string contact in contactsList)
        {

        }
    }

    public void OpenMap()
    {
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenSettings()
    {
        // Show messages interface
        // Hide icons
        HideIcons();
        // Change background
    }

    public void OpenPhotos()
    {
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

        state = phoneStateStack.Pop();
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
        messagesApp.SetActive(false);
        backButton.SetActive(false);
        ShowIcons();
    }

    private void ShowIcons()
    {   
        icons.SetActive(true);
    }

    //private class PhoneState
    //{

    //}
}

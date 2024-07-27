using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneMessages : PhoneApp
{
    public GameObject contacts;
    public GameObject contactsCanvas;
    public Contact contactTemplate;
    private HashSet<string> contactsList;
    private Dictionary<string, Contact> contactsMap;
    private CustomDialogueScript customDialogue;
    private List<GameObject> instances = new List<GameObject>();
    private Dictionary<string, string> unfinishedConversations; // contact name to name of conversation

    // Start is called before the first frame update
    void Start()
    {
        // wait until message app is opened before enabling the canvas
        contactsMap = new Dictionary<string, Contact>();

        // load contacts
        contactsList = new HashSet<string>();//new HashSet<string> { "Ricki", "Max", "Band", "Mom" };//SaveSystem.LoadContactsList();
        unfinishedConversations = new Dictionary<string, string>();
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        foreach (string c in contactsList)
        {
            AddContact(c);
        }
        customDialogue.CloseBacklogs();
    }

    private void Awake()
    {
        contactTemplate.gameObject.SetActive(false);
    }

    public bool HasPendingConvos()
    {
        return unfinishedConversations.Count > 0;
    }

    public void OpenTxtConvoWith(string contact)
    {
        CloseContacts();
        Phone.Instance.OpenConvo();
        if (ContactHasPendingConvo(contact))
            customDialogue.ResumeTxtConvo(contact, unfinishedConversations[contact]);
        else
        {
            customDialogue.FocusBackLog(contact);
        }
    }

    public void ReceiveMsg(string contactName, string conversation)
    {
        Phone.Instance.SendNotificationTo("Messages");
        if (unfinishedConversations.ContainsKey(contactName))
            return;
        unfinishedConversations.Add(contactName, conversation);
        if (!contactsList.Contains(contactName))
            AddContact(contactName);
        contactsMap[contactName].ShowNotificationIndicator();
    }

    public void CompleteConvo(string contactName)
    {
        unfinishedConversations.Remove(contactName);
        contactsMap[contactName].HideNotificationIndicator();
        if (unfinishedConversations.Count == 0)
            Phone.Instance.ClearNotificationFor("Messages");
    }

    private bool ContactHasPendingConvo(string contact)
    {
        return unfinishedConversations.ContainsKey(contact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenContacts()
    {
        contactsCanvas.SetActive(true);
        customDialogue.CloseBacklogs();
        foreach (string c in contactsList)
        {
            if (unfinishedConversations.ContainsKey(c))
            {
                contactsMap[c].ShowNotificationIndicator();
            } else
            {
                contactsMap[c].HideNotificationIndicator();
            }
        }
    }

    void CloseContacts()
    {
        contactsCanvas.SetActive(false);
    }

    void AddContact(string contactName)
    {
        contactsList.Add(contactName);
        Debug.Log("Create contact for: " + contactName);
        Contact instance = Instantiate(contactTemplate, contacts.transform);
        instance.gameObject.SetActive(true);
        instance.SetContact(contactName);
        if (unfinishedConversations.ContainsKey(contactName))
        {
            instance.ShowNotificationIndicator();
        }
        instances.Add(instance.gameObject);
        contactsMap.Add(contactName, instance);
        customDialogue.AddBackLog(contactName);
    }

}

using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneMessages : MonoBehaviour
{
    public GameObject contacts;
    public Phone phone;
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
        contactsList = new HashSet<string> { "Ricki", "Max", "Band" };//SaveSystem.LoadContactsList();
        unfinishedConversations = new Dictionary<string, string>();
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        foreach (string c in contactsList)
        {
            Debug.Log("Create contact for: " + c);
            Contact instance = Instantiate(contactTemplate, contacts.transform);
            instance.gameObject.SetActive(true);
            instance.SetContact(c);
            if (unfinishedConversations.ContainsKey(c))
            {
                instance.ShowNotificationIndicator();
            }
            instances.Add(instance.gameObject);
            contactsMap.Add(c, instance);
            customDialogue.AddBackLog(c);
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
        phone.OpenConvo();
        if (ContactHasPendingConvo(contact))
            customDialogue.ResumeTxtConvo(contact, unfinishedConversations[contact]);
        else
        {
            customDialogue.FocusBackLog(contact);
        }
    }

    public void ReceiveMsg(string contactName, string conversation)
    {
        if (unfinishedConversations.ContainsKey(contactName))
            return;
        unfinishedConversations.Add(contactName, conversation);
        contactsMap[contactName].ShowNotificationIndicator();
    }

    public void CompleteConvo(string contactName)
    {
        unfinishedConversations.Remove(contactName);
        contactsMap[contactName].HideNotificationIndicator();
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
        contacts.SetActive(true);
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
        contacts.SetActive(false);
    }

    void AddContact(string contactName)
    {
        contactsList.Add(name);
    }

}

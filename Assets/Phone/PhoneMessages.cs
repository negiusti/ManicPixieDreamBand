using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneMessages : MonoBehaviour
{
    public GameObject contacts;
    public Phone phone;
    public Contact contactTemplate;
    private HashSet<string> contactsList;
    public CustomDialogueScript customDialogue;
    private List<GameObject> instances = new List<GameObject>();
    private Dictionary<string, string> unfinishedConversations; // contact name to name of conversation

    // Start is called before the first frame update
    void Start()
    {
        // wait until message app is opened before enabling the canvas
        //canvas.enabled = false;

        // load contacts
        contactsList = new HashSet<string> { "Ricki", "Max", "Band" };//SaveSystem.LoadContactsList();
        unfinishedConversations = new Dictionary<string, string>();
        foreach (string c in contactsList)
        {
            Debug.Log("Create contact for: " + c);
            Contact instance = Instantiate(contactTemplate, contacts.transform);
            instance.gameObject.SetActive(true);
            instance.SetContact(c);
            instances.Add(instance.gameObject);
            customDialogue.AddBackLog(c);
        }

        customDialogue.CloseBacklogs();
    }

    private void Awake()
    {
        contactTemplate.gameObject.SetActive(false);
    }

    public void OpenTxtConvoWith(string contact)
    {
        CloseContacts();
        phone.OpenConvo();
        if (ContactHasPendingConvo(contact))
            customDialogue.ResumeTxtConvo(contact, unfinishedConversations[contact]);
    }

    public void ReceiveMsg(string contactName, string conversation)
    {
        unfinishedConversations.Add(contactName, conversation);
    }

    public void CompleteConvo(string contactName)
    {
        unfinishedConversations.Remove(contactName);
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

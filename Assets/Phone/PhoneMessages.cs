using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneMessages : MonoBehaviour
{
    //public Canvas canvas;
    public GameObject contacts;
    //public GameObject convo;
    public Phone phone;
    public Contact contactTemplate;
    private HashSet<string> contactsList;
    public CustomDialogueScript customDialogue;
    private List<GameObject> instances = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // wait until message app is opened before enabling the canvas
        //canvas.enabled = false;
        customDialogue.CloseBacklogs();

        // load contacts
        contactsList = new HashSet<string> { "Ricki", "Max", "Band" };//SaveSystem.LoadContactsList();

        foreach (string c in contactsList)
        {
            Debug.Log("Create contact for: " + c);
            Contact instance = Instantiate(contactTemplate, contacts.transform);
            instance.gameObject.SetActive(true);
            instance.setContact(c);
            instances.Add(instance.gameObject);
            customDialogue.AddBackLog(c);
        }
    }

    private void Awake()
    {
        contactTemplate.gameObject.SetActive(false);
    }

    public void OpenTxtConvoWith(string contact)
    {
        CloseContacts();
        phone.OpenConvo();
        customDialogue.FocusBackLog(contact);
        customDialogue.ResumeTxtConvo(contact);
        //DialogueManager.StartConversation("TXT_" + contact);
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

    //public void StartTestConvo()
    //{
    //    canvas.enabled = true;
    //    DialogueManager.StartConversation("TXT_Band");
    //    //DialogueManager.conversationController.
    //}
}

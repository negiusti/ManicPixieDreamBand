using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneMessages : MonoBehaviour
{
    public Canvas canvas;
    public GameObject contacts;
    public Contact contactTemplate;
    private List<string> contactsList;

    // Start is called before the first frame update
    void Start()
    {
        // wait until message app is opened before enabling the canvas
        canvas.enabled = false;

        // load contacts
        contactsList = SaveSystem.LoadContactsList();
        foreach (string c in contactsList)
        {
            Contact instance = Instantiate(contactTemplate, contacts.transform);
            instance.setContact(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddContact(string contactName)
    {
        contactsList.Add(name);
    }

    public void StartTestConvo()
    {
        canvas.enabled = true;
        DialogueManager.StartConversation("TXT_Group_Band");
        //DialogueManager.conversationController.
    }
}

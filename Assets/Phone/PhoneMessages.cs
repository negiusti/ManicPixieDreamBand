using System.Collections.Generic;
using System.Linq;
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
    private List<GameObject> instances = new List<GameObject>(); // contact game objects
    private Dictionary<string, string> unfinishedConversations; // contact name to name of conversation

    // Start is called before the first frame update
    void Start()
    {
        contactTemplate.gameObject.SetActive(false);
        // wait until message app is opened before enabling the canvas
        contactsMap = new Dictionary<string, Contact>();
        customDialogue = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        Load();
        customDialogue.CloseBacklogs();
    }

    public void Reset()
    {
        contactsList = new HashSet<string>();
        unfinishedConversations = new Dictionary<string, string>();
        foreach(Contact c in contactsMap.Values)
        {
            Destroy(c.gameObject);
        }
        contactsMap.Clear();
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
        if (unfinishedConversations == null)
            Start();
        if (unfinishedConversations.ContainsKey(contactName))
            return;
        unfinishedConversations.Add(contactName, conversation);
        if (!contactsList.Contains(contactName))
            AddContact(contactName);
        contactsMap[contactName].ShowNotificationIndicator();
        contactsMap[contactName].transform.SetAsFirstSibling();
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

    public override void Save()
    {
        Debug.Log("Saving contacts");
        ES3.Save("PhoneContacts", contactsList);
        ES3.Save("UnfinishedConversations", unfinishedConversations);
        GetComponentsInChildren<BackLog>(true).ToList().ForEach(c => c.SaveContact());
        GetComponentsInChildren<Emojis>(true).ToList().ForEach(e => e.SaveEmojis());
    }

    public void UpdateRomanceEmoji(string npc)
    {
        contactsMap[npc].UpdateRomanceEmoji();
    }

    public void UnlockEmoji(string contactName, string emojiName)
    {
        if (contactsMap[contactName] == null)
            return;// contact does not exist yet
        contactsMap[contactName].UnlockEmoji(emojiName);
    }

    public override void Load()
    {
        Debug.Log("loading contacts");
        contactsList = ES3.Load("PhoneContacts",new HashSet<string>());
        if (contactsList == null)
            contactsList = new HashSet<string>();
        unfinishedConversations = ES3.Load("UnfinishedConversations", new Dictionary<string, string>());
        if (unfinishedConversations == null)
            unfinishedConversations = new Dictionary<string, string>();
        foreach (string c in contactsList)
        {
            AddContact(c);
            Debug.Log("loading contact: " + c);
        }
    }
}

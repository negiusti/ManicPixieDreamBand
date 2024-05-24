using System.Collections.Generic;
using UnityEngine;

public class PhoneNotifications : MonoBehaviour
{
    private List<PhoneNotification> notifications;
    public PhoneNotification templateNotification;

    // Start is called before the first frame update
    void Start()
    {
        notifications = new List<PhoneNotification>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(string txt)
    {
        PhoneNotification notification = Instantiate(templateNotification, transform);
        notification.gameObject.SetActive(true);
        notification.SetText(txt);
        notifications.Add(notification);
    }
}

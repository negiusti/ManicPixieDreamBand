using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneNotifications : MonoBehaviour
{
    private Queue<PhoneNotification> notifications;
    public PhoneNotification templateNotification;

    // Start is called before the first frame update
    void Start()
    {
        notifications = new Queue<PhoneNotification>();
        StartCoroutine(DeleteTopNotif());
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
        notifications.Enqueue(notification);
    }

    private IEnumerator DeleteTopNotif()
    {
      while(true)
        {
            if (notifications.Count > 0)
            {
                yield return new WaitForSeconds(5f);
                Destroy(notifications.Dequeue().gameObject);
            }
            yield return new WaitForSeconds(5f);
        }

    }
}

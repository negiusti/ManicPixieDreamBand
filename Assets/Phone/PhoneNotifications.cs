using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneNotifications : MonoBehaviour
{
    private Queue<PhoneNotification> notifications;
    public PhoneNotification templateNotification;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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
        if (notifications.Count == 1)
        {
            notification.SetTop();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    private IEnumerator DeleteTopNotif()
    {
      while(true)
        {
            if (notifications.Count > 0)
            {
                PhoneNotification topNotif = notifications.Dequeue();
                topNotif.SetTop();
                yield return new WaitForSeconds(2.5f);
                Destroy(topNotif.gameObject);
                if (notifications.Count > 0)
                {
                    notifications.Peek().SetTop();
                }
            } else
            {
                yield return new WaitForSeconds(2.5f);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneNotifications : MonoBehaviour
{
    private Queue<PhoneNotification> notifications;
    public PhoneNotification templateNotification;
    private RectTransform rectTransform;
    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("STARTING NOTIF COROUTINE");
        if (coroutine == null)
            coroutine = StartCoroutine(DeleteTopNotif());
    }

    private void OnEnable()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(DeleteTopNotif());
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        notifications = new Queue<PhoneNotification>();
    }

    private void OnDisable()
    {
        Debug.Log("STOPPING NOTIF COROUTINE");
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(DeleteTopNotif());
        }
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
        Debug.Log("STARTING NOTIF COROUTINE DeleteTopNotif func");
        while (true)
        {
            if (notifications.Count > 0)
            {
                Debug.Log("notifications.Count > 0");
                PhoneNotification topNotif = notifications.Dequeue();
                topNotif.SetTop();
                yield return new WaitForSeconds(2.5f);
                topNotif.Hide();
                if (notifications.Count > 0)
                {
                    notifications.Peek().SetTop();
                }
                yield return new WaitForSeconds(1f);
                Destroy(topNotif.gameObject);
            } else
            {
                Debug.Log("notifications.Count < 0");
                yield return new WaitForSeconds(2.5f);
            }
        }

    }
}

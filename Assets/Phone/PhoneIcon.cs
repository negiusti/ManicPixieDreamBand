using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneIcon : MonoBehaviour
{
    private GameObject notifIndicator;
    public string appName;
    //private bool notif;
    // Start is called before the first frame update
    void Start()
    {
        notifIndicator = transform.GetChild(0).gameObject;
        //notif = false;
    }

    private void OnEnable()
    {
        if (notifIndicator == null)
            Start();
        if (Phone.Instance == null)
            return;
        if (Phone.Instance.appNotifications.Contains(appName))
        {
            ShowNotificationIndicator();
        } else
        {
            HideNotificationIndicator();
        }
    }

    public void ShowNotificationIndicator()
    {
        if (notifIndicator == null)
            Start();
        //notif = true;
        notifIndicator.SetActive(true);
    }

    public void HideNotificationIndicator()
    {
        if (notifIndicator == null)
            Start();
        //notif = false;
        notifIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneIcon : MonoBehaviour
{
    private GameObject notifIndicator;
    public string appName;

    void Start()
    {
        notifIndicator = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        if (notifIndicator == null)
            Start();
        Refresh();
    }

    public void Refresh()
    {
        if (Phone.Instance == null)
            return;
        if (Phone.Instance.appNotifications.Contains(appName))
        {
            ShowNotificationIndicator();
        }
        else
        {
            HideNotificationIndicator();
        }
    }

    public void ShowNotificationIndicator()
    {
        if (notifIndicator == null)
            Start();
        notifIndicator.SetActive(true);
    }

    public void HideNotificationIndicator()
    {
        if (notifIndicator == null)
            Start();
        notifIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

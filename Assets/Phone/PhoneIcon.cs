using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneIcon : MonoBehaviour
{
    private GameObject notifIndicator;   
    // Start is called before the first frame update
    void Start()
    {
        notifIndicator = transform.GetChild(0).gameObject;
        notifIndicator.SetActive(false);
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

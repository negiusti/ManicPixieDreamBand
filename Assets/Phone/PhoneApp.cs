using UnityEngine;
using System.Collections;

public class PhoneApp : MonoBehaviour
{
    public PhoneIcon phoneIcon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowNotification()
    {
        phoneIcon.ShowNotificationIndicator();
    }
}

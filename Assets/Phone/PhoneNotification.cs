using UnityEngine;
using TMPro;

public class PhoneNotification : MonoBehaviour
{
    private TextMeshPro notificationText;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        notificationText = this.GetComponent<TextMeshPro>();
        animator = this.GetComponent<Animator>();
        animator.CrossFade("NotificationText_Show", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string txt)
    {
        if (notificationText == null)
            notificationText = this.GetComponent<TextMeshPro>();
        notificationText.text = txt;
    }

    //private void Show()
    //{        
    //    animator.CrossFade("NotificationText_Show", 0.5f);
    //}
}

using UnityEngine;
using TMPro;

public class PhoneNotification : MonoBehaviour
{
    private TextMeshPro notificationText;
    private Animator animator;
    private bool isTop;
    // Start is called before the first frame update
    void Start()
    {
        notificationText = this.GetComponentInChildren<TextMeshPro>(includeInactive:true);
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string txt)
    {
        if (notificationText == null)
            notificationText = this.GetComponentInChildren<TextMeshPro>();
        notificationText.text = txt;
    }

    public void SetTop()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
        if (!isTop)
            animator.Play("NotificationText_Show");
        isTop = true;
    }

    //private void Show()
    //{        
    //    animator.CrossFade("NotificationText_Show", 0.5f);
    //}
}

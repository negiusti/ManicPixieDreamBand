using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PhoneNotification : MonoBehaviour
{
    private TextMeshPro notificationText;
    private Animator animator;
    private bool isTop;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        notificationText = this.GetComponentInChildren<TextMeshPro>(includeInactive:true);
        animator = this.GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string txt)
    {
        if (notificationText == null)
            Start();
        notificationText.text = txt;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void SetTop()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
        if (!isTop)
            animator.Play("NotificationText_Show");
        isTop = true;
    }

    public void Hide()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
        animator.Play("NotificationText_Hide");
    }

    //private void Show()
    //{        
    //    animator.CrossFade("NotificationText_Show", 0.5f);
    //}
}

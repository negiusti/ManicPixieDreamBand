using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhoneCalendarEvent : MonoBehaviour
{
    private TMP_Text tmp;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        tmp = this.GetComponentInChildren<TMP_Text>();
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignEvent(CalendarEvent e)
    {
        if (e == null)
            Debug.LogError("e is null");
        if (tmp == null)
            Debug.LogError("mtp is null");
        tmp.text = e.Name();
    }

    public void Complete()
    {
        sr.color = Color.gray;
    }
}

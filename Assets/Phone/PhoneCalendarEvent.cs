using TMPro;
using UnityEngine;

public class PhoneCalendarEvent : MonoBehaviour
{
    private TMP_Text tmp;
    private SpriteRenderer sr;
    private ICalendarEvent e;
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

    public void AssignEvent(ICalendarEvent e)
    {
        this.e = e;
        tmp.text = e.Name();
    }

    public void Complete()
    {
        sr.color = Color.gray;
    }

    public void NotComplete()
    {
        sr.color = Color.white;
    }
}

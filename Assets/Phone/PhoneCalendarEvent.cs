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
        if (e is JobEvent)
            tmp.text = "Work at " + JobSystem.CurrentJobInfo().Location();
        else
            tmp.text = e.Name();
    }

    public void Complete()
    {
        sr.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void NotComplete()
    {
        sr.color = Color.white;
    }
}

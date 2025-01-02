using UnityEngine;

public class EventOnly : MonoBehaviour
{
    public string EventName;

    // Start is called before the first frame update
    void Start()
    {
        if (!Calendar.GetCurrentEvent().Name().Equals(EventName))
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

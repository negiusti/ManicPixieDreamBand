using TMPro;
using UnityEngine;

public class OutdoorLocation : MonoBehaviour
{
    private TextMeshPro tmp;
    private bool inRange;
    private string location;

    // Start is called before the first frame update
    void Start()
    {
        tmp = this.GetComponentInChildren<TextMeshPro>();
        tmp.enabled = false;
        inRange = false;
        location = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inRange)
        {
            SceneChanger.Instance.ChangeScene(location);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tmp.enabled = true;
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tmp.enabled = false;
            inRange = false;
        }
    }
}

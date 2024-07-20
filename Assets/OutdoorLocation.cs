using TMPro;
using UnityEngine;

public class OutdoorLocation : MonoBehaviour
{
    private bool inRange;
    private string location;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        location = gameObject.name;
        DisableAllChildren();
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
            EnableAllChildren();
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisableAllChildren();
            inRange = false;
        }
    }

    private void SetChildrenActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    private void EnableAllChildren()
    {
        SetChildrenActive(true);
    }

    private void DisableAllChildren()
    {
        SetChildrenActive(false);
    }
}

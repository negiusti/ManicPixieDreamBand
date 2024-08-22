using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class OllieHint : MonoBehaviour
{
    private bool isOutside;
    private Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        isOutside = FindObjectsOfType<OutdoorLocation>().Length > 0;
        movement = GetComponentInParent<Movement>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.Instance.IsConversationActive && movement.IsSkating() && !Tutorial.hasOllied && Tutorial.hasSkated && Phone.Instance.IsLocked() && isOutside)
        {
            EnableAllChildren();
        }
        else
        {
            DisableAllChildren();
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

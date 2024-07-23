using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class WalkHint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.Instance.IsConversationActive && !Tutorial.hasWalked && Phone.Instance.IsLocked())
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToMeHint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HideButtonPrompt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowButtonPrompt()
    {
        EnableAllChildren();
    }

    public void HideButtonPrompt()
    {
        DisableAllChildren();
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

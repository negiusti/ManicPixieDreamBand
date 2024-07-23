using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneHint : MonoBehaviour
{
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.Instance.IsConversationActive && !Tutorial.hasClosedPhone)
        {
            sr.enabled = true;
        } else
        {
            sr.enabled = false;
        }
    }
}

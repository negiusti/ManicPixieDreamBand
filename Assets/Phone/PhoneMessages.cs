using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PhoneMessages : MonoBehaviour
{
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTestConvo()
    {
        canvas.enabled = true;
        DialogueManager.StartConversation("Booking Our First Show");
    }
}

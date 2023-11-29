using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Contact : MonoBehaviour
{
    private string contactName;
    private SpriteResolver spriteResolver;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteResolver = this.GetComponent<SpriteResolver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setContact(string name)
    {
        this.name = name;
        spriteResolver.SetCategoryAndLabel("Pic", contactName);
    }

    public void startConvo()
    {
        DialogueManager.StartConversation("TXT_" + name);
    }
}

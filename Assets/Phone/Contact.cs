using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Contact : MonoBehaviour
{
    private string contactName;
    private SpriteResolver spriteResolver;
    public PhoneMessages messages;
    public Phone phone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setContact(string contactName)
    {
        this.spriteResolver = this.GetComponent<SpriteResolver>();
        this.contactName = contactName;
        spriteResolver.SetCategoryAndLabel("Pic", contactName);
    }

    private void startConvo()
    {
        messages.OpenTxtConvoWith(contactName);
    }

    private void OnMouseDown()
    {
        startConvo();
    }
}

using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Contact : MonoBehaviour
{
    private string contactName;
    private SpriteResolver spriteResolver;
    public PhoneMessages messages;
    public Phone phone;
    private TextMeshPro contactNameTM;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetContact(string contactName)
    {
        this.spriteResolver = this.GetComponent<SpriteResolver>();
        this.contactName = contactName;
        this.contactNameTM = this.GetComponentInChildren<TextMeshPro>();
        contactNameTM.text = contactName;
        spriteResolver.SetCategoryAndLabel("Pic", contactName);
    }

    public void StartConvo()
    {
        messages.OpenTxtConvoWith(contactName);
    }

    private void OnMouseDown()
    {
        StartConvo();
    }

    
}

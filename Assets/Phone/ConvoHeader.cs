using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ConvoHeader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetConvoHeader(string contactName)
    {
        SpriteResolver sr = this.GetComponent<SpriteResolver>();
        sr.SetCategoryAndLabel("Pic", contactName);
        TextMeshPro tm = this.GetComponentInChildren<TextMeshPro>();
        tm.text = contactName;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumPadButton : MonoBehaviour
{
    public KeyCode keyCode;
    public string keyCodeString;
    public TextMeshPro textReadOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        string tmp = textReadOut.text;
        int idx = tmp.IndexOf('_');
        if (idx < 0)
        {
            tmp = keyCodeString + "___";
        }
        else
        {
            tmp = tmp.Insert(idx,keyCodeString).Substring(0, 4);
        }
        textReadOut.text = tmp;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppHeader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAppHeader(string appName)
    {
        TextMeshPro tm = this.GetComponent<TextMeshPro>();
        tm.text = appName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (GameManager.Instance == null)
            return;
        GameManager.Instance.GetComponent<MenuToggleScript>().EnableMenu();
    }
}

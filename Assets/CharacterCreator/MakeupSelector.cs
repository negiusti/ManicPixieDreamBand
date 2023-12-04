using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeupSelector : MonoBehaviour
{
    public string categoryName;
    public CharacterEditor characterEditor;

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
        characterEditor.SetCurrentFaceCategory(categoryName);
    }
}

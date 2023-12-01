using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    public string category;
    public CharacterEditor characterEditor;

    // Start is called before the first frame update
    void Start()
    {
        //pans = this.GetComponentsInChildren<PalettePan>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetColor(Color c)
    {
        characterEditor.SetCurrentFaceCategoryColor(c);
    }

}

using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    public CharacterEditor characterEditor;
    public string category;

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
        if (category.Equals("Skin"))
            characterEditor.SetSkinColor(c);
        else
            characterEditor.SetCurrentFaceCategoryColor(c);
    }

}

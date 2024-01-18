using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    private CharacterEditor characterEditor;
    public string category;

    // Start is called before the first frame update
    void Start()
    {
        characterEditor = GameObject.FindObjectOfType<CharacterEditor>();
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

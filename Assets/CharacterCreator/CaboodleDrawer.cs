using UnityEngine;

public class CaboodleDrawer : MonoBehaviour
{
    public GameObject content;
    public CaboodleDrawer[] otherDrawers;
    public string DefaultCategory;
    private CharacterEditor characterEditor;

    // Start is called before the first frame update
    void Start()
    {
        characterEditor = GameObject.FindObjectOfType<CharacterEditor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnEnable()
    //{
    //    if (content.activeSelf)
    //    {
    //        characterEditor.SetCurrentFaceCategory(DefaultCategory);
    //    }
    //}

    private void OnMouseDown()
    {
        bool switchedDrawer = !content.activeSelf;
        foreach (CaboodleDrawer drawer in otherDrawers)
        {
            drawer.UnselectDrawer();
        }
        content.SetActive(true);
        if (switchedDrawer)
        {
            characterEditor.SetCurrentFaceCategory(DefaultCategory);
        }
    }

    //public void SetDefaultSection()
    //{
    //    characterEditor.SetCurrentFaceCategory(DefaultCategory);
    //}

    public void UnselectDrawer()
    {
        content.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ColorPalettes : MonoBehaviour
{
    public ColorPalette hairPalette;
    public ColorPalette mouthPalette;
    public ColorPalette shadowPalette;
    public ColorPalette faceDetailPalette;
    private Dictionary<string, ColorPalette> categoryToColorPalette;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        categoryToColorPalette = new Dictionary<string, ColorPalette>();
        categoryToColorPalette.Add("Hair", hairPalette);
        categoryToColorPalette.Add("HiTails", hairPalette);
        categoryToColorPalette.Add("LoTails", hairPalette);
        categoryToColorPalette.Add("Bangs", hairPalette);
        categoryToColorPalette.Add("Eyebrows", hairPalette);
        categoryToColorPalette.Add("Face_Detail", faceDetailPalette);
        categoryToColorPalette.Add("Mouth", mouthPalette);
        categoryToColorPalette.Add("Eyeshadow", shadowPalette);
        animator.SetBool("Open", false);
    }

    public void SelectColorPalette(string category)
    {
        foreach (ColorPalette cp in categoryToColorPalette.Values)
        {
            cp.gameObject.SetActive(false);
        }
        Close();
        if (categoryToColorPalette.ContainsKey(category))
        {   
            categoryToColorPalette[category].gameObject.SetActive(true);
            Open();
        }
    }

    public void Open()
    {
        animator.SetBool("Open", true);
    }

    public void Close()
    {
        animator.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

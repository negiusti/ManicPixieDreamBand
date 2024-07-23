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
    private bool isOpen;
    
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
        //Close();
    }

    public void SelectColorPalette(string category)
    {
        if (categoryToColorPalette == null)
            Start();
        if (categoryToColorPalette.ContainsKey(category))
        {
            DisableColorPalettesExcept(categoryToColorPalette[category]);
            Debug.Log("Enabling color palette: " + category);
            categoryToColorPalette[category].gameObject.SetActive(true);
            Open();
        } else
        {
            Close();
        }
    }
    private void OnEnable()
    {
        if (animator != null)
            animator.SetBool("Open", isOpen);
    }

    public void Open()
    {
        Debug.Log("SETTING ANIMATOR BOOL TO TRUE");
        animator.SetBool("Open", true);
        Debug.Log("BOOL IS " + animator.GetBool("Open"));
        isOpen = true;
    }

    private void DisableColorPalettesExcept(ColorPalette p)
    {
        foreach (ColorPalette cp in categoryToColorPalette.Values)
        {
            if (p == null || cp != p)
                cp.gameObject.SetActive(false);
        }
    }

    private void DisableColorPalettes()
    {
        Debug.Log("Closing color palettes");
        foreach (ColorPalette cp in categoryToColorPalette.Values)
        {
            cp.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        Debug.Log("CLOSEING COLOR PALETTES");
        DisableColorPalettes();
        animator.SetBool("Open", false);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

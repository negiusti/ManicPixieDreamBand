using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    private PalettePan[] pans;
    private string currentCategory;
    private Dictionary<string, Color[]> colorOptionsPerCategory;
    private static Color pink = new Color(252 / 255f, 77 / 255f, 148 / 255f, 1f);
    private static Color orange = new Color(254 / 255f, 177 / 255f, 68 / 255f, 1f);
    private static Color purple = new Color(153 / 255f, 105 / 255f, 233 / 255f, 1f);
    private static Color green = new Color(158 / 255f, 224 / 255f, 158 / 255f, 1f);
    private static Color black = new Color(0.3f, 0.3f, 0.3f, 1f);
    private static Color red = new Color(1f, 102 / 255f, 99 / 255f, 1f);
    private static Color yellow = new Color(253 / 255f, 253 / 255f, 151 / 255f, 1f);
    private static Color blue = new Color(178 / 255f, 228 / 255f, 240 / 255f, 1f);
    private static Color brown = new Color(100 / 255f, 88 / 255f, 58 / 255f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        pans = this.GetComponentsInChildren<PalettePan>();
        colorOptionsPerCategory = new Dictionary<string, Color[]>();
        colorOptionsPerCategory.Add("Eyeshadow", new Color[]{pink, green, red, yellow, blue, brown, black, purple});
        colorOptionsPerCategory.Add("Hair", new Color[] { pink, green, red, yellow, blue, brown, black, purple });
        colorOptionsPerCategory.Add("Eyebrows", new Color[] { pink, green, red, yellow, blue, brown, black, purple });
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void ChangeColor(Color c)
    //{
    //    if (spriteRenderer != null)
    //        spriteRenderer.color = c;
    //}

    //public void SelectSR(SpriteRenderer sr)
    //{
    //    this.spriteRenderer = sr;

    //}

    public void SetCurrentFaceCategory(string category)
    {
        currentCategory = category;
        if (colorOptionsPerCategory.ContainsKey(currentCategory))
        {
            ShowPalette();
        } else
        {
            HidePalette();
        }
    }

    private void ShowPalette()
    {
        int i = 0;
        Color[] options = colorOptionsPerCategory.GetValueOrDefault(currentCategory);
        foreach (PalettePan pan in pans)
        {
            pan.gameObject.SetActive(true);
            if (i >= options.Length)
            {
                Debug.LogError("Not enough color options. There are " + pans.Length + " pans and "  + options.Length + " color options");
                return;
            }
            pan.SetColor(options[i++]);
        }
    }

    private void HidePalette()
    {
        foreach (PalettePan pan in pans)
        {
            pan.gameObject.SetActive(false);
        }
    }

}

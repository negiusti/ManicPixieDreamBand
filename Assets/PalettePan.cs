using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalettePan : MonoBehaviour
{
    private ColorPalette palette;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        palette = this.GetComponentInParent<ColorPalette>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color c)
    {
        image.color = c;
    }

    public Color GetColor()
    {
        return image.color;
    }

    private void OnMouseDown()
    {
        palette.SetColor(image.color);
    }
}

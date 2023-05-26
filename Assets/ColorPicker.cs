using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    private bool isSkin;
    private SpriteRenderer spriteRenderer;
    private int color = 0;
    private static Color pink = new Color(252/255f, 77 / 255f, 148/255f, 1f);
    private static Color orange = new Color(250/255f, 117 / 255f, 0f, 1f);
    private static Color purple = new Color(153 / 255f, 105/255f, 233/255f, 1f);
    private static Color green = new Color(48 / 255f, 174 / 255f, 0f, 1f);
    private static Color black = new Color(0.3f, 0.3f, 0.3f, 1f);
    private static Color red = new Color(254 /255f, 0f, 64/255f, 1f);
    private static Color yellow = new Color(233/255f, 229/255f, 0f, 1f);
    private static Color blue = new Color(0f, 201/255f, 1f, 1f);
    private static Color brown = new Color(100 / 255f, 88 / 255f, 58 / 255f, 1f);

    private Color[] colors = {pink, black, green, blue, red, yellow, brown, Color.white, Color.gray, orange, purple };
    private Color[] skinColors = { new Color(203 / 255f, 174 / 255f, 145 / 255f, 1f), new Color(1f, 213 / 255f, 170 / 255f, 1f), new Color(100 / 255f, 88 / 255f, 58 / 255f, 1f), new Color(135 / 255f, 114 / 255f, 79 / 255f, 1f), new Color(193 / 255f, 171 / 255f, 118 / 255f, 1f), new Color(1f, 209 / 255f, 170 / 255f, 1f), new Color(174 / 255f, 154 / 255f, 127 / 255f, 1f), new Color(176 / 255f, 241 / 255f, 92 / 255f, 1f), new Color(162 / 255f, 92 / 255f, 210 / 255f, 1f) };

    // Start is called before the first frame update
    void Start()
    {
        isSkin = this.tag == "BodyPart";
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isSkin)
        {
            spriteRenderer.color = skinColors[color];
        } else
        {
            spriteRenderer.color = colors[color];
        }
    }

    public int NumColors()
    {
        if (isSkin)
            return skinColors.Length;
        else
            return colors.Length;
    }

    public void ChangeColor()
    {
        color++;
        if (isSkin)
        {
            if (color >= skinColors.Length)
            {
                color = 0;
            }
            spriteRenderer.color = skinColors[color];
        } else
        {
            if (color >= colors.Length)
            {
                color = 0;
            }
            spriteRenderer.color = colors[color];
        }
    }

    public void SetColor(int index)
    {
        color = index;
        if (isSkin)
        {
            if (index >= skinColors.Length)
            {
                index = 0;
            }
            spriteRenderer.color = skinColors[index];
        }
        else
        {
            if (index >= colors.Length)
            {
                index = 0;
            }
            spriteRenderer.color = colors[index];
        }
    }

    public int GetColor()
    {
        return color;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    public Texture2D texture;
    public SpriteRenderer spriteRenderer;
    public int eraseRadius;

    private Color[] transparentPixels;
    private Vector2Int textureSize;
    private Color[] originalPixels; // Store the original pixels

    void Start()
    {
        // Ensure the texture is readable and writable
        texture = Instantiate(spriteRenderer.sprite.texture);
        textureSize = new Vector2Int(texture.width, texture.height);
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        originalPixels = (Color[])texture.GetPixels().Clone(); // Save the original state of the pixels

        // Create an array of transparent pixels
        transparentPixels = new Color[eraseRadius * eraseRadius * 4];
        for (int i = 0; i < transparentPixels.Length; i++)
        {
            transparentPixels[i] = new Color(0, 0, 0, 0);
        }
    }

    private void ResetTexture()
    {
        // Copy the original pixels back to the texturePixels array
        //System.Array.Copy(originalPixels, texturePixels, texturePixels.Length);

        // Apply the original pixels back to the texture
        texture.SetPixels(originalPixels);
        texture.Apply();

        // Reset the erased visible pixels counter
        //erasedVisiblePixels = 0;

        Debug.Log("Texture has been reset to its original state.");
    }

    private void OnEnable()
    {
        if (texture == null)
            return;
        ResetTexture();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0)) // Check if the left mouse button is pressed
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 localPoint = transform.InverseTransformPoint(mousePosition);
            Vector2Int pixelPosition = WorldToTextureCoords(localPoint);

            EraseAt(pixelPosition);
        }
    }

    Vector2Int WorldToTextureCoords(Vector2 localPoint)
    {
        float ppu = spriteRenderer.sprite.pixelsPerUnit;
        Vector2 pivot = spriteRenderer.sprite.pivot / ppu;
        Vector2 coord = localPoint + pivot;
        Vector2Int pixelPos = new Vector2Int(Mathf.RoundToInt(coord.x * ppu), Mathf.RoundToInt(coord.y * ppu));
        return pixelPos;
    }

    void EraseAt(Vector2Int position)
    {
        int startX = Mathf.Clamp(position.x - eraseRadius, 0, textureSize.x);
        int startY = Mathf.Clamp(position.y - eraseRadius, 0, textureSize.y);
        int endX = Mathf.Clamp(position.x + eraseRadius, 0, textureSize.x);
        int endY = Mathf.Clamp(position.y + eraseRadius, 0, textureSize.y);

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                if (Vector2Int.Distance(new Vector2Int(x, y), position) <= eraseRadius)
                {
                    texture.SetPixels(x, y, 1, 1, transparentPixels);
                }
            }
        }

        texture.Apply();
    }

    //SpriteMask Mask;
    //Color[] Colors;
    //int Width;
    //int Height;

    //void Start()
    //{
    //    //Get objects
    //    //Mask = GameObject.Find("Mask").GetComponent<SpriteMask>();
    //    Mask = GetComponent<SpriteMask>();

    //    //Extract color data once
    //    Colors = Mask.sprite.texture.GetPixels();

    //    //Store mask dimensionns
    //    Width = Mask.sprite.texture.width;
    //    Height = Mask.sprite.texture.height;

    //    ClearMask();
    //}

    //void ClearMask()
    //{
    //    //set all color data to transparent
    //    for (int i = 0; i < Colors.Length; ++i)
    //    {
    //        Colors[i] = new Color(1, 1, 1, 0);
    //    }

    //    Mask.sprite.texture.SetPixels(Colors);
    //    Mask.sprite.texture.Apply(false);
    //}

    ////This function will draw a circle onto the texture at position cx, cy with radius r
    //public void DrawOnMask(int cx, int cy, int r)
    //{
    //    int px, nx, py, ny, d;

    //    for (int x = 0; x <= r; x++)
    //    {
    //        d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));

    //        for (int y = 0; y <= d; y++)
    //        {
    //            px = cx + x;
    //            nx = cx - x;
    //            py = cy + y;
    //            ny = cy - y;

    //            Colors[py * Width + px] = new Color(1, 1, 1, 1);
    //            Colors[py * Width + nx] = new Color(1, 1, 1, 1);
    //            Colors[ny * Height + px] = new Color(1, 1, 1, 1);
    //            Colors[ny * Height + nx] = new Color(1, 1, 1, 1);
    //        }
    //    }

    //    Mask.sprite.texture.SetPixels(Colors);
    //    Mask.sprite.texture.Apply(false);
    //}


    //void Update()
    //{

    //    ////Get mouse coordinates
    //    //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    ////Check if mouse button is held down
    //    //if (Input.GetMouseButton(0))
    //    //{
    //    //    //Check if we hit the collider
    //    //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
    //    //    if (hit.collider != null)
    //    //    {
    //    //        //Normalize to the texture coodinates
    //    //        int y = (int)((0.5 - (Mask.transform.position - mousePosition).y) * Height);
    //    //        int x = (int)((0.5 - (Mask.transform.position - mousePosition).x) * Width);

    //    //        //Draw onto the mask
    //    //        DrawOnMask(x, y, 5);
    //    //    }
    //    //}
    //}

    //private void OnMouseDrag()
    //{
    //    //Get mouse coordinates
    //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    //Check if we hit the collider
    //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
    //    if (hit.collider != null)
    //    {
    //        //Normalize to the texture coodinates
    //        int y = (int)((0.5 - (Mask.transform.position - mousePosition).y) * Height);
    //        int x = (int)((0.5 - (Mask.transform.position - mousePosition).x) * Width);

    //        //Draw onto the mask
    //        DrawOnMask(x, y, 5);
    //    }
    //}

}

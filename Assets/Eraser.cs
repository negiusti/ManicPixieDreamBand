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
    private int totalPixels;

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
        totalPixels = textureSize.x * textureSize.y;
    }

    public float EraseCompletionPercentage()
    {
        if (texture == null)
            Start();
        // Count visible pixels
        int visiblePixels = 0;
        Color[] currentPixels = texture.GetPixels();
        foreach (Color pixel in currentPixels)
        {
            if (pixel.a > 0.01f) // If pixel is not fully transparent
            {
                visiblePixels++;
            }
        }

        float erasedPercentage = 1.0f - ((float)visiblePixels / totalPixels);
        return erasedPercentage;
    }

    private void ResetTexture()
    {
        // Copy the original pixels back to the texturePixels array
        //System.Array.Copy(originalPixels, texturePixels, texturePixels.Length);

        // Apply the original pixels back to the texture
        texture.SetPixels(originalPixels);
        texture.Apply();

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
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteCombiner", menuName = "Custom/Sprite Combination")]
public class SpriteCombiner : ScriptableObject
{
    [SerializeField]
    //private Sprite[] sprites;

    //public GameObject[] designObjects; // Assign objects with the "design" tag in the Inspector

    //public string screenshotName = "CombinedScreenshot";
    public int width = 1920;
    public int height = 1080;
    public LayerMask objectsToCheck; // Assign the layers of objects you want to check in the Inspector
    private Collider2D[] touchingObjects;


    //public void AddSprite(Sprite sprite)
    //{
    //    if (sprites == null)
    //        sprites = new Sprite[] { sprite };
    //    else
    //    {
    //        int length = sprites.Length;
    //        Sprite[] newSprites = new Sprite[length + 1];
    //        for (int i = 0; i < length; i++)
    //        {
    //            newSprites[i] = sprites[i];
    //        }
    //        newSprites[length] = sprite;
    //        sprites = newSprites;
    //    }
    //}

    private void DisableNonDesignSprites()
    {
        objectsToCheck = LayerMask.NameToLayer("Default");
        // Find the "designcanvas" game object
        GameObject designCanvas = GameObject.FindWithTag("DesignCanvas");
        if (designCanvas == null)
        {
            Debug.LogWarning("No 'designcanvas' object found.");
        }

        // Get the collider of the "designcanvas" object
        Collider2D designCanvasCollider = designCanvas.GetComponent<Collider2D>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (!obj.CompareTag("DesignCanvas") && (!obj.CompareTag("Design") || !designCanvasCollider.IsTouching(obj.GetComponent<Collider2D>())))
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    //obj.SetActive(true);
                    Debug.Log("Enable " + obj.tag);
                    spriteRenderer.enabled = false;
                }
            }
        }
    }

    private void EnableNonDesignSprites()
    {
        objectsToCheck = LayerMask.NameToLayer("Default");
        // Find the "designcanvas" game object
        GameObject designCanvas = GameObject.FindWithTag("DesignCanvas");
        if (designCanvas == null)
        {
            Debug.LogWarning("No 'designcanvas' object found.");
        }

        // Get the collider of the "designcanvas" object
        Collider2D designCanvasCollider = designCanvas.GetComponent<Collider2D>();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (!obj.CompareTag("DesignCanvas") && (!obj.CompareTag("Design") || !designCanvasCollider.IsTouching(obj.GetComponent<Collider2D>())))
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    //obj.SetActive(true);
                    Debug.Log("Enable " + obj.tag);
                    spriteRenderer.enabled = true;
                }
            }
        }
    }

    //public Sprite CombineSprites2(Camera camera)
    //{
    //    // Find the "designcanvas" game object
    //    GameObject designCanvas = GameObject.FindWithTag("designcanvas");
    //    if (designCanvas == null)
    //    {
    //        Debug.LogWarning("No 'designcanvas' object found.");
    //    }

    //    // Get the collider of the "designcanvas" object
    //    Collider2D designCanvasCollider = designCanvas.GetComponent<Collider2D>();

    //    // Check for overlaps with the specified layers
    //    touchingObjects = Physics2D.OverlapBoxAll(designCanvasCollider.bounds.center, designCanvasCollider.bounds.extents, 0, objectsToCheck);

    //    // Process the overlapping objects
    //    foreach (Collider2D objCollider in touchingObjects)
    //    {
    //        GameObject obj = objCollider.gameObject;
    //        // Do something with the touching objects
    //        Debug.Log("Object with tag '" + obj.tag + "' is touching the 'designcanvas'.");
    //    }
    //}

    public Sprite CombineSprites(Camera cam)
    {
        // Activate the camera for taking the screenshot
        //Camera captureCamera = GetComponent<Camera>();
        //captureCamera.enabled = true;
        Camera captureCamera = Instantiate<Camera>(cam);
        GameObject[] designObjects = GameObject.FindGameObjectsWithTag("Design");

        // Combine all design objects into one GameObject
        //GameObject combinedDesign = new GameObject("CombinedDesign");

        //foreach (GameObject obj in designObjects)
        //{
        //    if (obj != null)
        //    {
        //        obj.transform.parent = combinedDesign.transform;
        //    }
        //}

        // Position the camera to capture the combined design objects
        //captureCamera.transform.position = combinedDesign.transform.position + new Vector3(0, 0, -10);
        captureCamera.orthographicSize = Mathf.Max(width, height) / 200.0f;
        DisableNonDesignSprites();
        // Create a RenderTexture to capture the screenshot
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        captureCamera.targetTexture = renderTexture;

        // Capture the screenshot
        captureCamera.Render();

        // Create a Texture2D from the RenderTexture
        Texture2D screenshot = new Texture2D(width, height);
        RenderTexture.active = renderTexture;

        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();
        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        // Combine the captured screenshot into one sprite
        Sprite combinedSprite = Sprite.Create(screenshot, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));

        //Destroy(combinedDesign);
        EnableNonDesignSprites();
        Destroy(captureCamera);
        // Use the combined sprite in a SpriteRenderer or save it to a file
        return combinedSprite;

        // Deactivate the capture camera
        //captureCamera.enabled = false;


        //GameObject[] designCanvasObjects = GameObject.FindGameObjectsWithTag("designcanvas");

        //// Loop through the found objects
        //foreach (GameObject designCanvas in designCanvasObjects)
        //{

        //}
        //if (sprites == null || sprites.Length == 0)
        //{
        //    Debug.LogWarning("No sprites to combine.");
        //    return null;
        //}

        //int width = 0;
        //int height = 0;

        //// Calculate the width and height of the combined texture
        //foreach (Sprite sprite in sprites)
        //{
        //    width += (int)sprite.rect.width;
        //    height = Mathf.Max(height, (int)sprite.rect.height);
        //}

        //Texture2D combinedTexture = new Texture2D(width, height);

        //int xOffset = 0;

        //// Combine the sprites into the combined texture
        //foreach (Sprite sprite in sprites)
        //{
        //    Color[] pixels = sprite.texture.GetPixels();
        //    combinedTexture.SetPixels(xOffset, 0, (int)sprite.rect.width, (int)sprite.rect.height, pixels);
        //    xOffset += (int)sprite.rect.width;
        //}

        //combinedTexture.Apply();

        //return Sprite.Create(combinedTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }


}

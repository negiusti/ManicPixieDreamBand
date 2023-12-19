using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TakeMyPhoto : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;
    public GameObject canvas;
    public InputField emailInput;
    public InputField igInput;
    private ContactInfo contactInfo;

    [System.Serializable]
    private class ContactInfo
    {
        public string email;
        public string ig;
        public bool canShare;
    }
    
    private long dt;

    // Start is called before the first frame update
    void Start()
    {
        dt = DateTime.Now.Ticks;
        contactInfo = new ContactInfo();

        // Use this for initialization
        // set the desired aspect ratio, I set it to fit every screen 
        float targetaspect = (float)Screen.width / (float)Screen.height;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = Camera.main;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add container box
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSharingPermission(bool shareable)
    {
        contactInfo.canShare = shareable;
    }

    public void SetEmail()
    {
        contactInfo.email = emailInput.text;
    }

    public void SetIG()
    {
        contactInfo.ig = igInput.text;
    }

    private void DisableNonDesignSprites()
    {
        canvas.SetActive(false);
    }

    private void EnableNonDesignSprites()
    {
        canvas.SetActive(true);
    }

    public Sprite CombineSprites()
    {
        // Activate the camera for taking the screenshot
        //Camera captureCamera = GetComponent<Camera>();
        //captureCamera.enabled = true;
        Camera captureCamera = Instantiate<Camera>(Camera.main);

        // Position the camera to capture the combined design objects
        //captureCamera.transform.position = combinedDesign.transform.position + new Vector3(0, 0, -10);
        //captureCamera.orthographicSize = Mathf.Max(width, height) / 200.0f;
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

        EnableNonDesignSprites();
        // Deactivate the capture camera
        captureCamera.enabled = false;
        Destroy(captureCamera.gameObject);

        return combinedSprite;
    }

    public void SaveSpriteAsPNG()
    {
        Sprite spriteToSave = CombineSprites();
        if (spriteToSave == null)
        {
            Debug.LogError("Sprite not assigned.");
            return;
        }

        // Convert the Sprite to a Texture2D
        Texture2D texture = spriteToSave.texture;

        // Encode the Texture2D as a PNG file
        byte[] bytes = texture.EncodeToPNG();

        // Specify the file path
        string filePath = Application.persistentDataPath + "/MagFestDemo/" + dt + "/savedSprite.png";

        string directoryPath = Path.GetDirectoryName(filePath);
        Directory.CreateDirectory(directoryPath);

        // Write the bytes to the file
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Sprite saved as PNG: " + filePath);

        string contactInfoFile = Application.persistentDataPath + "/MagFestDemo/" + dt + "/contact.json";
        //Dictionary<string, string> contactInfo = new Dictionary<string, string>
        //{
        //    { "email", email },
        //    { "ig", ig },
        //    { "canShare", canShare.ToString() }
        //};

        // Convert the dictionary to JSON
        string json = JsonUtility.ToJson(contactInfo);

        // Write the JSON to a file
        File.WriteAllText(contactInfoFile, json);
    }
}

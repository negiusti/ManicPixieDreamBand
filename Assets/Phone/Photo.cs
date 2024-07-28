using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    private SpriteResolver resolver;
    private SpriteRenderer spriteRenderer;
    private Image image;
    private TextMeshProUGUI tmp;
    private static Dictionary<string, string> photoToCaption = new() { { "PizzaRat", "This city stinks" }, { "Boxes", "God I hate moving" }, { "Band", "Joined a band, I guess..." } };
    // Start is called before the first frame update
    void Start()
    {
        resolver = GetComponent<SpriteResolver>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignPhoto(string photoName)
    {
        if (resolver == null)
            Start();
        UpdateImg(photoName);
        tmp.text = photoToCaption[photoName] ?? "";
    }

    private void UpdateImg(string label)
    {
        if (resolver == null)
            Start();
        resolver.SetCategoryAndLabel("Photos", label);
        resolver.ResolveSpriteToSpriteRenderer();
        image.sprite = spriteRenderer.sprite;
    }
}

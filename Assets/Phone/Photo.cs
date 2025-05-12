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
    private static Dictionary<string, string> photoToCaption = new() {
        { "_PizzaRat", "This city STINKS" },
        { "_Boxes", "A fresh start :')" },
        { "_Band", "I think I'm in the band??" },
        { "_Party1", "Zombie party at Ricki's!!" },
        { "_Party2", "They want me to join their band??" },
        { "_RexHospital", "Glad they're better now :)" },
        { "_Couch", "i need a tetanus shot..." },
        { "_Maracas", "eith is so creative!" },
        { "_MarchingBand", "still the same dorks today" },
        { "_MaxShirts", "spot the difference" },
        { "_Pickles", "ew..." },
        { "_Flyer", "look mom i'm famous!" },
        { "_Ollie", "bestest buds" },
        { "_Pedals", "bzzzzzz" },
        { "_Pixie", "wow!!" },
        { "_Ube", "omg *drool*" },
        { "_Hugs", "I really needed this hug!" },
        { "_Beautiful", "<3 mom" },
        { "_Daisy", "they're so cute together!" },
        { "_Melissa", "drawn by @furry_dragon_art999" }
    };
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

        if (!photoToCaption.ContainsKey(photoName)) {
            Debug.Log("oops");
            return;
        }

        UpdateImg(photoName);
        tmp.text = photoToCaption[photoName] ?? "";
    }

    private void UpdateImg(string label)
    {
        if (resolver == null)
            Start();
        resolver.SetCategoryAndLabel("Pics", label);
        resolver.ResolveSpriteToSpriteRenderer();
        image.sprite = spriteRenderer.sprite;
    }
}

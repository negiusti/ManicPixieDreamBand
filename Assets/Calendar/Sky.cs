using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Sky : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = this.GetComponent<SpriteResolver>();
        UpdateSky();
    }

    public void UpdateSky()
    {
        string label = Calendar.IsDay() ? "Day" : "Night";
        if (spriteResolver == null)
            Start();
        spriteResolver.SetCategoryAndLabel("Sky", label);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

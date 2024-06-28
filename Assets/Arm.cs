using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Arm : MonoBehaviour
{
    public Color[] skinColorOptions;
    private SpriteResolver resolver;
    private SpriteRenderer sr;
    //private SpriteLibrary spriteLib;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        resolver = GetComponent<SpriteResolver>();
        //spriteLib = GetComponent<SpriteLibrary>();
        resolver.SetCategoryAndLabel(resolver.GetCategory(), Random.Range(1, 4).ToString());
        sr.color = skinColorOptions[Random.Range(0, skinColorOptions.Length)];
    }

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        resolver = GetComponent<SpriteResolver>();
        //spriteLib = GetComponent<SpriteLibrary>();
        resolver.SetCategoryAndLabel(resolver.GetCategory(), Random.Range(1, 4).ToString());
        sr.color = skinColorOptions[Random.Range(0, skinColorOptions.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

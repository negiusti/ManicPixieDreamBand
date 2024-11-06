using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongSelection : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TextMeshPro txt;
    private Color unhilightedTxtColor;
    private Color highlightedTxtColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        txt = GetComponent<TextMeshPro>();
        highlightedTxtColor = Color.white;
        unhilightedTxtColor = txt.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight()
    {
        if (spriteRenderer == null)
            Start();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 5.31f);
        spriteRenderer.gameObject.SetActive(true);
        txt.color = highlightedTxtColor;
    }

    public void Unhighlight()
    {
        if (spriteRenderer == null)
            Start();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        spriteRenderer.gameObject.SetActive(false);
        txt.color = unhilightedTxtColor;
    }
}

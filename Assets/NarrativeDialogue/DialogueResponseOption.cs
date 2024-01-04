using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueResponseOption : MonoBehaviour
{
    public GameObject UpArrow;
    public GameObject DownArrow;
    public Sprite top;
    public Sprite middle;
    public Sprite bottom;
    private Text text;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponentInChildren<Text>();
        button = this.GetComponent<Button>();
        Deselect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        if (button != null)
            button.image.color = new Color(0.6276432f, 0.5379584f, 0.7264151f, 1);
        if (text != null)
            text.color = new Color(1, 1, 1, 1);
    }

    public void SetTop()
    {
        if (button != null)
            button.image.sprite = top;
    }

    public void SetMiddle()
    {
        if (button != null)
            button.image.sprite = middle;
    }

    public void SetBottom()
    {
        if (button != null)
            button.image.sprite = bottom;
    }

    public void Deselect()
    {
        UpArrow.SetActive(false);
        DownArrow.SetActive(false);
        if (button != null)
            button.image.color = new Color(0, 0, 0, 0);
        if (text != null)
            text.color = new Color(0, 0, 0, 1);
    }

    public void ShowDownArrow()
    {
        DownArrow.SetActive(true);
    }

    public void ShowUpArrow()
    {
        UpArrow.SetActive(true);
    }
}

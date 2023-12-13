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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        button.image.color = new Color(0, 0, 1, 1);
        text.color = new Color(1, 1, 1, 1);
    }

    public void SetTop()
    {
        button.image.sprite = top;
    }

    public void SetMiddle()
    {
        button.image.sprite = middle;
    }

    public void SetBottom()
    {
        button.image.sprite = bottom;
    }

    public void Deselect()
    {
        UpArrow.SetActive(false);
        DownArrow.SetActive(false);
        button.image.color = new Color(0, 0, 0, 0);
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

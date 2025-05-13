using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class BobaOrderItem : MonoBehaviour
{
    public BobaMiniGame.Step step;
    public SpriteRenderer line;
    private SpriteResolver iconResolver;
    private Animator animator;
    private SpriteLibrary spriteLib;
    private TextMeshPro tmp;

    // Start is called before the first frame update
    void Start()
    {
        iconResolver = GetComponentInChildren<SpriteResolver>();
        animator = GetComponent<Animator>();
        spriteLib = GetComponentInParent<SpriteLibrary>();
        tmp = GetComponent<TextMeshPro>();
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetText()
    {
        switch (step)
        {
            case BobaMiniGame.Step.Milk:
                tmp.text = iconResolver.GetLabel() + " Milk";
                break;
            case BobaMiniGame.Step.Flavor:
                tmp.text = iconResolver.GetLabel();
                break;
            case BobaMiniGame.Step.Ice:
                tmp.text = "ICE LVL";
                break;
            case BobaMiniGame.Step.Toppings:
                tmp.text = "w/" + iconResolver.GetLabel();
                break;
            case BobaMiniGame.Step.Done:
                break;
        }
    }

    public void Randomize()
    {
        if (spriteLib == null)
            Start();
        string[] labels = spriteLib.spriteLibraryAsset.GetCategoryLabelNames(iconResolver.GetCategory()).ToArray();
        string label = labels[Random.Range(0, labels.Length)];
        iconResolver.SetCategoryAndLabel(iconResolver.GetCategory(), label);
        SetText();
        line.gameObject.SetActive(false);
    }

    public string CurrentRequestedItem()
    {
        return iconResolver.GetLabel();
    }

    public void Check()
    {
        line.gameObject.SetActive(true);
        line.color = new Color(24f/255f, 155f/255f, 20f/255f,200f/255f);//Color.green;
        animator.Play("OrderItemCross", -1, 0f);
    }

    public void Fail()
    {
        line.gameObject.SetActive(true);
        line.color = new Color(228f/255f,8f/255f,31f/255f,200f/255f);//Color.red;
        animator.Play("OrderItemCross", -1, 0f);
    }
    
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ShirtIcon : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    private Animator animator;
    private TextMeshPro moneyScoreText;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        animator = GetComponent<Animator>();
        moneyScoreText = GetComponentInChildren<TextMeshPro>(includeInactive:true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeIcon(string label)
    {
        if (spriteResolver == null)
            Start();
        spriteResolver.SetCategoryAndLabel("ShirtIcon", label);
        animator.Play("Pop");
    }

    public void Reset()
    {
        ChangeIcon("Empty");
        moneyScoreText.text = "";
    }

    public void Success(int score)
    {
        ChangeIcon("Success");
        moneyScoreText.text = "+$" + score;
        moneyScoreText.color = Color.green;
    }

    public void Failure(int score)
    {
        ChangeIcon("Failure");
        moneyScoreText.color = Color.red;
        moneyScoreText.text = "-$" + Math.Abs(score);
    }

    public void Misprint(int score)
    {
        ChangeIcon("Misprint");
        moneyScoreText.color = Color.yellow;
        moneyScoreText.text = "+$" + score;
    }
}

using UnityEngine;
using UnityEngine.U2D.Animation;

public class ShirtIcon : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeIcon(string label)
    {
        spriteResolver.SetCategoryAndLabel("ShirtIcon", label);
        animator.Play("Pop");
    }

    public void Reset()
    {
        ChangeIcon("Empty");
    }

    public void Success()
    {
        ChangeIcon("Success");
    }

    public void Failure()
    {
        ChangeIcon("Failure");
    }

    public void Misprint()
    {
        ChangeIcon("Misprint");
    }
}

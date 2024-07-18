using UnityEngine;

public class CaboodleSection : MonoBehaviour
{
    private Animator animator;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        mat.SetColor("_Color", new Color(.7f, .7f, .7f));
    }

    private void OnMouseExit()
    {
        mat.SetColor("_Color", Color.white);
    }

    public void Select()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Selected", true);
    }

    public void Unselect()
    {
        if (animator != null)
            animator.SetBool("Selected", false);
    }

    public void DisableSection()
    {
        Unselect();
    }

    public void EnableSection()
    {
    }
}

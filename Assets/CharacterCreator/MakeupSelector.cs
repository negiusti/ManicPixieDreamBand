using TMPro;
using UnityEngine;

public class MakeupSelector : MonoBehaviour
{
    public string categoryName;
    private CharacterEditor characterEditor;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;
    private MakeupSelector[] selectors;
    public Material defaultMat;
    public Material outlineMat;
    private Animator anim;
    public string hintText;
    private TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        originalScale = this.spriteRenderer.gameObject.transform.localScale;
        selectors = this.transform.parent.gameObject.GetComponentsInChildren<MakeupSelector>();
        characterEditor = GameObject.FindObjectOfType<CharacterEditor>();
        spriteRenderer.material = defaultMat;
        anim = this.GetComponent<Animator>();
        anim.enabled = false;
        tmp = this.GetComponentInChildren<TextMeshPro>();
        tmp.text = hintText;
        tmp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        Select();
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = originalScale * 1.1f;
        this.spriteRenderer.gameObject.transform.localScale = newScale;
        //anim.CrossFade("MakeupMenu_Anim", 0f, 0);
        //anim.Play("MakeupMenu_Anim", 0, 0f);
        tmp.enabled = true;
    }

    private void OnMouseExit()
    {
        spriteRenderer.gameObject.transform.localScale = originalScale;
        tmp.enabled = false;
    }

    public void UnSelect()
    {
        spriteRenderer.material = defaultMat;
        anim.enabled = false;
    }

    private void UnSelectAll()
    {
        foreach (MakeupSelector selector in selectors)
        {
            selector.UnSelect();
        }
    }

    public void Select()
    {
        //anim.ResetTrigger("MakeupSelectorTrigger");
        //anim.SetTrigger("MakeupSelectorTrigger");
        UnSelectAll();
        anim.enabled = true;
        anim.Play("MakeupMenu_Anim", 0, 0f);
        characterEditor.SetCurrentFaceCategory(categoryName);
        spriteRenderer.material = outlineMat;
    }
}

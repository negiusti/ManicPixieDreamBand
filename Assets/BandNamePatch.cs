using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandNamePatch : MonoBehaviour
{
    private Animator animator;
    public string bandName;
    private BandNameMiniGame mg;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mg = GetComponentInParent<BandNameMiniGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        animator.SetBool("Hovering", true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("Hovering", false);
    }

    private void OnMouseDown()
    {
        mg.SelectBandName(bandName);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        animator.Play("PatchDestroy", -1, 0f);
    }
}

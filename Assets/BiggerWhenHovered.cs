using UnityEngine;

public class BiggerWhenHovered : MonoBehaviour
{
    private Vector3 originalScale;
    public float scaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = this.gameObject.transform.localScale;
        if (scaleFactor < 1f)
        {
            scaleFactor = 1.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseExit()
    {    
        this.gameObject.transform.localScale = originalScale;
    }

    private void OnMouseEnter()
    {
        Vector3 newScale = originalScale * scaleFactor;
        this.gameObject.transform.localScale = newScale;
    }
}
